using System;
using System.Runtime.Serialization;

namespace WCA.Domain
{
    public class WCAException : Exception
    {
        public WCAException()
        {
        }

        public WCAException(string message) : base(message)
        {
        }

        public WCAException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WCAException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
