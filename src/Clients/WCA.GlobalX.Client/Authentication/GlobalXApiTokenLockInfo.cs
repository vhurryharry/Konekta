using NodaTime;
using System;

namespace WCA.GlobalX.Client.Authentication
{
    public class GlobalXApiTokenLockInfo : IDisposable
    {
        /// <summary>
        /// A reference to the <see cref="IGlobalXApiTokenRepository"/> where this lock
        /// is stored. This is used to delete the lock when it is disposed.
        /// </summary>
        private readonly IGlobalXApiTokenRepository _globalXApiTokenRepository;

        public GlobalXApiTokenLockInfo(string userId, Instant expiresAt, IGlobalXApiTokenRepository globalXApiTokenRepository)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Must be supplied", nameof(userId));

            _globalXApiTokenRepository = globalXApiTokenRepository ?? throw new ArgumentNullException(nameof(globalXApiTokenRepository));
            UserId = userId;
            ExpiresAt = expiresAt;
            LockId = Guid.NewGuid();
        }

        public GlobalXApiTokenLockInfo(string userId, Instant expiresAt, IGlobalXApiTokenRepository globalXApiTokenRepository, Guid lockId)
        {
            UserId = userId;
            ExpiresAt = expiresAt;
            LockId = lockId;
            _globalXApiTokenRepository = globalXApiTokenRepository;
        }

        public string UserId { get; }
        public Guid LockId { get; }
        public Instant ExpiresAt { get; }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    /// Remove this lock when disposing to allow for <see cref="GlobalXApiTokenLockInfo"/>
                    /// to be used inside a using statement.
                    _globalXApiTokenRepository?.DeleteLock(this).GetAwaiter().GetResult();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}