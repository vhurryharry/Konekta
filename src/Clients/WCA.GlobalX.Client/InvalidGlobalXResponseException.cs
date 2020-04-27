using System;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client
{
    [Serializable]
    public class InvalidGlobalXResponseException : Exception
    {
        public InvalidGlobalXResponseException()
        {
        }

        public InvalidGlobalXResponseException(string message) : base(message)
        {
        }

        public InvalidGlobalXResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidGlobalXResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}