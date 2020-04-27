using NodaTime;
using System;

namespace WCA.Actionstep.Client.Resources
{
    public class TokenSetRefreshLockInfo
    {
        public TokenSetRefreshLockInfo(Instant lockExpiresAt)
        {
            LockExpiresAt = lockExpiresAt;
            LockId = Guid.NewGuid();
        }

        public TokenSetRefreshLockInfo(Instant lockExpiresAt, Guid existingLockId)
        {
            LockExpiresAt = lockExpiresAt;
            LockId = existingLockId;
        }

        public Instant LockExpiresAt { get; }
        public Guid LockId { get; }
    }
}