using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class DeleteDataCollectionFieldRequest : IActionstepRequest
    {
        public List<string> IdsToDelete { get; } = new List<string>();

        public string RelativeResourcePath =>
            $"rest/datacollectionfields/{string.Join(",", IdsToDelete)}";

        public TokenSetQuery TokenSetQuery { get; set; }

        public HttpMethod HttpMethod => HttpMethod.Delete;

        public object JsonPayload => null;
    }
}
