using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Actionstep.Client;

namespace Actionstep
{
    public class RefreshAllTokens
    {
        private readonly ITokenSetRepository _tokenSetRepository;
        private readonly IActionstepService _actionstepService;
        private readonly IClock _clock;

        public RefreshAllTokens(
            ITokenSetRepository tokenSetRepository,
            IActionstepService actionstepService,
            IClock clock)
        {
            _tokenSetRepository = tokenSetRepository;
            _actionstepService = actionstepService;
            _clock = clock;
        }

        /// <summary>
        /// The CRON syntax in TimerTrigger attribute means that this will execute daily at 3AM
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="logger"></param>
        /// <param name="settings"></param>
        /// <param name="mediator"></param>
        /// <returns></returns>
        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(RefreshAllTokens))]
        public async Task Run(
#pragma warning disable CA1801 // Remove unused parameter: Required by framework
            [TimerTrigger("0 */30 * * * *")]TimerInfo myTimer,
#pragma warning restore CA1801 // Remove unused parameter
            ILogger logger)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            var individualCredentialRefreshExceptions = new List<Exception>();

            // Ensures all tokens are returned
            const int minimumTokenValidityInDays = 60;
            var latestRefreshTokenExpiryToRefresh = _clock.GetCurrentInstant().Plus(Duration.FromDays(minimumTokenValidityInDays));

            logger.LogInformation(
                "Retrieving tokens expiring within the next {MinimumTokenValidityInDays} days. Checking for tokens that expire between now and before {LatestRefreshTokenExpiryToRefresh}.",
                minimumTokenValidityInDays, latestRefreshTokenExpiryToRefresh);

            var tokensNearingExpiry = await _tokenSetRepository.GetTokensByRefreshExpiry(latestRefreshTokenExpiryToRefresh);
            int refreshedCount = 0;
            int skippedCount = 0;
            int errorCount = 0;
            int totalTokenCount = 0;
            foreach (var tokenSet in tokensNearingExpiry)
            {
                totalTokenCount++;

                try
                {
                    if (!tokenSet.RefreshTokenAppearsValid(_clock))
                    {
                        skippedCount++;
                        var revokedAt = tokenSet.RevokedAt.HasValue ? tokenSet.RevokedAt.Value.ToString() : "Not revoked";
                        logger.LogInformation(
                            "Skipping invalid TokenSet. TokenSet ID: '{TokenSetId}', User ID: '{UserId}', OrgKey: '{OrgKey}', Revoked at (UTC): '{RevokedAt}', Refresh token expires at (UTC): '{RefreshTokenExpiresAt}'",
                            tokenSet.Id, tokenSet.UserId, tokenSet.OrgKey, revokedAt, tokenSet.RefreshTokenExpiresAt.ToString());
                    }
                    else
                    {
                        await _actionstepService.RefreshAccessTokenIfExpired(tokenSet, forceRefresh: true);
                        refreshedCount++;

                        logger.LogInformation(
                            "Successfully refreshed TokenSet. TokenSet ID: '{TokenSetId}', User ID: '{UserId}', OrgKey: '{OrgKey}', Refresh token expires at (UTC): '{RefreshTokenExpiresAt}'",
                            tokenSet.Id, tokenSet.UserId, tokenSet.OrgKey, tokenSet.RefreshTokenExpiresAt.ToString());
                    }
                }
#pragma warning disable CA1031 /// Do not catch general exception types: Exception is logged and captured in AggregatException. We don't want one credential failure to prevent others from being refreshed.
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    individualCredentialRefreshExceptions.Add(ex);
                    errorCount++;

                    var revokedAt = tokenSet.RevokedAt.HasValue ? tokenSet.RevokedAt.Value.ToString() : "Not revoked";
                    logger.LogError(
                        ex,
                        "Error encountered while refreshingTokenSet. TokenSet ID: '{TokenSetId}', User ID: '{UserId}', OrgKey: '{OrgKey}', Revoked at (UTC): '{RevokedAt}', Refresh token expires at (UTC): '{RefreshTokenExpiresAt}'",
                        tokenSet.Id, tokenSet.UserId, tokenSet.OrgKey, revokedAt, tokenSet.RefreshTokenExpiresAt.ToString());
                }
            }

            if (totalTokenCount > 0)
            {
                logger.LogInformation(
                    "Finished processing {TotalTokenCount} tokens. Refreshed: {RefreshedCount}, skipped: {SkippedCount}, errored: {ErrorCount}.",
                    totalTokenCount, refreshedCount, skippedCount, errorCount);
            }
            else
            {
                logger.LogInformation("No tokens found that are close to expiry.");
            }

            if (individualCredentialRefreshExceptions.Count > 0)
            {
                // If there were any failures, throwing ensures that the TimerJob shows up as failed.
                throw new AggregateException("One or more refresh operations failed.", individualCredentialRefreshExceptions);
            }
        }
    }
}
