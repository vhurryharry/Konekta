using FluentValidation;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Pexa.Authentication
{
    public class StorePexaApiTokenCommand : IAuthenticatedCommand
    {
        public WCAUser AuthenticatedUser { get; set; }
        public PexaApiToken PexaApiToken { get; set; }

        public class Validator : AbstractValidator<StorePexaApiTokenCommand>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.PexaApiToken).NotEmpty();
            }
        }
    }
}
