using WCA.Core.Features.Actionstep.Responses;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.Requests
{
    public class FixIntegrationsTabCommand : IAuthenticatedCommand
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string OrgKey { get; set; }
        public int ActionTypeId { get; set; }
    }
}
