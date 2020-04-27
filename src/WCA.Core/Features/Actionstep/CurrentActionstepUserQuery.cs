using WCA.Actionstep.Client.Resources;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.Requests
{
    public class CurrentActionstepUserQuery : IAuthenticatedQuery<ActionstepUser>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string OrgKey { get; set; }
    }
}
