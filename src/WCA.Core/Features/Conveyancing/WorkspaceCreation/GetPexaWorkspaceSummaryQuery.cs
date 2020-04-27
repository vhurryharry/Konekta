using FluentValidation;
using WCA.Domain.Models.Account;
using WCA.Domain.Pexa;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class GetPexaWorkspaceSummaryQuery : IPexaAuthenticatedQuery<WorkspaceSummaryResponseType>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string AccessToken { get; set; }

        public RetrieveWorkspaceSummaryParameters RetrieveWorkspaceSummaryParameters { get; set; }

        public class ValidatorCollection : AbstractValidator<GetPexaWorkspaceSummaryQuery>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.AccessToken).NotNull();
                RuleFor(c => c.RetrieveWorkspaceSummaryParameters).NotEmpty();
            }
        }
    }
}
