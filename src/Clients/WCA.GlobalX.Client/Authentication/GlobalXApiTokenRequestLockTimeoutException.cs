using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client.Authentication
{
    [Serializable]
    public class GlobalXApiTokenRequestLockTimeoutException : Exception
    {
        public GlobalXApiTokenLockInfo CurrentLock { get; set; }
        public string UserId { get; set; }

        public GlobalXApiTokenRequestLockTimeoutException()
        {
        }

        public GlobalXApiTokenRequestLockTimeoutException(string message) : base(message)
        {
        }

        public GlobalXApiTokenRequestLockTimeoutException(GlobalXApiTokenLockInfo currentLock, string userId)
            : base($"Request to lock API token timed out. " +
                  $"User: '{userId}', " +
                  $"existing lock ID: '{currentLock?.LockId}', " +
                  $"expiring at '{currentLock?.ExpiresAt.ToString("g", CultureInfo.InvariantCulture)}'.")
        {
            CurrentLock = currentLock;
            UserId = userId;
        }

        public GlobalXApiTokenRequestLockTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GlobalXApiTokenRequestLockTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
