using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Conveyancing.SettlementCalculator
{
    public class SettlementCalculatorUrlQuery : IAuthenticatedQuery<string>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string OrgKey { get; set; }
        public int MatterId { get; set; }
    }
}
