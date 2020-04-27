using System;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client.Authentication
{
    [Serializable]
    public class GlobalXApiTokenLockIdMismatchException : Exception
    {
        public GlobalXApiTokenLockInfo ExistingLock { get; }
        public GlobalXApiTokenLockInfo LockInfo { get; }
        public GlobalXApiToken GlobalXApiToken { get; }

        public GlobalXApiTokenLockIdMismatchException()
        {
        }

        public GlobalXApiTokenLockIdMismatchException(string message) : base(message)
        {
        }

        public GlobalXApiTokenLockIdMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public GlobalXApiTokenLockIdMismatchException(
                GlobalXApiTokenLockInfo existingLock,
                GlobalXApiTokenLockInfo lockInfoSuppliedWithRequest,
                GlobalXApiToken globalXApiToken)
            : base($"An attempt was made to update GlobalXApiToken for user '{globalXApiToken?.UserId}'," +
                  $" however the lock ID supplied was '{lockInfoSuppliedWithRequest?.LockId}'" +
                  $" which doesn't match the existing lock of '{existingLock?.LockId}'.")
        {
            ExistingLock = existingLock;
            LockInfo = lockInfoSuppliedWithRequest;
            GlobalXApiToken = globalXApiToken;
        }

        protected GlobalXApiTokenLockIdMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}