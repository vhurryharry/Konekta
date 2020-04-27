using System;
using System.Runtime.Serialization;

namespace WCA.Core.Features.GlobalX.Documents
{
    [Serializable]
    public class FailedToUploadGlobalXDocumentToActionstepException : Exception
    {
        public FailedToUploadGlobalXDocumentToActionstepException()
        {
        }

        public FailedToUploadGlobalXDocumentToActionstepException(Exception ex)
            : base("Unknown error uploading GlobalX Document to Actionstep. See InnerException for details.", ex)
        {
        }

        public FailedToUploadGlobalXDocumentToActionstepException(string message) : base(message)
        {
        }

        public FailedToUploadGlobalXDocumentToActionstepException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FailedToUploadGlobalXDocumentToActionstepException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}