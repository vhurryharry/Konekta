using Newtonsoft.Json;
using NodaTime;
using System;
using System.Collections.Generic;
using WCA.GlobalX.Client.Serialisation;

namespace WCA.GlobalX.Client.Documents
{
    public class Document
    {
        /// <summary>
        /// When retrieving a single document from the API, this is populated with version information
        /// for each version of the document.
        /// </summary>
        [JsonProperty("documentVersions", NullValueHandling = NullValueHandling.Ignore)]
        public List<DocumentVersion> DocumentVersions { get; } = new List<DocumentVersion>();

        /// <summary>The document id</summary>
        [JsonProperty("documentId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? DocumentId { get; set; }

        /// <summary>The document store id. This is analogous to the documentVersionId and is required to retrieve a specific version of a document via the /versions API call</summary>
        [JsonProperty("documentStoreId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? DocumentStoreId { get; set; }

        /// <summary>The title of the document</summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>The matter reference that the document belongs to</summary>
        [JsonProperty("matterReference", NullValueHandling = NullValueHandling.Ignore)]
        public string MatterReference { get; set; }

        /// <summary>The status of the document</summary>
        [JsonProperty("statusDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusDescription { get; set; }

        /// <summary>The date and time when the document was last updated</summary>
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(OffsetDateTimeConverter))]
        public OffsetDateTime? Timestamp { get; set; }

        /// <summary>The date when the order was made</summary>
        [JsonProperty("orderDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(OffsetDateTimeConverter))]
        public OffsetDateTime? OrderDate { get; set; }

        /// <summary>The user who initiated the document creation</summary>
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        /// <summary>The order number that the document belongs to</summary>
        [JsonProperty("orderId", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderId { get; set; }

        /// <summary>The criteria used to search for the document</summary>
        [JsonProperty("criteria", NullValueHandling = NullValueHandling.Ignore)]
        public string Criteria { get; set; }

        /// <summary>The offering type that the document belongs to</summary>
        [JsonProperty("orderType", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderType { get; set; }

        /// <summary>The item number used to bill the purchase of the document</summary>
        [JsonProperty("itemNumber", NullValueHandling = NullValueHandling.Ignore)]
        public int? ItemNumber { get; set; }
    }
}
