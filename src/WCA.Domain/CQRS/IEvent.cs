using MediatR;
using NodaTime;
using System;

namespace WCA.Domain.CQRS
{
    /// <summary>
    /// Setters are used so that serialisatoin works correctly.
    /// Ideally we would remove the setters.
    /// </summary>
    public interface IEvent : INotification
    {
        /// <summary>
        /// Unique event Id
        /// </summary>
        Guid EventId { get; set; }

        /// <summary>
        /// The name of the event.
        /// </summary>
        string EventName { get; }

        /// <summary>
        /// Version / sequence of this event in the stream. Can be used to detect concurrency exceptions.
        /// </summary>
        int Version { get; set; }

        /// <summary>
        /// When the event was created in the system.
        /// Other timestamps should be stored separately as-needed. For example "OccurredAt" might be relevant for an aggregate that different to when the event was created in the system.
        /// </summary>
        Instant EventCreatedAt { get; set; }

        /// <summary>
        /// An Id to track related events. For example events that are part of a workflow managed by a process manager.
        /// </summary>
        Guid CorrelationId { get; set; }

        /// <summary>
        /// The Id of an event that triggered this event. Only relevant where this event was caused / triggered by another message/event/versioned command.
        /// </summary>
        Guid CausationId { get; set; }
    }
}
