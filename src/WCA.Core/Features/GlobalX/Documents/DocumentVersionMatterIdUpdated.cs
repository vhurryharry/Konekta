using MediatR;
using System;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class DocumentVersionMatterIdUpdated : INotification
    {
        public Guid DocumentVersionId { get; set; }

        public string ActionstepOrgKey { get; set; }
        public string OldMatterId { get; set; }
        public int NewMatterId { get; set; }

        public DocumentVersionMatterIdUpdated(Guid documentVersionId, string actionstepOrgKey, string oldMatterId, int newMatterId)
        {
            DocumentVersionId = documentVersionId;
            ActionstepOrgKey = actionstepOrgKey;
            OldMatterId = oldMatterId;
            NewMatterId = newMatterId;
        }
    }
}
