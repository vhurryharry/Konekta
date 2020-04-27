using FluentValidation;
using MediatR;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Extensions;
using WCA.Core.Features.Pexa;
using WCA.Data;
using WCA.Domain.Actionstep;
using WCA.Domain.CQRS;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class CreatePexaWorkspaceCommandHandler : IRequestHandler<CreatePexaWorkspaceCommand, CreatePexaWorkspaceResponse>
    {
        private readonly CreatePexaWorkspaceCommand.ValidatorCollection _validator;
        private readonly IClock _clock;
        private readonly IExtendedPexaService _pexaService;
        private readonly IEventSourcedAggregateRepository<ActionstepMatter> _actionstepMatterRepository;
        private readonly WCADbContext _wCADbContext;

        public CreatePexaWorkspaceCommandHandler(
            CreatePexaWorkspaceCommand.ValidatorCollection validator,
            IClock clock,
            IExtendedPexaService pexaService,
            IEventSourcedAggregateRepository<ActionstepMatter> actionstepMatterRepository,
            WCADbContext wCADbContext
            )
        {
            _validator = validator;
            _clock = clock;
            _pexaService = pexaService;
            _actionstepMatterRepository = actionstepMatterRepository;
            _wCADbContext = wCADbContext;
        }

        public async Task<CreatePexaWorkspaceResponse> Handle(CreatePexaWorkspaceCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await _validator.ValidateAndThrowAsync(command);

            // Check that the user has valid Actionstep credentials for the given Actionstep Matter in Subscriber Reference
            if (!_wCADbContext.ActionstepCredentials.UserHasValidCredentialsForOrg(command.AuthenticatedUser, command.OrgKey))
            {
                throw new UnauthorizedAccessException($"User {command.AuthenticatedUser.Id} does not have valid Actionstep credentials for org {command.OrgKey} which is specified in the PEXA Subscriber Reference field.");
            }

            // Load Actionstep matter aggregate. SubscriberReference should already be in correct Aggregate Id format
            var actionstepMatter = _actionstepMatterRepository.GetById(command.PexaWorkspaceCreationRequest.SubscriberReference);
            if (actionstepMatter.Id == null)
                actionstepMatter = new ActionstepMatter(_clock.GetCurrentInstant(), command.OrgKey, command.MatterId, command.AuthenticatedUser.Id);

            PexaWorkspaceCreationRequested pexaWorkspaceCreationRequested = null;

            try
            {
                // Mark request for PEXA workspace creation
                pexaWorkspaceCreationRequested = actionstepMatter.RequestPexaWorkspaceCreation(_clock.GetCurrentInstant(), command.AuthenticatedUser.Id);
                _actionstepMatterRepository.Save(actionstepMatter);

                // Call PEXA and attempt to create workspace
                var workspaceCreationResponse = await _pexaService.Handle<WorkspaceCreationResponse>(
                        new WorkspaceCreationRequestCommand(command.PexaWorkspaceCreationRequest, command.AccessToken), command.AuthenticatedUser, cancellationToken);

                var urlSafeId = Uri.EscapeDataString(workspaceCreationResponse.WorkspaceId);

                if (string.IsNullOrEmpty(urlSafeId))
                {
                    throw new PexaUnexpectedErrorResponseException("PEXA Request came back successful but there was no PEXA Workspace ID.");
                }

                // If sucessful, save immediately
                actionstepMatter.MarkPexaWorkspaceCreated(_clock.GetCurrentInstant(), workspaceCreationResponse.WorkspaceId, pexaWorkspaceCreationRequested);
                _actionstepMatterRepository.Save(actionstepMatter);

                var workspaceUri = _pexaService.GetWorkspaceUri(workspaceCreationResponse.WorkspaceId, command.PexaWorkspaceCreationRequest.Role);
                var invitationUri = _pexaService.GetInvitationUri(workspaceCreationResponse.WorkspaceId, command.PexaWorkspaceCreationRequest.Role);

                return new CreatePexaWorkspaceResponse(workspaceUri, workspaceCreationResponse.WorkspaceId, invitationUri);
            }
            catch (PexaWorkspaceAlreadyExistsException ex)
            {
                var workspaceUri = _pexaService.GetWorkspaceUri(ex.WorkspaceId, command.PexaWorkspaceCreationRequest.Role);
                var invitationUri = _pexaService.GetInvitationUri(ex.WorkspaceId, command.PexaWorkspaceCreationRequest.Role);
                return new CreatePexaWorkspaceResponse(workspaceUri, ex.WorkspaceId, invitationUri, true);
            }
            catch (Exception ex)
            {
                actionstepMatter.MarkPexaWorkspaceCreationFailed(_clock.GetCurrentInstant(), ex.Message, pexaWorkspaceCreationRequested);
                _actionstepMatterRepository.Save(actionstepMatter);
                throw;
            }
        }
    }
}
