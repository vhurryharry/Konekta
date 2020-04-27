using FluentValidation;
using WCA.Domain.Models.Account;
using WCA.Domain.Pexa;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Conveyancing.WorkspaceInvitation
{
    public class SearchSubscriberQuery : IPexaAuthenticatedQuery<SubscriberSearchResponseType>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string AccessToken { get; set; }

        public SubscriberInformation SubscriberInformation { get; set; }

        public class ValidatorCollection : AbstractValidator<SearchSubscriberQuery>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.AccessToken).NotNull();
                RuleFor(c => c.SubscriberInformation).NotNull();
            }
        }
    }
}
