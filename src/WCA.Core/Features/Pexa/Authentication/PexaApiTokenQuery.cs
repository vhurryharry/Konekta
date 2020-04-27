using FluentValidation;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Pexa.Authentication
{
    public class PexaApiTokenQuery : IAuthenticatedQuery<PexaApiToken>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public bool BypassAndUpdateCache { get; set; } = false;

        public class Validator : AbstractValidator<PexaApiTokenQuery>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
            }
        }
    }
}
