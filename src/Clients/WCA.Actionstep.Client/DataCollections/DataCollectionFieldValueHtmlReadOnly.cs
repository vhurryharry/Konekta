using WCA.Actionstep.Client.Contracts;

namespace WCA.Actionstep.Client.DataCollections
{
    public class DataCollectionFieldValueHtmlReadOnly : DataCollectionFieldValueBase
    {
        public override ActionstepDataType DataType => ActionstepDataType.HtmlReadOnly;

        /// <summary>
        /// TODO - figure out suitable type or properties/types.
        /// </summary>
    }
}
