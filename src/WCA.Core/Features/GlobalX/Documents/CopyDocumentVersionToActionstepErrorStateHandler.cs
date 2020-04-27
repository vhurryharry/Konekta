using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class CopyDocumentVersionToActionstepErrorStateHandler : INotificationHandler<CopyDocumentVersionToActionstepError>
    {
        private readonly WCADbContext _wCADbContext;

        public CopyDocumentVersionToActionstepErrorStateHandler(
            WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task Handle(CopyDocumentVersionToActionstepError notification, CancellationToken cancellationToken)
        {
            if (notification is null) throw new ArgumentNullException(nameof(notification));

            var currentState = await _wCADbContext.GlobalXDocumentVersionStates.FindAsync(notification.DocumentVersionId);

            var documentCopyStatus = notification.CopyErrorType switch
            {
                CopyErrorType.DownloadFromGlobalXError => DocumentCopyStatus.ErrorDownloadingFromGlobalX,
                CopyErrorType.UploadToActionstepError => DocumentCopyStatus.ErrorUploadingToActionstep,
                CopyErrorType.MatterIdNotFoundInActionstep => DocumentCopyStatus.MatterIdNotFoundInActionstep,
                CopyErrorType.MatterIdBelowMinimum => DocumentCopyStatus.MatterIdBelowMinimum,
                CopyErrorType.MatterIdUnableToParseAsInt => DocumentCopyStatus.MatterIdUnableToParseAsInt,
                _ => DocumentCopyStatus.UnknownError,
            };

            currentState.UpdateStatus(documentCopyStatus, notification.ErrorMessage);

            await _wCADbContext.SaveChangesAsync();
        }
    }
}
