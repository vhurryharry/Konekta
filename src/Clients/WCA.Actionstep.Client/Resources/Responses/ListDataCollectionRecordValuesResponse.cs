using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using WCA.Actionstep.Client.Converters;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class ListDataCollectionRecordValuesResponse : IActionstepResponse
    {
        [JsonConverter(typeof(SingleOrArrayConverter<DataCollectionRecordValue>))]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<DataCollectionRecordValue> DataCollectionRecordValues { get; set; } = new List<DataCollectionRecordValue>();

        public Linked Linked { get; set; } = new Linked();

        public string this[string collectionName, string propName]
        {
            get
            {
                var dataCollection = Linked.DataCollections.SingleOrDefault(c => c.Name == collectionName);
                if (dataCollection == null)
                {
                    return string.Empty;
                }

                var field = Linked.DataCollectionFields
                    .Where(f => f.Name == propName && f.Links.DataCollection == dataCollection.Id.ToString(CultureInfo.InvariantCulture))
                    .SingleOrDefault();
                if (field == null)
                {
                    return string.Empty;
                }

                var record = DataCollectionRecordValues.Where(r => r.Links.DataCollectionField == field.Id).SingleOrDefault();
                if (record == null)
                {
                    return string.Empty;
                }

                return record.StringValue;
            }
            set
            {
                var dataCollection = Linked.DataCollections.SingleOrDefault(c => c.Name == collectionName);
                if (dataCollection == null)
                {
                    return;
                }

                var field = Linked.DataCollectionFields
                    .Where(f => f.Name == propName && f.Links.DataCollection == dataCollection.Id.ToString(CultureInfo.InvariantCulture))
                    .SingleOrDefault();
                if (field == null)
                {
                    return;
                }

                var record = DataCollectionRecordValues.Where(r => r.Links.DataCollectionField == field.Id).SingleOrDefault();
                if (record == null)
                {
                    return;
                }

                record.StringValue = value;
            }
        }
    }
        
    public class Linked
    {
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<DataCollectionField> DataCollectionFields { get; set; } = new List<DataCollectionField>();

        [JsonConverter(typeof(SingleOrArrayConverter<DataCollection>))]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<DataCollection> DataCollections { get; set; } = new List<DataCollection>();
    }

    public class Link
    {
        public int DataCollection { get; set; }
        public string Tag { get; set; }
    }
}
