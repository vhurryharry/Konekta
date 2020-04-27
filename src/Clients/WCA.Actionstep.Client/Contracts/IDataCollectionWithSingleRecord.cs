namespace WCA.Actionstep.Client.Contracts
{
    public interface IDataCollectionWithSingleRecord : IDataCollection
    {
        IDataCollectionFieldValue this[string fieldName] { get; }
    }
}
