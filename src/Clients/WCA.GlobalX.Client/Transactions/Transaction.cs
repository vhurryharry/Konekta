using Newtonsoft.Json;
using System;

namespace WCA.GlobalX.Client.Transactions
{
    public class Transaction
    {
        [JsonProperty("TransactionId")]
        public int TransactionId { get; set; }

        [JsonProperty("TransactionDateTime")]
        public DateTimeOffset TransactionDateTime { get; set; }

        [JsonProperty("Matter")]
        public string Matter { get; set; }

        [JsonProperty("SearchReference")]
        public string SearchReference { get; set; }

        [JsonProperty("WholesalePrice")]
        public decimal WholesalePrice { get; set; }

        [JsonProperty("WholesaleGst")]
        public decimal WholesaleGst { get; set; }

        [JsonProperty("RetailPrice")]
        public decimal RetailPrice { get; set; }

        [JsonProperty("RetailGst")]
        public decimal RetailGst { get; set; }

        [JsonProperty("OrderId")]
        public string OrderId { get; set; }

        [JsonProperty("CreditFor")]
        public int CreditFor { get; set; }

        [JsonProperty("ItemNumber")]
        public int ItemNumber { get; set; }

        [JsonProperty("MatterBasedInvoiced")]
        public bool MatterBasedInvoiced { get; set; }

        [JsonProperty("User")]
        public User User { get; set; } = new User();

        [JsonProperty("Product")]
        public Product Product { get; set; } = new Product();
    }
}
