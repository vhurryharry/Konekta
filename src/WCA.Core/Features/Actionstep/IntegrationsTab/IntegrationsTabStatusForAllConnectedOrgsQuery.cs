using System.Collections.Generic;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.IntegrationsTab
{
    public class IntegrationsTabStatusForAllConnectedOrgsQuery : IAuthenticatedQuery<List<IntegrationsTabStatusForOrgAndActionType>>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string OrgKey { get; set; }
    }
}
