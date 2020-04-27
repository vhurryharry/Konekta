using FluentValidation;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Conveyancing.PolicyRequest
{
    public class SavePolicyPDFToActionstepCommand : IAuthenticatedCommand<FTAttachment>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string ActionstepOrg { get; set; }
        public int MatterId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public class ValidatorCollection : AbstractValidator<SavePolicyPDFToActionstepCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.ActionstepOrg).NotNull().MinimumLength(1);
                RuleFor(c => c.MatterId).GreaterThan(0);
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.FilePath).NotNull();
                RuleFor(c => c.FileName).NotNull();
            }
        }
    }
}
