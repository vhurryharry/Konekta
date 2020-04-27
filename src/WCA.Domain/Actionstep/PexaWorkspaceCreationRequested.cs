using NodaTime;
using System;
using WCA.Domain.CQRS;

namespace WCA.Domain.Actionstep
{
    public class PexaWorkspaceCreationRequested : EventBase
    {
        public string RequestedByUserId { get; }

        public PexaWorkspaceCreationRequested(Instant eventCreatedAt, string requestedByUserId)
            :base(eventCreatedAt, Guid.NewGuid(), Guid.Empty)
        {
            RequestedByUserId = requestedByUserId;
        }
    }
}
