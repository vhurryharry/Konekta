using System;
using System.Runtime.Serialization;

namespace WCA.Actionstep.Client
{
    [Serializable]
    public class InvalidJwtDiscoveryResponseException : Exception
    {
        public int TotalAttempts { get; }

        public InvalidJwtDiscoveryResponseException()
        {
        }
        public InvalidJwtDiscoveryResponseException(string message) : base(message)
        {
        }

        public InvalidJwtDiscoveryResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidJwtDiscoveryResponseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}