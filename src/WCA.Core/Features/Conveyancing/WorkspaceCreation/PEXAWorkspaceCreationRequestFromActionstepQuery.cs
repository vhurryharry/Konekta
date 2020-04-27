using System.Collections.Generic;
using WCA.Domain.Conveyancing;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Domain.Pexa;
using WCA.PEXA.Client;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class PEXAWorkspaceCreationRequestFromActionstepQuery : IAuthenticatedQuery<PEXAWorkspaceCreationRequestWithActionstepResponse>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public int MatterId { get; set; }
        public string ActionstepOrg { get; set; }
    }

    public class PEXAWorkspaceCreationRequestWithActionstepResponse
    {
        public CreatePexaWorkspaceCommand CreatePexaWorkspaceCommand { get; set; }
        public bool PexaRoleSpecified { get; set; } = true;
        public ICollection<UserProfileRetrievalResponseTypeUserProfileWorkgroupListWorkgroup> WorkgroupList { get; set; }
        public PexaWorkspaceInfo ExistingPexaWorkspace { get; set; } = null;
        public ConveyancingMatter ActionstepData { get; set; }
    }
}
