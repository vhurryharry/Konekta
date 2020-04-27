
using FluentValidation;
using WCA.Domain.Models.Account;
using WCA.Domain.Pexa;
using WCA.PEXA.Client;

namespace WCA.Core.Features.Conveyancing.WorkspaceInvitation
{
    public class GetPexaUserProfileQuery : IPexaAuthenticatedQuery<UserProfileRetrievalResponseType>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string AccessToken { get; set; }

        public class ValidatorCollection : AbstractValidator<GetPexaUserProfileQuery>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.AccessToken).NotNull();
            }
        }
    }
}
