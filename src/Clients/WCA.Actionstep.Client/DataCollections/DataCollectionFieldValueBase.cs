using WCA.Actionstep.Client.Contracts;

namespace WCA.Actionstep.Client.DataCollections
{
    public abstract class DataCollectionFieldValueBase : IDataCollectionFieldValue
    {
        private string _stringValue;
        private bool _isDirty = false;

        public abstract ActionstepDataType DataType { get; }

        public virtual string StringValue
        {
            get => _stringValue;
            set
            {
                if (value != _stringValue)
                {
                    _isDirty = true;
                    _stringValue = value;
                }
            }
        }

        public bool IsDirty => _isDirty;
    }
}
