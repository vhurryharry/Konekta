using NodaTime;
using System;

namespace WCA.Domain.CQRS
{
    public abstract class EventBase : IEvent
    {
        public EventBase(Instant eventCreatedAt)
        {
            EventId = Guid.NewGuid();
            EventCreatedAt = eventCreatedAt;
            CorrelationId = Guid.Empty;
            CausationId = Guid.Empty;
        }

        public string EventName
        {
            get
            {
                return GetType().Name;
            }
        }
        public EventBase(Instant eventCreatedAt, Guid correlatedWith, Guid causedBy)
            : this(eventCreatedAt)
        {
            CorrelationId = correlatedWith;
            CausationId = causedBy;
        }

        public Guid EventId { get; set; }
        public Instant EventCreatedAt { get; set; }
        public int Version { get; set; }

        public Guid CorrelationId { get; set; }
        public Guid CausationId { get; set; }
    }
}
