using System;
using System.Runtime.Serialization;

namespace WCA.Actionstep.Client
{
    [Serializable]
    public class RetryCountExceededException : Exception
    {
        public int TotalAttempts { get; }

        public RetryCountExceededException()
        {
        }
        public RetryCountExceededException(string message) : base(message)
        {
        }

        public RetryCountExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RetryCountExceededException(int totalAttempts)
        {
            TotalAttempts = totalAttempts;
        }

        protected RetryCountExceededException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}