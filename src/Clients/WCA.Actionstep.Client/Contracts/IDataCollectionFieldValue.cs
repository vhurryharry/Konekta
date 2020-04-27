namespace WCA.Actionstep.Client.Contracts
{
    public interface IDataCollectionFieldValue
    {
        ActionstepDataType DataType { get; }
        string StringValue { get; set; }

        /// <summary>
        /// Whether the value has been modified since being retrieved.
        ///
        /// Used to determine if data needs to be sent back to the repository (Actionstep API).
        /// </summary>
        bool IsDirty { get; }
    }
}
