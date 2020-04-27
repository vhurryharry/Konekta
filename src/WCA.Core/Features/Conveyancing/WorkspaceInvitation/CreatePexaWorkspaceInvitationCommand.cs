using FluentValidation;
using WCA.Domain.Models.Account;
using WCA.Domain.Pexa;
using WCA.PEXA.Client;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class CreatePexaWorkspaceInvitationCommand : IPexaAuthenticatedCommand<CreateWorkspaceInvitationResponseType[]>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string AccessToken { get; set; }

        public CreateWorkspaceInvitationRequestType[] PexaWorkspaceInvitationRequests { get; set; }

        public class ValidatorCollection : AbstractValidator<CreatePexaWorkspaceInvitationCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.AccessToken).NotNull();
                RuleFor(c => c.PexaWorkspaceInvitationRequests).NotEmpty();
            }
        }
    }
}
