using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class DocumentVersionMatterIdUpdatedStateHandler : INotificationHandler<DocumentVersionMatterIdUpdated>
    {
        private readonly WCADbContext _wCADbContext;

        public DocumentVersionMatterIdUpdatedStateHandler(
            WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task Handle(DocumentVersionMatterIdUpdated notification, CancellationToken cancellationToken)
        {
            if (notification is null) throw new ArgumentNullException(nameof(notification));

            var currentState = await _wCADbContext.GlobalXDocumentVersionStates.FindAsync(notification.DocumentVersionId);

            currentState.MatterId = notification.NewMatterId.ToString(CultureInfo.InvariantCulture);

            await _wCADbContext.SaveChangesAsync();
        }
    }
}
