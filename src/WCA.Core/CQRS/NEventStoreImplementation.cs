using NEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using WCA.Domain.CQRS;

namespace WCA.Core.CQRS
{
    public class NEventStoreImplementation : IEventStore, IDisposable
    {
        private IStoreEvents _store;

        /// <summary>
        /// Please do not dispose of <c>store</c> This implementation will take care of its disposal.
        /// </summary>
        /// <param name="store"></param>
        public NEventStoreImplementation(IStoreEvents store)
        {
            _store = store;
        }

        public List<IEvent> GetEventsForAggregate(string aggregateId)
        {
            using (var stream = _store.OpenStream(aggregateId))
            {
                var i = 0;

                return stream.CommittedEvents
                    .Select(ce => {
                        var thisEvent = ce.Body as IEvent;
                        thisEvent.Version = i++;
                        return thisEvent;
                    })
                    .ToList();
            }
        }

        public void SaveEvents(string aggregateId, IEvent[] events)
        {
            using (var stream = _store.OpenStream(aggregateId))
            {
                foreach (var @event in events)
                {
                    // Ensure all versions are stored as 0.
                    // Correct event versions will be applied on read.
                    @event.Version = 0;
                    stream.Add(new EventMessage() { Body = @event });
                }

                stream.CommitChanges(Guid.NewGuid());
            }
        }


        public void SaveEvents(string aggregateId, IEvent[] events, int expectedVersion)
        {
            using (var stream = _store.OpenStream(aggregateId))
            {
                // Not sure about this
                if (stream.StreamRevision > expectedVersion)
                    throw new ConcurrencyException("StreamRevision was greater than expectedVersion");

                int i = expectedVersion;

                foreach (var @event in events)
                {
                    // Ensure all versions are stored as 0.
                    // Correct event versions will be applied on read.
                    @event.Version = 0;
                    stream.Add(new EventMessage() { Body = @event });
                }

                stream.CommitChanges(Guid.NewGuid());
            }
        }

        public void DangerPurgeAllEvents()
        {
            _store.Advanced.Purge();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _store.Dispose();
                    _store = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
