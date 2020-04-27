using System.Collections.Generic;

namespace WCA.Domain.CQRS
{
    public interface IAggregateRoot
    {
        string Id { get; }
        int Version { get; }

        IEnumerable<IEvent> GetUncommittedChanges();

        void MarkChangesAsCommitted();

        void LoadFromHistory(IEnumerable<IEvent> history);

        void Apply(IEvent e);
    }
}
