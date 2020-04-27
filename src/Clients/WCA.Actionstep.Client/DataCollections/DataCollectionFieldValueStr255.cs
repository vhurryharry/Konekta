using WCA.Actionstep.Client.Contracts;

namespace WCA.Actionstep.Client.DataCollections
{
    public class DataCollectionFieldValueStr255 : DataCollectionFieldValueBase
    {
        public override ActionstepDataType DataType => ActionstepDataType.Str255;

        /// <summary>
        /// TODO: Implement properties. E.g.. should enforse char limit (255).
        /// </summary>
        // public string Value { get; set; }
    }
}
