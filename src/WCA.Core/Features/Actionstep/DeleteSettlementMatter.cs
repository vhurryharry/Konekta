using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Extensions;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Domain.Models.Settlement;

namespace WCA.Core.Features.Actionstep
{
    public class DeleteSettlementMatter
    {
        public class DeleteSettlementMatterCommand : IAuthenticatedCommand<SettlementMatter>
        {
            public WCAUser AuthenticatedUser { get; set; }
            public string OrgKey { get; set; }
            public int MatterId { get; set; }
        }

        public class ValidatorCollection : AbstractValidator<DeleteSettlementMatterCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.OrgKey).NotNull();
                RuleFor(c => c.MatterId).NotNull();
            }
        }

        public class Handler : IRequestHandler<DeleteSettlementMatterCommand, SettlementMatter>
        {
            private readonly ValidatorCollection _validator;
            private readonly WCADbContext _wCADbContext;

            public Handler(
                ValidatorCollection validator,
                WCADbContext wCADbContext)
            {
                _validator = validator;
                _wCADbContext = wCADbContext;
            }

            public async Task<SettlementMatter> Handle(DeleteSettlementMatterCommand message, CancellationToken token)
            {
                if (message is null)
                {
                    throw new ArgumentNullException(nameof(message));
                }

                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Invalid input.", result.Errors);
                }

                if (!_wCADbContext.ActionstepCredentials.UserHasValidCredentialsForOrg(message.AuthenticatedUser, message.OrgKey))
                {
                    throw new UnauthorizedAccessException("User does not have access to the org for this matter.");
                }

                var orgSettlementMatters = _wCADbContext.SettlementMatters
                    .Where(s => (s.ActionstepOrgKey == message.OrgKey && s.ActionstepMatterId == message.MatterId))
                    .OrderBy(s => s.Version);

                _wCADbContext.SettlementMatters.RemoveRange(orgSettlementMatters);
                _wCADbContext.SaveChanges();

                return await Task.FromResult(orgSettlementMatters.LastOrDefault());
            }
        }
    }
}
