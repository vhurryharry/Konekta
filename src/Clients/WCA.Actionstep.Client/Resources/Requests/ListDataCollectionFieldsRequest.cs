using System.Collections.Generic;
using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class ListDataCollectionFieldsRequest : IActionstepRequest
    {
        public List<string> DataCollectionNames { get; } = new List<string>();
        public List<int> ActionTypes { get; } = new List<int>();

        public HttpMethod HttpMethod => HttpMethod.Get;
        public string RelativeResourcePath
        {
            get
            {
                var dataCollectionNames = string.Join(",", DataCollectionNames);
                var actionTypes = string.Join(",", ActionTypes);

                var requestUri = $"rest/datacollectionfields?dataCollection[name_in]={dataCollectionNames}"
                    + $"&include=dataCollectionField,dataCollection";

                if (!string.IsNullOrEmpty(actionTypes))
                    requestUri += $"&dataCollection[actionType_in]={actionTypes}";

                if (!string.IsNullOrEmpty(Filter))
                    requestUri += $"&{Filter}";

                return requestUri;
            }
        }

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => null;

        public string Filter { get; set; } = null;
    }
}
