using NodaTime;
using System;
using WCA.Domain.CQRS;

namespace WCA.Domain.Actionstep
{
    public class PexaWorkspaceCreated : EventBase
    {
        public string WorkspaceId { get; }

        public PexaWorkspaceCreated(Instant eventCreatedAt, string workspaceId, Guid correlatedWith, Guid causedBy)
            : base(eventCreatedAt, correlatedWith, causedBy)
        {
            WorkspaceId = workspaceId;
        }
    }
}
