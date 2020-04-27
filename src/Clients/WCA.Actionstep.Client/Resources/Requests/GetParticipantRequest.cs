using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class GetParticipantRequest : IActionstepRequest
    {
        public int Id { get; set; }
        public string RelativeResourcePath => $"rest/participants/{Id}";
        public HttpMethod HttpMethod => HttpMethod.Get;

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => null;
    }
}
