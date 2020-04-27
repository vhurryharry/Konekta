using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep
{
    public class GetActionstepActions
    {
        public class GetActionstepActionsCommand : IAuthenticatedCommand<GetActionstepActionsResponse>
        {
            public WCAUser AuthenticatedUser { get; set; }
            public IEnumerable<int> MatterIds { get; set; }
            public string ActionstepOrgKey { get; set; }
        }

        public class Handler : IRequestHandler<GetActionstepActionsCommand, GetActionstepActionsResponse>
        {
            private readonly IActionstepService _actionstepService;

            public Handler(IActionstepService actionstepService)
            {
                _actionstepService = actionstepService;
            }

            public async Task<GetActionstepActionsResponse> Handle(GetActionstepActionsCommand command, CancellationToken cancellationToken)
            {
                if (command is null) throw new System.ArgumentNullException(nameof(command));

                var tokenSetQuery = new TokenSetQuery(command.AuthenticatedUser?.Id, command.ActionstepOrgKey);
                var apiParams = string.Join(",", command.MatterIds);

                return await _actionstepService.Handle<GetActionstepActionsResponse>(new GenericActionstepRequest(tokenSetQuery, $"rest/actions/{apiParams}", HttpMethod.Get));
            }
        }

        public class GetActionstepActionsResponse
        {
#pragma warning disable CA2227 // Collection properties should be read only: DTO
            public List<Action> Actions { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        }

        public class Action
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
