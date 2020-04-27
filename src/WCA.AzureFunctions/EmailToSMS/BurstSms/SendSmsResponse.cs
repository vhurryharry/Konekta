using Newtonsoft.Json;

namespace WCA.AzureFunctions.EmailToSMS.BurstSms
{
    public class SendSmsResponse
    {
        [JsonProperty("message_id")]
        public int MessageId { get; set; }

        [JsonProperty("send_at")]
        public string SendAt { get; set; }

        [JsonProperty("recipients")]
        public int Recipients { get; set; }

        [JsonProperty("cost")]
        public float Cost { get; set; }

        [JsonProperty("sms")]
        public int Sms { get; set; }

        [JsonProperty("list")]
        public List List { get; set; }

        [JsonProperty("delivery_stats")]
        public DeliveryStats DeliveryStats { get; set; } = new DeliveryStats();

        [JsonProperty("error")]
        public SmsError Error { get; set; } = new SmsError();
    }

    public class List
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class DeliveryStats
    {
        [JsonProperty("delivered")]
        public int Delivered { get; set; }

        [JsonProperty("pending")]
        public int Pending { get; set; }

        [JsonProperty("bounced")]
        public int Bounced { get; set; }

        [JsonProperty("responses")]
        public int Responses { get; set; }

        [JsonProperty("optouts")]
        public int Optouts { get; set; }
    }

    public class SmsError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

}
