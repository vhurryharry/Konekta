using System;
using System.Runtime.Serialization;

namespace WCA.Actionstep.Client.Resources
{
    /// <summary>
    /// To be used when a malformed or invalid token response is received.
    /// 
    /// NOT to be used for known scenarios such as invalid or expired tokens. This is for invalid *responses* only.
    /// </summary>
    [Serializable]
    public class InvalidTokenResponseException : Exception
    {
        public InvalidTokenResponseException()
        {
        }

        public InvalidTokenResponseException(string message) : base(message)
        {
        }

        public InvalidTokenResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTokenResponseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}