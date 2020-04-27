using System;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client.Authentication
{
    [Serializable]
    public class GlobalXApiTokenNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="GlobalXApiTokenNotFoundException"/> with the specified
        /// UserId and with a default message.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static GlobalXApiTokenNotFoundException FromUserId(string userId)
        {
            return new GlobalXApiTokenNotFoundException()
            {
                UserId = userId
            };
        }

        /// <summary>
        /// The UserId for which a token could not be found.
        /// </summary>
        public string UserId { get; private set; }

        protected GlobalXApiTokenNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }

        public GlobalXApiTokenNotFoundException()
        { }

        public GlobalXApiTokenNotFoundException(string message) : base(message)
        { }

        public GlobalXApiTokenNotFoundException(string message, string userId)
            : base(message)
        {
            UserId = userId;
        }

        public GlobalXApiTokenNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public GlobalXApiTokenNotFoundException(string message, string userId, Exception innerException)
            : this(message, innerException)
        {
            UserId = userId;
        }
    }
}
