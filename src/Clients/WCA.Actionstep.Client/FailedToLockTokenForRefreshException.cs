using System;
using System.Runtime.Serialization;

namespace WCA.Actionstep.Client
{
    [Serializable]
    public class FailedToLockTokenForRefreshException : Exception
    {
        public string TokenSetId { get; }

        public FailedToLockTokenForRefreshException()
        {
        }

        public FailedToLockTokenForRefreshException(string message) : base(message)
        {
        }

        public FailedToLockTokenForRefreshException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FailedToLockTokenForRefreshException(string message, string tokenSetId) : this(message)
        {
            TokenSetId = tokenSetId;
        }

        protected FailedToLockTokenForRefreshException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}