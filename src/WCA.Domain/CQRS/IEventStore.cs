using System.Collections.Generic;

namespace WCA.Domain.CQRS
{
    public interface IEventStore
    {
        void SaveEvents(string aggregateId, IEvent[] events);

        void SaveEvents(string aggregateId, IEvent[] events, int expectedVersion);

        List<IEvent> GetEventsForAggregate(string aggregateId);

        /// <summary>
        /// Designed for testing purposes.
        /// </summary>
        void DangerPurgeAllEvents();
    }
}
