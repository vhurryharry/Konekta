using FluentValidation;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Domain.Pexa;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class GetPexaWorkspaceForActionstepMatterQuery : IAuthenticatedQuery<PexaWorkspaceInfo>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string ActionstepOrg { get; set; }
        public int MatterId { get; set; }

        public class ValidatorCollection : AbstractValidator<GetPexaWorkspaceForActionstepMatterQuery>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.ActionstepOrg).NotNull();
                RuleFor(c => c.MatterId).NotNull();
            }
        }
    }
}
