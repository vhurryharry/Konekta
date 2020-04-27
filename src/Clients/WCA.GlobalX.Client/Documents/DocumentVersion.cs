using Newtonsoft.Json;
using NodaTime;
using System;
using WCA.GlobalX.Client.Serialisation;

namespace WCA.GlobalX.Client.Documents
{
    public class DocumentVersion
    {
        /// <summary>The cache id of the document version</summary>
        [JsonProperty("cacheId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CacheId { get; set; }

        /// <summary>The document id</summary>
        [JsonProperty("documentId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? DocumentId { get; set; }

        /// <summary>The document version id</summary>
        [JsonProperty("documentVersionId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? DocumentVersionId { get; set; }

        /// <summary>The name of the document</summary>
        [JsonProperty("documentName", NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentName { get; set; }

        /// <summary>The MIME type of the document</summary>
        [JsonProperty("mimeType", NullValueHandling = NullValueHandling.Ignore)]
        public string MimeType { get; set; }

        /// DISABLED. It does not seem to be populated, so we will ignore.
        /// We will use the /blob route to retrieve content instead.
        /// 
        /// <summary>The content of the document in binary stream format</summary>
        /// [JsonProperty("documentContent", NullValueHandling = NullValueHandling.Ignore)]
        /// public byte[] DocumentContent { get; set; }

        /// <summary>The size of the document in bytes</summary>
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public double? Size { get; set; }

        /// <summary>The status of the document</summary>
        [JsonProperty("statusDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusDescription { get; set; }

        /// <summary>The date and time when the document was last updated</summary>
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(OffsetDateTimeConverter))]
        public OffsetDateTime? Timestamp { get; set; }

        /// <summary>The order date when the document was purchased</summary>
        [JsonProperty("orderDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(OffsetDateTimeConverter))]
        public OffsetDateTime? OrderDate { get; set; }

        /// <summary>The sequence of the version of the document</summary>
        [JsonProperty("versionSequence", NullValueHandling = NullValueHandling.Ignore)]
        public int? VersionSequence { get; set; }

        /// <summary>Indicator if the PDF version of the document is still being processed</summary>
        [JsonProperty("isAwaitingPDF", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAwaitingPDF { get; set; }
    }
}
