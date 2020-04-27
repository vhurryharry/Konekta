using FluentValidation;
using WCA.Core.Features.Actionstep.Responses;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Conveyancing.StampDutyCalculator
{
    public class StampDutyCalculatorInfoQuery : IAuthenticatedQuery<StampDutyCalculatorInfo>
    {
        public string OrgKey { get; }
        public int MatterId { get; }
        public WCAUser AuthenticatedUser { get; set; }

        public StampDutyCalculatorInfoQuery(
            string orgKey,
            int matterId,
            WCAUser authenticatedUser)
        {
            OrgKey = orgKey;
            MatterId = matterId;
            AuthenticatedUser = authenticatedUser;
        }

        public class Validator : AbstractValidator<StampDutyCalculatorInfoQuery>
        {
            public Validator()
            {
                RuleFor(c => c.OrgKey).NotNull().MinimumLength(1);
                RuleFor(c => c.MatterId).GreaterThan(0);
                RuleFor(c => c.AuthenticatedUser).NotNull();
            }
        }

    }
}
