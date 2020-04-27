using System;
using WCA.Domain.Abstractions;

namespace WCA.Domain.Actionstep
{
    public class PexaWorkspace: EntityBase, IEntityWithId
    {
        public int Id { get; set; }
        public string WorkspaceId { get; set; }
        public Uri WorkspaceUri { get; set; }
        public string ActionstepOrg { get; set; }
        public int MatterId { get; set; }
    }
}
