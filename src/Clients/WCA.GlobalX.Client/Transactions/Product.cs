using Newtonsoft.Json;

namespace WCA.GlobalX.Client.Transactions
{
    public class Product
    {
        [JsonProperty("ProductCode")] 
        public string ProductCode { get; set; }

        [JsonProperty("ProductDescription")] 
        public string ProductDescription { get; set; }

        [JsonProperty("ProductSubGroup")]
        public string ProductSubGroup { get; set; }
    }
}
