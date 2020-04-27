using FluentValidation;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.IntegrationTests
{
    public class RunIntegrationTestsCommand : IAuthenticatedCommand<RunIntegrationTestsResponse>
    {
        public string OrgKey { get; }
        public int MatterId { get; }
        public WCAUser AuthenticatedUser { get; set; }

        public RunIntegrationTestsCommand(
            string orgKey,
            int matterId,
            WCAUser authenticatedUser)
        {
            OrgKey = orgKey;
            MatterId = matterId;
            AuthenticatedUser = authenticatedUser;
        }

        public class Validator : AbstractValidator<RunIntegrationTestsCommand>
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
