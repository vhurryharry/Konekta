using FluentValidation;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.FirstTitle.Connection
{
    public class FirstTitleCredentialsQuery: IAuthenticatedQuery<FirstTitleCredential>
    {
        public WCAUser AuthenticatedUser { get; set; }

        public FirstTitleCredentialsQuery()
        {
        }

        public class Validator : AbstractValidator<FirstTitleCredentialsQuery>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
            }
        }
    }
}
