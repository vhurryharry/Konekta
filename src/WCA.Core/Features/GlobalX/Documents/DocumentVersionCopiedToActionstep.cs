using MediatR;
using System;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class DocumentVersionCopiedToActionstep : INotification
    {
        public Guid DocumentVersionId { get; set; }
        public DocumentRelationship TransactionDisbursementRelationship { get; set; }

        public DocumentVersionCopiedToActionstep(
            Guid documentVersionId,
            DocumentRelationship transactionDisbursementRelationship)
        {
            DocumentVersionId = documentVersionId;
            TransactionDisbursementRelationship = transactionDisbursementRelationship;
        }
    }
}
