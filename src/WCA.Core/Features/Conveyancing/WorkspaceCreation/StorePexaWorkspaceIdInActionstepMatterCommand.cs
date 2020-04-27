using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class StorePexaWorkspaceIdInActionstepMatterCommand : IAuthenticatedCommand<bool>
    {
        public string WorkspaceId { get; set; }

        public WCAUser AuthenticatedUser { get; set; }

        public string ActionstepOrg { get; set; }
        public int MatterId { get; set; }

        public class ValidatorCollection : AbstractValidator<StorePexaWorkspaceIdInActionstepMatterCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.WorkspaceId).NotEmpty();
                RuleFor(c => c.ActionstepOrg).NotEmpty();
                RuleFor(c => c.MatterId).GreaterThan(0);
            }
        }
    }
}
