using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class UpdateDataCollectionRecordValuesRequest : IActionstepRequest
    {
        public List<DataCollectionRecordValue> DataCollectionRecordValues { get; } = new List<DataCollectionRecordValue>();

        public TokenSetQuery TokenSetQuery { get; set; }

        public HttpMethod HttpMethod => HttpMethod.Put;

        public string RelativeResourcePath
        {
            get
            {
                string ids = string.Join(",", DataCollectionRecordValues.Select(f => f.Id).ToList());
                return $"rest/datacollectionrecordvalues/{ids}";
            }
        }

        public object JsonPayload =>
            new
            {
                datacollectionrecordvalues = DataCollectionRecordValues
            };
    };
}
