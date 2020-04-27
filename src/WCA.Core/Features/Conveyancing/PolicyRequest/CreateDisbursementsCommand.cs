using WCA.Actionstep.Client.Resources.Responses;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Conveyancing.PolicyRequest
{
    public class CreateDisbursementsCommand : IAuthenticatedCommand<ListDisbursementsResponse>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string PolicyNumber { get; set; }
        public FirstTitlePrice FirstTitlePrice { get; set; }
        public int MatterId { get; set; }
        public string ActionstepOrg { get; set; }
    }
}
