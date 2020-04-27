using System;
using System.Runtime.Serialization;
using WCA.Actionstep.Client.Resources;

namespace WCA.Actionstep.Client
{
    [Serializable]
    public class TokenSetAlreadyLockedForRefreshException : Exception
    {
        public TokenSet TokenSet { get; }

        public TokenSetAlreadyLockedForRefreshException()
        {
        }

        public TokenSetAlreadyLockedForRefreshException(string message) : base(message)
        {
        }

        public TokenSetAlreadyLockedForRefreshException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenSetAlreadyLockedForRefreshException(TokenSet tokenSet)
            : this(CreateDefaultMessage(tokenSet))
        {
            TokenSet = tokenSet;
        }

        private static string CreateDefaultMessage(TokenSet tokenSet) =>
            $"Cannot refresh TokenSet with ID '{tokenSet?.Id}' for user '{tokenSet?.UserId}' because an existing lock was found and it has not yet expired." +
                $" Lock ID: '{tokenSet?.RefreshLockInfo?.LockId}'" +
                $", Lock Expires At: '{tokenSet?.RefreshLockInfo?.LockExpiresAt}'";

        protected TokenSetAlreadyLockedForRefreshException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}