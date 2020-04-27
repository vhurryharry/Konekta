using WCA.Actionstep.Client.Contracts;

namespace WCA.Actionstep.Client.DataCollections
{
    public class DataCollectionFieldValueNumber : DataCollectionFieldValueBase
    {
        public override ActionstepDataType DataType => ActionstepDataType.Number;

        /// <summary>
        /// TODO: Implement properties
        /// </summary>
        // public decimal Value { get; set; }
    }
}
