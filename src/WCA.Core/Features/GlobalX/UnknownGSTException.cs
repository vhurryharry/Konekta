using System;
using System.Runtime.Serialization;

namespace WCA.Core.Features.GlobalX
{
    [Serializable]
    public class UnknownGSTException : Exception
    {
        public UnknownGSTException()
        {
        }

        public UnknownGSTException(string message) : base(message)
        {
        }

        public UnknownGSTException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownGSTException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}