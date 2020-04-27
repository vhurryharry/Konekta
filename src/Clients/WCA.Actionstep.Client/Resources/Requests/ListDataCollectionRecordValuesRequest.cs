using System.Collections.Generic;
using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class ListDataCollectionRecordValuesRequest : IActionstepRequest
    {
        public int ActionstepId { get; set; }

        public List<string> DataCollectionRecordNames { get; } = new List<string>();

        public List<string> DataCollectionFieldNames { get; } = new List<string>();

        public string RelativeResourcePath
        {
            get
            {
                var dataCollectionRecordNames = string.Join(",", DataCollectionRecordNames);
                var dataCollectionFieldNames = string.Join(",", DataCollectionFieldNames);

                return $"rest/datacollectionrecordvalues?action={ActionstepId}"
                        + $"&dataCollectionRecord[dataCollection][name_in]={dataCollectionRecordNames}"
                        + $"&dataCollectionField[name_in]={dataCollectionFieldNames}"
                        + $"&include=dataCollectionField,dataCollection";
            }
        }

        public TokenSetQuery TokenSetQuery { get; set; }

        public HttpMethod HttpMethod => HttpMethod.Get;

        public object JsonPayload => null;
    }
}
