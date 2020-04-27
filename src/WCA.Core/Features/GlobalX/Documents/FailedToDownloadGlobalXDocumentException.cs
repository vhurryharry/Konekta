using System;
using System.Runtime.Serialization;

namespace WCA.Core.Features.GlobalX.Documents
{
    [Serializable]
    public class FailedToDownloadGlobalXDocumentException : Exception
    {
        public FailedToDownloadGlobalXDocumentException()
        {
        }

        public FailedToDownloadGlobalXDocumentException(Exception ex)
            :base("Unknown error downloading document from GlobalX. See InnerException for details.", ex)
        {
        }

        public FailedToDownloadGlobalXDocumentException(string message) : base(message)
        {
        }

        public FailedToDownloadGlobalXDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FailedToDownloadGlobalXDocumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}