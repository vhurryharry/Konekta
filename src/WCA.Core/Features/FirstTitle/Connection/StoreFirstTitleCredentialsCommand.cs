using FluentValidation;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.FirstTitle.Connection
{
    public class StoreFirstTitleCredentialsCommand : IAuthenticatedCommand
    {
        public WCAUser AuthenticatedUser { get; set; }
        public FirstTitleCredential FirstTitleCredentials { get; set; }

        public class Validator : AbstractValidator<StoreFirstTitleCredentialsCommand>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.FirstTitleCredentials).NotEmpty();
            }
        }
    }
}
