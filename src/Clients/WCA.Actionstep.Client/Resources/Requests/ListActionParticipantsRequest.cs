using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class ListActionParticipantsRequest : IActionstepRequest
    {
        public int ActionstepId { get; set; }

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string RelativeResourcePath =>
            $"rest/actionparticipants?action={ActionstepId}&include=participant,participantType";

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => null;
    }
}
