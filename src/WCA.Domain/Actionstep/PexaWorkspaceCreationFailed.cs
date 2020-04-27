using NodaTime;
using System;
using WCA.Domain.CQRS;

namespace WCA.Domain.Actionstep
{
    public class PexaWorkspaceCreationFailed : EventBase
    {
        public string Message { get; }

        public PexaWorkspaceCreationFailed(Instant eventCreatedAt, string message, Guid correlatedWith, Guid causedBy)
            : base(eventCreatedAt, correlatedWith, causedBy)
        {
            Message = message;
        }
    }
}
