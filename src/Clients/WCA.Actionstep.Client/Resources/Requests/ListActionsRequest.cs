using System.Collections.Generic;
using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class ListActionsRequest : IActionstepRequest
    {
        public List<int> ActionstepIds { get; } = new List<int>();

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string RelativeResourcePath =>
            $"rest/actions?id_in={string.Join(",", ActionstepIds)}";

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => null;
    }
}
