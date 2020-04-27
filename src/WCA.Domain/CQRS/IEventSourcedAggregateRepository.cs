namespace WCA.Domain.CQRS
{
    public interface IEventSourcedAggregateRepository<T>
        where T : IAggregateRoot, new()
    {
        void Save(IAggregateRoot aggregate);

        void Save(IAggregateRoot aggregate, int expectedVersion);

        T GetById(string id);
    }
}
