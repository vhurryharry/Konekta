
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Core.Extensions;
using WCA.Core.Services;
using WCA.GlobalX.Client.Authentication;

namespace WCA.Core.Features.GlobalX.Authentication
{
    public class GlobalXApiTokenRepository : IGlobalXApiTokenRepository
    {
        private const string _credentialNamePrefix = "GlobalX-API-token-for-user-";
        private readonly ITelemetryLogger _telemetry;
        private readonly IKeyVaultClient _keyVaultClient;
        private readonly IClock _clock;
        private readonly WCACoreSettings _appSettings;

        private const string _globalXTokenLockTableName = "GlobalXApiTokenLocks";
        private readonly CloudTable _lockTableReference;

        private const string _globalXTokenExpiryTableName = "GlobalXApiTokenExpiry";
        private readonly CloudTable _expiryTableReference;

        public GlobalXApiTokenRepository(
            ITelemetryLogger telemetry,
            IKeyVaultClient keyVaultClient,
            CloudStorageAccount cloudStorageAccount,
            IClock clock,
            IOptions<WCACoreSettings> appSettingsAccessor)
        {
            if (cloudStorageAccount is null) throw new ArgumentNullException(nameof(cloudStorageAccount));
            if (appSettingsAccessor is null) throw new ArgumentNullException(nameof(appSettingsAccessor));

            _telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
            _appSettings = appSettingsAccessor.Value;
            _keyVaultClient = keyVaultClient ?? throw new ArgumentNullException(nameof(keyVaultClient));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            _lockTableReference = tableClient.GetTableReference(_globalXTokenLockTableName);
            _lockTableReference.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            _expiryTableReference = tableClient.GetTableReference(_globalXTokenExpiryTableName);
            _expiryTableReference.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }

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
        public async Task AddOrUpdateGlobalXApiToken(GlobalXApiToken apiToken, GlobalXApiTokenLockInfo lockInfo, bool overrideAndClearLock = false)
        {
            if (apiToken is null) { throw new ArgumentNullException(nameof(apiToken)); }
            if (lockInfo is null && !overrideAndClearLock)
            {
                throw new ArgumentNullException(nameof(lockInfo),
                    $"Must not be null if {nameof(overrideAndClearLock)} is false.");
            }

            if (!(lockInfo is null))
            {
                var existingLock = await GetLock(lockInfo?.UserId);

                if (overrideAndClearLock)
                {
                    await DeleteLock(existingLock);
                }
                else
                {
                    if (existingLock is null)
                    {
                        throw new CannotUpdateGlobalXApiTokenWithoutLockException(apiToken);
                    }

                    if (existingLock.LockId != lockInfo.LockId)
                    {
                        throw new GlobalXApiTokenLockIdMismatchException(existingLock, lockInfo, apiToken);
                    }
                }
            }

            _telemetry.TrackEvent(nameof(AddOrUpdateGlobalXApiToken), new Dictionary<string, string>() {
                { "Access Token User Id", apiToken.UserId },
                { "Access Token Expires At", apiToken.AccessTokenExpiryUtc.ToString() },
            });

            // Store or update the expiry dates in table storage as a read model for quicker enumeration tokens nearing expiry
            DateTimeOffset? revokedAt = null;
            if (apiToken.RevokedAt.HasValue)
            {
                revokedAt = apiToken.RevokedAt.Value.ToDateTimeOffset();
            }

            await _expiryTableReference.ExecuteAsync(TableOperation.InsertOrReplace(new TokenExpiryTableEntity(
                apiToken.RefreshTokenExpiryUtc.ToDateTimeUtc(),
                apiToken.AccessTokenExpiryUtc.ToDateTimeUtc(),
                apiToken.UserId,
                revokedAt)));

            // Store the actual token data
            await _keyVaultClient.SetSecretAsync(
                _appSettings.CredentialAzureKeyVaultUrl,
                GenerateTokenId(apiToken.UserId),
                JsonConvert.SerializeObject(apiToken));
        }

        public async Task<GlobalXApiToken> GetTokenForUser(string userId)
        {
            var tokenId = GenerateTokenId(userId);
            var secretBundle = await _keyVaultClient.GetSecretOrNullAsync(_appSettings.CredentialAzureKeyVaultUrl, tokenId);

            if (secretBundle != null && !string.IsNullOrEmpty(secretBundle.Value))
            {
                var apiToken = JsonConvert.DeserializeObject<GlobalXApiToken>(secretBundle.Value);
                return apiToken;
            }
            else
            {
                return null;
            }
        }

        public async Task SetLock(GlobalXApiTokenLockInfo lockInfo)
        {
            await _lockTableReference.ExecuteAsync(TableOperation.InsertOrReplace(new LockInfoTableEntity(lockInfo)));
        }

        public async IAsyncEnumerable<string> GetUserIdsNearingRefreshTokenExpiry(Instant refreshExpiryIsLessThanOrEqualTo)
        {
            TableContinuationToken tableContinuationToken = null;
            var dateTimeOffsetFilter = refreshExpiryIsLessThanOrEqualTo.ToDateTimeOffset();
            var latestRevokedAt = _clock.GetCurrentInstant().ToDateTimeOffset();

            do
            {
                var query = TableQuery.GenerateFilterConditionForDate(
                                "RefreshTokenExpiryUtc",
                                QueryComparisons.LessThanOrEqual,
                                dateTimeOffsetFilter
                            );

                var queryResult = await _expiryTableReference.ExecuteQuerySegmentedAsync(
                    new TableQuery<TokenExpiryTableEntity>().Where(query),
                    tableContinuationToken);

                foreach (var entity in queryResult.Results)
                {
                    // Only return non-revoked tokens
                    if (!(entity.RevokedAtUtc.HasValue && entity.RevokedAtUtc.Value < latestRevokedAt))
                    {
                        yield return entity.UserId;
                    }
                }

                tableContinuationToken = queryResult.ContinuationToken;

            } while (tableContinuationToken != null);

        }

        public async Task<GlobalXApiTokenLockInfo> GetLock(string userId)
        {
            var existingLockEntity = await GetLockInfoTableEntity(userId);

            if (existingLockEntity != null)
            {
                return new GlobalXApiTokenLockInfo(
                    existingLockEntity.PartitionKey,
                    Instant.FromDateTimeUtc(existingLockEntity.ExpiresAtUtc),
                    this,
                    existingLockEntity.LockId);
            }

            return null;
        }

        public async Task DeleteLock(GlobalXApiTokenLockInfo lockInfo)
        {
            if (lockInfo is null)
                return;

            var existingLockEntity = await GetLockInfoTableEntity(lockInfo.UserId);

            // If no existing lock, nothing to delete
            if (existingLockEntity is null)
                return;

            if (lockInfo.LockId == existingLockEntity.LockId)
                await _lockTableReference.ExecuteAsync(TableOperation.Delete(existingLockEntity));
        }

        private async Task<LockInfoTableEntity> GetLockInfoTableEntity(string userId)
        {
            var tableEntry = await _lockTableReference.ExecuteAsync(
                TableOperation.Retrieve<LockInfoTableEntity>(userId, LockInfoTableEntity.RowKey));

            if (tableEntry.Result is LockInfoTableEntity lockInfoTableEntity)
            {
                return lockInfoTableEntity;
            }

            return null;
        }

        private async Task<TokenExpiryTableEntity> GetTokenExpiryTableEntity(string userId)
        {
            var tableEntry = await _expiryTableReference.ExecuteAsync(
                TableOperation.Retrieve<TokenExpiryTableEntity>(userId, TokenExpiryTableEntity.RowKey));

            if (tableEntry.Result is TokenExpiryTableEntity tokenExpiryTableEntity)
            {
                return tokenExpiryTableEntity;
            }

            return null;
        }

        private static string GenerateTokenId(string userId)
        {
            return $"{_credentialNamePrefix}{userId}";
        }
    }
}
