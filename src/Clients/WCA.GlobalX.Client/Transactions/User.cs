using Newtonsoft.Json;

namespace WCA.GlobalX.Client.Transactions
{
    public class User
    {
        [JsonProperty("UserId")] 
        public string UserId { get; set; }

        [JsonProperty("CustomerRef")]
        public string CustomerRef { get; set; }
    }
}
