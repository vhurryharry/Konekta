namespace WCA.Actionstep.Client.Contracts
{
    public interface IDataCollections
    {
        IDataCollection this[string dataCollectionName] { get; }
    }
}