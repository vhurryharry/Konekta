using Newtonsoft.Json;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class GetParticipantResponse : IActionstepResponse
    {
        [JsonProperty("participants")]
        public Participant Participant { get; set; }
    }
}
