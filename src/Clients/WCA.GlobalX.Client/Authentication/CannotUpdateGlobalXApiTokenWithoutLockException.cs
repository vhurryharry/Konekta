using System;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client.Authentication
{
    [Serializable]
    public class CannotUpdateGlobalXApiTokenWithoutLockException : Exception
    {
        public CannotUpdateGlobalXApiTokenWithoutLockException()
        {
        }

        public CannotUpdateGlobalXApiTokenWithoutLockException(GlobalXApiToken apiToken)
            : base("Cannot update GlobalXApiToken because a lock was not provided first." +
                  $" The token belongs to user with ID: '{apiToken?.UserId}'.")
        {
            ApiToken = apiToken;
        }

        public CannotUpdateGlobalXApiTokenWithoutLockException(string message) : base(message)
        {
        }

        public CannotUpdateGlobalXApiTokenWithoutLockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotUpdateGlobalXApiTokenWithoutLockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public GlobalXApiToken ApiToken { get; }
    }
}