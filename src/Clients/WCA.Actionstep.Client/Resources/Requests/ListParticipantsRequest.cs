using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class ListParticipantsRequest : IActionstepRequest
    {
        public HttpMethod HttpMethod => HttpMethod.Get;
        public string RelativeResourcePath => "rest/participants";

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => null;
    }
}
