using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Core.Features.Actionstep.Queries;
using WCA.Core.Features.Actionstep.Responses;

namespace WCA.Core.Features.Actionstep
{
    public class GetMatterInfoHandler : IRequestHandler<ActionstepMatterInfoQuery, ActionstepMatterInfo>
    {
        private readonly ActionstepMatterInfoQuery.Validator _validator;
        private readonly IActionstepService _actionstepService;

        public GetMatterInfoHandler(
            ActionstepMatterInfoQuery.Validator validator,
            IActionstepService actionstepService)
        {
            _validator = validator;
            _actionstepService = actionstepService;
        }

        public async Task<ActionstepMatterInfo> Handle(ActionstepMatterInfoQuery message, CancellationToken token)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));

            ValidationResult result = _validator.Validate(message);
            if (!result.IsValid)
            {
                throw new ValidationException("Invalid input.", result.Errors);
            }

            var actionResponse = await _actionstepService.Handle<GetActionResponse>(new GetActionRequest()
            {
                TokenSetQuery = new TokenSetQuery(message.AuthenticatedUser?.Id, message.OrgKey),
                ActionId = message.MatterId
            });

            try
            {
                return new ActionstepMatterInfo(
                    message.OrgKey,
                    actionResponse.Action.Name,
                    message.MatterId,
                    actionResponse.OrgName);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                throw new MatterNotFoundException(message.OrgKey,
                                                  message.MatterId,
                                                  ex);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}