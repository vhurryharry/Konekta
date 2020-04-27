using FluentValidation;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.FirstTitle.Connection
{
    public class CheckFirstTitleCredentialsQuery : IAuthenticatedQuery<bool>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public FirstTitleCredential FirstTitleCredentials { get; set; }

        public CheckFirstTitleCredentialsQuery()
        {
        }

        public class Validator : AbstractValidator<CheckFirstTitleCredentialsQuery>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.FirstTitleCredentials).NotNull();
            }
        }
    }
}
