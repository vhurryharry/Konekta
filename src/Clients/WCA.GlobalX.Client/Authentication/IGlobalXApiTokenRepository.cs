using NodaTime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WCA.GlobalX.Client.Authentication
{
    public interface IGlobalXApiTokenRepository
    {
        /// <summary>
        /// DO NOT USE this from your code. Use <see cref="IGlobalXService.SafeAddOrUpdateGlobalXApiToken(GlobalXApiToken)"/>
        /// instead, because it will safely request a lock before calling this method.
        /// 
        /// Adds or updates the supplied <see cref="GlobalXApiToken"/>. A lock must be requested before you can add or update
        /// a token, so you must supply a valid <see cref="GlobalXApiTokenLockInfo"/>.
        /// </summary>
        /// <param name="apiToken"></param>
        /// <param name="lockInfo"></param>
        /// <returns></returns>
        /// <exception cref="CannotUpdateGlobalXApiTokenWithoutLockException">If there is no existing lock for this token.</exception>
        /// <exception cref="GlobalXApiTokenLockIdMismatchException">
        ///     If the <see cref="GlobalXApiTokenLockInfo"/> supplied doesn't match the one stored for the token.
        /// </exception>
        Task AddOrUpdateGlobalXApiToken(GlobalXApiToken apiToken, GlobalXApiTokenLockInfo lockInfo, bool overrideAndClearLock = false);

        /// <summary>
        /// Retrieve <see cref="GlobalXApiToken"/> stored for a given user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<GlobalXApiToken> GetTokenForUser(string userId);

        IAsyncEnumerable<string> GetUserIdsNearingRefreshTokenExpiry(Instant refreshExpiryIsLessThanOrEqualTo);

        Task<GlobalXApiTokenLockInfo> GetLock(string userId);
        Task SetLock(GlobalXApiTokenLockInfo lockInfo);
        Task DeleteLock(GlobalXApiTokenLockInfo lockInfo);
    }
}
