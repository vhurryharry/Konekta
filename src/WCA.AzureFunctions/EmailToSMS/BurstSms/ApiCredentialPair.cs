namespace WCA.AzureFunctions.EmailToSMS.BurstSms
{
    public class ApiCredentialPair
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public ApiCredentialPair(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        public bool ContainsValues ()
        {
            return !string.IsNullOrEmpty(ApiKey) && !string.IsNullOrEmpty(ApiSecret);
        }
    }
}
