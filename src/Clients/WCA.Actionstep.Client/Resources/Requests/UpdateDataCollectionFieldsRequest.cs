using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class UpdateDataCollectionFieldsRequest : IActionstepRequest
    {
        public List<DataCollectionField> DataCollectionFields { get; } = new List<DataCollectionField>();

        public TokenSetQuery TokenSetQuery { get; set; }

        public HttpMethod HttpMethod => HttpMethod.Put;

        public string RelativeResourcePath
        {
            get
            {
                string ids = string.Join(",", DataCollectionFields.Select(f => f.Id).ToList());
                return $"rest/datacollectionfields/{ids}";
            }
        }

        public object JsonPayload => new { datacollectionfields = DataCollectionFields };
    }
}
