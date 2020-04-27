using FluentValidation;
using System;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Domain.Pexa;
using WCA.PEXA.Client;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class CreatePexaWorkspaceCommand : IPexaAuthenticatedCommand<CreatePexaWorkspaceResponse>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string AccessToken { get; set; }

        public WorkspaceCreationRequest PexaWorkspaceCreationRequest { get; set; }
        public string OrgKey { get; set; }
        public int MatterId { get; set; }

        public class ValidatorCollection : AbstractValidator<CreatePexaWorkspaceCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.AccessToken).NotNull();
                RuleFor(c => c.PexaWorkspaceCreationRequest).NotEmpty();
                RuleFor(c => c.OrgKey).NotEmpty();
                RuleFor(c => c.MatterId).GreaterThan(0);
            }
        }
    }

    public class CreatePexaWorkspaceResponse
    {
        public Uri WorkspaceUri { get; set; }
        public string WorkspaceId { get; set; }
        public Uri InvitationUri { get; set; }
        public bool WorkspaceExists { get; set; }

        public CreatePexaWorkspaceResponse(Uri workspaceUri, string workspaceId, Uri invitationUri, bool workspaceExists = false)
        {
            WorkspaceUri = workspaceUri;
            WorkspaceId = workspaceId;
            InvitationUri = invitationUri;
            WorkspaceExists = workspaceExists;
        }
    }
}
