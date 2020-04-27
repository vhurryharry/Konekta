using System;
using System.Runtime.Serialization;

namespace WCA.Core.Services
{
    [Serializable]
    public class FailedToSendEmailException : Exception
    {
        public FailedToSendEmailException()
        {
        }

        public FailedToSendEmailException(string message) : base(message)
        {
        }

        public FailedToSendEmailException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FailedToSendEmailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}