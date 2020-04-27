using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using WCA.Actionstep.Client.Converters;

namespace WCA.Actionstep.Client.Resources.Responses
{
    /// <summary>
    /// For example, returned on a bad request.
    /// </summary>
    public class ErrorResponse
    {
        [JsonConverter(typeof(SingleOrArrayConverter<ErrorDetail>))]
        public List<ErrorDetail> Errors { get; } = new List<ErrorDetail>();
    }

    [JsonObject(Title = "Error")]
    public class ErrorDetail
    {
        public HttpStatusCode Status { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        [JsonProperty("x-field")]
        public string XField { get; set; }
    }
}
