using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Features.Conveyancing.SettlementCalculator;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Domain.Models.Settlement;

namespace WCA.Core.Features.Actionstep.Conveyancing.SettlementCalculator
{
    public class GetSettlementMatter
    {
        public class GetSettlementMatterQuery : IAuthenticatedQuery<SettlementMatter>
        {
            public WCAUser AuthenticatedUser { get; set; }
            public string OrgKey { get; set; }
            public int MatterId { get; set; }
        }

        public class ValidatorCollection : AbstractValidator<GetSettlementMatterQuery>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.OrgKey).NotNull();
                RuleFor(c => c.MatterId).NotNull();
            }
        }

        public class Handler : IRequestHandler<GetSettlementMatterQuery, SettlementMatter>
        {
            private readonly ValidatorCollection _validator;
            private readonly WCADbContext _wCADbContext;
            private readonly IMediator _mediator;

            public Handler(
                ValidatorCollection validator,
                WCADbContext wCADbContext,
                IMediator mediator)
            {
                _validator = validator;
                _wCADbContext = wCADbContext;
                _mediator = mediator;
            }

            public async Task<SettlementMatter> Handle(GetSettlementMatterQuery message, CancellationToken token)
            {
                if (message is null) throw new System.ArgumentNullException(nameof(message));

                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Invalid input.", result.Errors);
                }

                SettlementMatter settlementMatter;

                settlementMatter = _wCADbContext.SettlementMatters
                    .Where(s => (s.ActionstepOrgKey == message.OrgKey && s.ActionstepMatterId == message.MatterId))
                    .OrderBy(s => s.Version)
                    .LastOrDefault();

                if (settlementMatter == null)
                {
                    settlementMatter = new SettlementMatter(message.OrgKey, message.MatterId);
                }

                settlementMatter.ActionstepData = await _mediator.Send(new SettlementCalculatorMatterQuery
                {
                    AuthenticatedUser = message.AuthenticatedUser,
                    MatterId = message.MatterId,
                    OrgKey = message.OrgKey
                });

                return settlementMatter;
            }
        }
    }
}
