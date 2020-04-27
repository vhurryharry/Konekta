using FluentValidation;
using WCA.Domain.Models.Account;
using WCA.Domain.Pexa;
using WCA.PEXA.Client;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class GetPexaWorkgroupsQuery : IPexaAuthenticatedQuery<WorkgroupListRetrievalResponseType>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string AccessToken { get; set; }

        public class ValidatorCollection : AbstractValidator<GetPexaWorkgroupsQuery>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.AccessToken).NotNull();
            }
        }
    }
}
