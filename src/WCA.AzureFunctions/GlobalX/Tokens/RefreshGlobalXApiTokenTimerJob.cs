using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WCA.GlobalX.Client;
using WCA.GlobalX.Client.Authentication;

namespace WCA.AzureFunctions.GlobalX.Tokens
{
    public class RefreshGlobalXApiTokenTimerJob
    {
        private readonly IGlobalXService _globalXService;
        private readonly IGlobalXApiTokenRepository _globalXApiTokenRepository;
        private readonly IClock _clock;
        private readonly ILogger<RefreshGlobalXApiTokenTimerJob> _logger;
        
        private readonly Duration _minimumRefreshTokenValidity = Duration.FromDays(47);

        public RefreshGlobalXApiTokenTimerJob(
            IGlobalXService globalXService,
            IGlobalXApiTokenRepository globalXApiTokenRepository,
            IClock clock,
            ILogger<RefreshGlobalXApiTokenTimerJob> logger)
        {
            _globalXService = globalXService ?? throw new ArgumentNullException(nameof(globalXService));
            _globalXApiTokenRepository = globalXApiTokenRepository ?? throw new ArgumentNullException(nameof(globalXApiTokenRepository));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(RefreshGlobalXApiTokenTimerJob))]
        public async Task Run(
            [TimerTrigger("0 0 */2 * *")] TimerInfo timerInfo, // Run this every two days
            [DurableClient] IDurableClient client,
            CancellationToken cancellationToken)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));

            // Retrieve all the entities and refresh them
            var individualCredentialRefreshExceptions = new List<Exception>();
            int refreshedCount = 0;
            int errorCount = 0;
            int totalTokenCount = 0;

            var earliestExpiry = _clock.GetCurrentInstant().Plus(_minimumRefreshTokenValidity);

            await foreach (var userIdToRefresh in _globalXApiTokenRepository.GetUserIdsNearingRefreshTokenExpiry(earliestExpiry))
            {
                totalTokenCount++;

                try
                {
                    refreshedCount++;
                    var refreshedToken = await _globalXService.RefreshAndPersistTokenForUser(userIdToRefresh);
                    _logger.LogInformation($"Successfully refreshed ApiToken. ApiToken for user: '{userIdToRefresh}', new refresh token expires at (UTC): '{refreshedToken.RefreshTokenExpiryUtc}'");
                }
                catch (Exception ex)
                {
                    errorCount++;
                    // Error while refreshing token
                    individualCredentialRefreshExceptions.Add(ex);
                    _logger.LogError(ex, $"Error encountered while refreshing ApiToken. ApiToken for user: '{userIdToRefresh}'.");
                }
            }

            if (totalTokenCount > 0)
            {
                _logger.LogInformation($"Finished processing {totalTokenCount} tokens. Refreshed: {refreshedCount}, errored: {errorCount}.");
            }
            else
            {
                _logger.LogInformation("No tokens found to refresh.");
            }

            if (individualCredentialRefreshExceptions.Count > 0)
            {
                // If there were any failures, throwing ensures that the TimerJob shows up as failed.
                throw new AggregateException("One or more refresh operations failed.", individualCredentialRefreshExceptions);
            }
        }
    }
}