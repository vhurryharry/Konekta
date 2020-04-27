using WCA.Actionstep.Client.Contracts;

namespace WCA.Actionstep.Client.DataCollections
{
    public class DataCollectionFieldValueMoney: DataCollectionFieldValueBase
    {
        public override ActionstepDataType DataType => ActionstepDataType.Money;

        /// <summary>
        /// TODO: Implement properties
        /// </summary>
        // public decimal Value { get; set; }
    }
}
