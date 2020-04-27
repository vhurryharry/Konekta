using System;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client.Authentication
{
    [Serializable]
    public class GlobalXApiCredentialsNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="GlobalXApiCredentialsNotFoundException"/> with the specified
        /// UserId and with a default message.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static GlobalXApiCredentialsNotFoundException FromUserId(string userId)
        {
            return new GlobalXApiCredentialsNotFoundException()
            {
                UserId = userId
            };
        }

        /// <summary>
        /// The UserId for which a token could not be found.
        /// </summary>
        public string UserId { get; private set; }

        protected GlobalXApiCredentialsNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }

        public GlobalXApiCredentialsNotFoundException()
        { }

        public GlobalXApiCredentialsNotFoundException(string message) : base(message)
        { }

        public GlobalXApiCredentialsNotFoundException(string message, string userId)
            : base(message)
        {
            UserId = userId;
        }

        public GlobalXApiCredentialsNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
