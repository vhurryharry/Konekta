using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WCA.GlobalX.Client.Documents
{
    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "This is deserialised from JSON, hence no direct instantiation.")]
    internal class DocumentsResponse
    {
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("documentsRequest")]
        public DocumentsRequest DocumentsRequest { get; set; }

        [JsonProperty("items")]
        public List<Document> Items { get; } = new List<Document>();
    }
}
