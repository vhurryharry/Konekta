using System.Collections.Generic;

namespace WCA.Domain.CQRS
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        public abstract string Id { get; }
        public int Version { get; internal set; }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            if (history != null)
            {
                foreach (var @event in history) ApplyChange(@event, false);
            }
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(IEvent @event, bool isNew)
        {
            ((dynamic)this).Apply((dynamic)@event);
            if (isNew) _changes.Add(@event);
            Version++;
        }

        public void Apply(IEvent e)
        {
            // no-op
        }
    }
}
