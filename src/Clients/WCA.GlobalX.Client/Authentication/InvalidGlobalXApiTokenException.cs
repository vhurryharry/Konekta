using System;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client.Authentication
{
    [Serializable]
    public class InvalidGlobalXApiTokenException : Exception
    {
        public GlobalXApiToken ApiToken { get; private set; }

        public InvalidGlobalXApiTokenException()
        {
        }

        public InvalidGlobalXApiTokenException(GlobalXApiToken apiToken)
        {
            ApiToken = apiToken;
        }

        public InvalidGlobalXApiTokenException(string message, GlobalXApiToken apiToken) : base(message)
        {
            ApiToken = apiToken;
        }


        public InvalidGlobalXApiTokenException(string message) : base(message)
        {
        }

        public InvalidGlobalXApiTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidGlobalXApiTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}