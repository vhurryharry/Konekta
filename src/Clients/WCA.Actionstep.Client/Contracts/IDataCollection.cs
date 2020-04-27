namespace WCA.Actionstep.Client.Contracts
{
    public interface IDataCollection
    {
        /// <summary>
        /// The name of the Data Collection. This is a ReadOnly constant used for Merge Field use.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// If TRUE, must display custom HTML blocks defined by Data Collection Fields for this Data Collection.
        /// </summary>
        bool AlwaysShowDescriptions { get; }

        /// <summary>
        /// The ActionType that the DataCollection belongs to.
        /// </summary>
        string ActionType { get; }

        /// <summary>
        /// The label to be used for display purposes.
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// A general description of the Data Collection.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Indicates if the Data Collection consists of a single record or multiple records.
        /// </summary>
        bool MultipleRecords { get; }

        /// <summary>
        /// The position of the Data Collection when part of a list of Data Collections.
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// Whether the values of any properties or children have been modified since being retrieved.
        ///
        /// Used to determine if data needs to be sent back to the repository (Actionstep API).
        /// </summary>
        bool IsDirty { get; }
    }
}