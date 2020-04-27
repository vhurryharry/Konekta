using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Domain.Models.Settlement;

namespace WCA.Core.Features.Actionstep.Conveyancing.SettlementCalculator
{
    public class SaveSettlementMatter
    {
        public class SaveSettlementMatterCommand : IAuthenticatedCommand<SettlementMatter>
        {
            public WCAUser AuthenticatedUser { get; set; }
            public string OrgKey { get; set; }
            public int MatterId { get; set; }
            public SettlementMatter Matter { get; set; }
        }

        public class Validator : AbstractValidator<SaveSettlementMatterCommand>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.OrgKey).NotNull();
                RuleFor(c => c.MatterId).NotNull();
            }
        }

        public class Handler : IRequestHandler<SaveSettlementMatterCommand, SettlementMatter>
        {
            private readonly Validator _validator;
            private readonly WCADbContext _wCADbContext;

            public Handler(
                Validator validator,
                WCADbContext wCADbContext)
            {
                _validator = validator;
                _wCADbContext = wCADbContext;
            }

            public async Task<SettlementMatter> Handle(SaveSettlementMatterCommand message, CancellationToken token)
            {
                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Invalid input.", result.Errors);
                }

                SettlementMatter orgSettlementMatter;

                orgSettlementMatter = _wCADbContext.SettlementMatters
                    .Where(s => (s.ActionstepOrgKey == message.OrgKey && s.ActionstepMatterId == message.MatterId))
                    .OrderBy(s => s.Version)
                    .LastOrDefault();

                message.Matter.CreatedBy = message.AuthenticatedUser;
                message.Matter.UpdatedBy = message.AuthenticatedUser;
                message.Matter.DateCreatedUtc = DateTime.UtcNow;
                message.Matter.LastUpdatedUtc = DateTime.UtcNow;

                if (orgSettlementMatter != null)
                {
                    message.Matter.Version = orgSettlementMatter.Version + 1;
                    message.Matter.CreatedBy = orgSettlementMatter.CreatedBy;
                }

                _wCADbContext.SettlementMatters.Add(message.Matter);
                _wCADbContext.SaveChanges();

                return message.Matter;
            }
        }
    }
}
