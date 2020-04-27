using FluentValidation;
using System;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class StorePexaWorkspaceInfoIntoDBCommand : IAuthenticatedCommand<bool>
    {
        public string WorkspaceId { get; set; }
        public Uri WorkspaceUri { get; set; }

        public WCAUser AuthenticatedUser { get; set; }

        public string ActionstepOrg { get; set; }
        public int MatterId { get; set; }

        public class ValidatorCollection : AbstractValidator<StorePexaWorkspaceInfoIntoDBCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.WorkspaceId).NotEmpty();
                RuleFor(c => c.WorkspaceUri).NotEmpty();
                RuleFor(c => c.ActionstepOrg).NotEmpty();
                RuleFor(c => c.MatterId).GreaterThan(0);
            }
        }
    }
}
