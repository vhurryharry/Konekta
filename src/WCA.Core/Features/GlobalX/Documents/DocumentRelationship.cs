using System;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class DocumentRelationship
    {
        public Guid GlobalXDocumentId { get; }
        public Guid GlobalXDocumentVersionId { get; }
        public string FileName { get; }
        public string MimeType { get; }
        public string ActionstepOrgKey { get; }
        public int ActionstepMatterId { get; }
        public int ActionstepActionDocumentId { get; }
        public Uri ActionstepSharePointUrl { get; }

        public DocumentRelationship(
            Guid globalXDocumentId,
            Guid globalXDocumentVersionId,
            string fileName,
            string mimeType,
            string actionstepOrgKey,
            int actionstepMatterId,
            int actionstepActionDocumentId,
            Uri actionstepSharePointUrl)
        {
            GlobalXDocumentId = globalXDocumentId;
            GlobalXDocumentVersionId = globalXDocumentVersionId;
            FileName = fileName;
            MimeType = mimeType;
            ActionstepOrgKey = actionstepOrgKey;
            ActionstepMatterId = actionstepMatterId;
            ActionstepActionDocumentId = actionstepActionDocumentId;
            ActionstepSharePointUrl = actionstepSharePointUrl;
        }
    }
}
