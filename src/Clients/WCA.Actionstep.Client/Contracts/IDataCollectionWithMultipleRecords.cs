namespace WCA.Actionstep.Client.Contracts
{
    public interface IDataCollectionWithMultipleRecords : IDataCollection
    {
        IDataCollectionFieldValue this[int index] { get; }
    }
}
