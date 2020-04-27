using System;
using System.Linq;

namespace WCA.Domain.CQRS
{
    public class AggregateRepository<T> : IEventSourcedAggregateRepository<T>
        where T : IAggregateRoot, new()
    {
        private readonly IEventStore _storage;

        public AggregateRepository(IEventStore storage)
        {
            _storage = storage;
        }

        public void Save(IAggregateRoot aggregate)
        {
            if (aggregate is null)
            {
                throw new ArgumentNullException(nameof(aggregate));
            }

            _storage.SaveEvents(
                aggregate.Id,
                aggregate.GetUncommittedChanges().ToArray());
        }

        public void Save(IAggregateRoot aggregate, int expectedVersion)
        {
            if (aggregate is null)
            {
                throw new ArgumentNullException(nameof(aggregate));
            }

            _storage.SaveEvents(
                aggregate.Id,
                aggregate.GetUncommittedChanges().ToArray(),
                expectedVersion);
        }

        public T GetById(string id)
        {
            var aggregate = new T();
            var allEvents = _storage.GetEventsForAggregate(id);
            aggregate.LoadFromHistory(allEvents);
            return aggregate;
        }
    }
}
