using FluentValidation;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Domain.Pexa;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class ValidateLandTitleReferenceQuery : IPexaAuthenticatedQuery<LandTitleReferenceVerificationResponseType>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string AccessToken { get; set; }

        public LandTitleReferenceAndJurisdiction LandTitleReferenceAndJurisdiction { get; set; }

        public class ValidatorCollection : AbstractValidator<ValidateLandTitleReferenceQuery>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.AccessToken).NotNull();
                RuleFor(c => c.LandTitleReferenceAndJurisdiction).NotEmpty();
            }
        }
    }
}
