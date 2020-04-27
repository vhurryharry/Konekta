
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Domain.Models.Settlement;

namespace WCA.Core.Features.Conveyancing.SettlementCalculator
{
    public class SettlementCalculatorMatterQuery : IAuthenticatedQuery<ActionstepMatter>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string OrgKey { get; set; }
        public int MatterId { get; set; }
    }
}
