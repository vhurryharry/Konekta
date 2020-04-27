using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class DocumentVersionAddedStateHandler : INotificationHandler<DocumentVersionAdded>
    {
        private readonly WCADbContext _wCADbContext;

        public DocumentVersionAddedStateHandler(
            WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task Handle(DocumentVersionAdded notification, CancellationToken cancellationToken)
        {
            if (notification is null) throw new ArgumentNullException(nameof(notification));
            if (notification.DocumentVersion is null) throw new ArgumentException("Document Version info must be supplied", nameof(notification));

            var documentVersionState = new GlobalXDocumentVersionState();
            documentVersionState.ActionstepOrgKey = notification.ActionstepOrgKey;

            documentVersionState.MatterId = notification.Document.MatterReference;
            documentVersionState.OrderId = notification.Document.OrderId;
            documentVersionState.Title = notification.Document.Title;
            documentVersionState.GlobalXUserId = notification.Document.UserId;
            documentVersionState.Criteria = notification.Document.Criteria;
            documentVersionState.OrderType = notification.Document.OrderType;
            documentVersionState.ItemNumber = notification.Document.ItemNumber;

            documentVersionState.DocumentId = notification.DocumentVersion.DocumentId.Value;
            documentVersionState.DocumentVersionId = notification.DocumentVersion.DocumentVersionId.Value;
            documentVersionState.StatusDescription = notification.DocumentVersion.StatusDescription;
            documentVersionState.TimestampUtc = notification.DocumentVersion.Timestamp.Value.ToDateTimeOffset().UtcDateTime;
            documentVersionState.OrderDateUtc = notification.DocumentVersion.OrderDate.Value.ToDateTimeOffset().UtcDateTime;
            documentVersionState.MimeType = notification.DocumentVersion.MimeType;

            _wCADbContext.GlobalXDocumentVersionStates.Add(documentVersionState);

            await _wCADbContext.SaveChangesAsync();
        }
    }
}
