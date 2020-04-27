using MediatR;
using System;

namespace WCA.Core.Features.GlobalX.Documents
{
    public enum CopyErrorType
    {
        DownloadFromGlobalXError,
        UploadToActionstepError,
        MatterIdNotFoundInActionstep,
        MatterIdBelowMinimum,
        MatterIdUnableToParseAsInt,
        UnknownError,
    }

    public class CopyDocumentVersionToActionstepError : INotification
    {
        public Guid DocumentVersionId { get; set; }
        public CopyErrorType CopyErrorType { get;set; }
        public string ErrorMessage { get; set; }

        public CopyDocumentVersionToActionstepError(
            Guid documentVersionId,
            CopyErrorType copyErrorType,
            string errorMessage)
        {
            DocumentVersionId = documentVersionId;
            CopyErrorType = copyErrorType;
            ErrorMessage = errorMessage;
        }
    }
}
