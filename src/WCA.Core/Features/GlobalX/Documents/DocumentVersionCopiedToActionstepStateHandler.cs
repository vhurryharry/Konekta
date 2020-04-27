using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class DocumentVersionCopiedToActionstepStateHandler : INotificationHandler<DocumentVersionCopiedToActionstep>
    {
        private readonly WCADbContext _wCADbContext;

        public DocumentVersionCopiedToActionstepStateHandler(
            WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task Handle(DocumentVersionCopiedToActionstep notification, CancellationToken cancellationToken)
        {
            if (notification is null) throw new ArgumentNullException(nameof(notification));

            var currentState = await _wCADbContext.GlobalXDocumentVersionStates.FindAsync(notification.DocumentVersionId);

            currentState.ActionstepSharePointUrl = notification.TransactionDisbursementRelationship.ActionstepSharePointUrl;
            currentState.ActionstepActionDocumentId = notification.TransactionDisbursementRelationship.ActionstepActionDocumentId;
            currentState.MatterId = notification.TransactionDisbursementRelationship.ActionstepMatterId.ToString(CultureInfo.InvariantCulture);

            currentState.UpdateStatus(DocumentCopyStatus.CopiedToActionstep);

            await _wCADbContext.SaveChangesAsync();
        }
    }
}
