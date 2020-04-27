using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Features.Pexa;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class CreatePexaWorkspaceInvitationCommandHandler : IRequestHandler<CreatePexaWorkspaceInvitationCommand, CreateWorkspaceInvitationResponseType[]>
    {
        private readonly CreatePexaWorkspaceInvitationCommand.ValidatorCollection _validator;
        private readonly IExtendedPexaService _pexaService;

        public CreatePexaWorkspaceInvitationCommandHandler(
            CreatePexaWorkspaceInvitationCommand.ValidatorCollection validator,
            IExtendedPexaService pexaService)
        {
            _validator = validator;
            _pexaService = pexaService;
        }

        public async Task<CreateWorkspaceInvitationResponseType[]> Handle(CreatePexaWorkspaceInvitationCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await _validator.ValidateAndThrowAsync(command);

            var response = new List<CreateWorkspaceInvitationResponseType>();

            foreach (var invitationRequest in command.PexaWorkspaceInvitationRequests)
            {
                var workspaceInvitationResponse = await _pexaService.Handle<CreateWorkspaceInvitationResponseType>(
                        new WorkspaceInvitationRequestCommand(invitationRequest, command.AccessToken), command.AuthenticatedUser, cancellationToken);

                response.Add(workspaceInvitationResponse);
            }

            return response.ToArray();
        }
    }
}
