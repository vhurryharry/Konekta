using System;
using WCA.Domain.Abstractions;
using WCA.Domain.Actionstep;

namespace WCA.Domain.GlobalX
{
    public enum DocumentCopyStatus
    {
        NotYetCopied,
        CopiedToActionstep,

        ErrorDownloadingFromGlobalX,
        ErrorUploadingToActionstep,

        MatterIdNotFoundInActionstep,
        MatterIdBelowMinimum,
        MatterIdUnableToParseAsInt,

        UnknownError,
    }

    public class GlobalXDocumentVersionState : EntityBase
    {
        public Guid DocumentId { get; set; }
        public Guid DocumentVersionId { get; set; }

        /// <summary>The order number that the document belongs to</summary>
        public string OrderId { get; set; }

        public ActionstepOrg ActionstepOrg { get; set; }
        public string ActionstepOrgKey { get; set; }

        /// <summary>
        /// Stored as a string because it is a string in GlobalX. This allows us to store state
        /// in the event an invalid matter comes back from GlobalX.
        /// </summary>
        public string MatterId { get; set; }

        /// <summary>
        /// If set, <see cref="DocumentCopyStatusUpdatedUtc"/> will be updated automatically.
        /// </summary>
        public DocumentCopyStatus DocumentCopyStatus { get; private set; } = DocumentCopyStatus.NotYetCopied;
        public DateTime DocumentCopyStatusUpdatedUtc { get; private set; }
        public string LastError { get; set; }

        /// <summary>The title of the document</summary>
        public string Title { get; set; }

        /// <summary>The status of the document</summary>
        public string StatusDescription { get; set; }

        /// <summary>The date and time when the document was last updated</summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>The date when the order was made</summary>
        public DateTime OrderDateUtc { get; set; }

        /// <summary>The user who initiated the document creation</summary>
        public string GlobalXUserId { get; set; }

        /// <summary>The criteria used to search for the document</summary>
        public string Criteria { get; set; }

        /// <summary>The offering type that the document belongs to</summary>
        public string OrderType { get; set; }

        /// <summary>The item number used to bill the purchase of the document</summary>
        public int? ItemNumber { get; set; }


        public string MimeType { get; set; }
        public Uri ActionstepSharePointUrl { get; set; }
        public int ActionstepActionDocumentId { get; set; }

        public void UpdateStatus(DocumentCopyStatus documentCopyStatus, string lastErrorMessage = null)
        {
            DocumentCopyStatus = documentCopyStatus;
            DocumentCopyStatusUpdatedUtc = DateTime.UtcNow;
            LastError = lastErrorMessage;
        }
    }
}
