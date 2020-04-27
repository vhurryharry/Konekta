using System;
using System.Runtime.Serialization;

namespace WCA.AzureFunctions.GlobalX.Documents
{
    [Serializable]
    public class DocumentNotSuppliedException : Exception
    {
        public DocumentNotSuppliedException()
        {
        }

        public DocumentNotSuppliedException(string message) : base(message)
        {
        }

        public DocumentNotSuppliedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DocumentNotSuppliedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}