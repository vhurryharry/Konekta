using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class CreateDataCollectionFieldRequest : IActionstepRequest
    {
        public List<DataCollectionField> DataCollectionFields { get; } = new List<DataCollectionField>();

        public string RelativeResourcePath
        {
            get
            {
                var ids = string.Join(",", DataCollectionFields.Select(f => f.Id));
                return $"rest/datacollectionfields/{ids}";
            }
        }

        public HttpMethod HttpMethod => HttpMethod.Post;

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => new { datacollectionfields = DataCollectionFields };
    }
}
