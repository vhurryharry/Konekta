using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client.Authentication
{
    [Serializable]
    public class InvalidGlobalXCredentialsException : Exception
    {
        /// <summary>
        /// The UserId for which a token could not be found.
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// The full JObject response from GlobalX
        /// </summary>
        public JObject Response { get; }

        protected InvalidGlobalXCredentialsException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }

        public InvalidGlobalXCredentialsException()
        { }

        public InvalidGlobalXCredentialsException(string userId, JObject response)
        {
            UserId = userId;
            Response = response;
        }

        public InvalidGlobalXCredentialsException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public InvalidGlobalXCredentialsException(string message) : base(message)
        { }
    }
}
