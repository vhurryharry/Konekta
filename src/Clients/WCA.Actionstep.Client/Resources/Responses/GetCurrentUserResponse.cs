using Newtonsoft.Json;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class GetCurrentUserResponse : IActionstepResponse
    {
        [JsonProperty(PropertyName = "users")]
        public ActionstepUser User { get; set; }
    }
}
