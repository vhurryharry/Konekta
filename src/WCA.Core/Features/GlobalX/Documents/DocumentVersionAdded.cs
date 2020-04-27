using MediatR;
using WCA.GlobalX.Client.Documents;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class DocumentVersionAdded : INotification
    {
        public string ActionstepOrgKey { get; set; }
        public Document Document { get; set; }
        public DocumentVersion DocumentVersion { get; set; }

        public DocumentVersionAdded(string actionstepOrgKey, Document document, DocumentVersion documentVersion)
        {
            ActionstepOrgKey = actionstepOrgKey;
            Document = document;
            DocumentVersion = documentVersion;
        }
    }
}
