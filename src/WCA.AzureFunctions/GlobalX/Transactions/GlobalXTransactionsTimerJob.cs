using FluentValidation;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using WCA.Core.Features.GlobalX.Settings;
using WCA.Core.Features.GlobalX.Sync;
using WCA.Core.Features.GlobalX.Transactions;
using WCA.Domain.GlobalX;
using WCA.GlobalX.Client;
using WCA.GlobalX.Client.Transactions;

namespace WCA.AzureFunctions.GlobalX.Transactions
{
    public class GlobalXTransactionsTimerJob
    {
        private readonly IMediator _mediator;
        private readonly IGlobalXService _globalXService;
        private readonly ILogger<GlobalXTransactionsTimerJob> _logger;

        public GlobalXTransactionsTimerJob(
            IMediator mediator,
            IGlobalXService globalXService,
            ILogger<GlobalXTransactionsTimerJob> logger)
        {
            _mediator = mediator;
            _globalXService = globalXService;
            _logger = logger;
        }

        /// <summary>
        /// GlobalX transaction monitor job. Runs periodically to retrieve new transactions from GlobalX so they can be synced to Actionstep.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(GlobalXTransactionsTimerJob))]
        public async Task Run(
            [TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
            [DurableClient] IDurableOrchestrationClient starter)
        {
            if (starter is null) throw new ArgumentNullException(nameof(starter));

            var allGlobalXSettings = await _mediator.Send(new GlobalXOrgSettingsQuery()
            {
                TransactionSyncEnabled = true
            });

            var allExceptions = new List<Exception>();

            foreach (var globalXSettings in allGlobalXSettings)
            {
                var latestTransactionId = globalXSettings.LatestTransactionId;

                try
                {
                    var validator = new GlobalXOrgSettings.Validator();
                    validator.ValidateAndThrow(globalXSettings);
                }
                catch (ValidationException vex)
                {
                    allExceptions.Add(vex);
                    _logger.LogError(vex, $"Error encountered processing transactions for org key'{globalXSettings?.ActionstepOrgKey}'. Settings are invalid.");
                }

                TransactionsResponse transactionsResponse = null;

                try
                {
                    transactionsResponse = await _globalXService.GetTransactions(new TransactionsQuery()
                    {
                        UserId = globalXSettings.GlobalXAdminId,
                        TransId = globalXSettings.LatestTransactionId + 1,
                        UserType = UserType.AllChildren
                    });
                }
                catch (Exception ex)
                {
                    allExceptions.Add(ex);
                    _logger.LogError(ex, $"Error encountered while retrieving transactions for" +
                        $" org '{globalXSettings?.ActionstepOrgKey}', GlobalX Admin ID: '{globalXSettings?.GlobalXAdminId}'");
                }

                if (!(transactionsResponse is null))
                {
                    foreach (var transaction in transactionsResponse.Transactions)
                    {
                        try
                        {
                            var transactionSyncInstanceId = GlobalXTransactionSyncOrchestrator.InstancePrefix + transaction.TransactionId.ToString(CultureInfo.InvariantCulture);
                            var existingInstance = await starter.GetStatusAsync(transactionSyncInstanceId);
                            if (existingInstance is null)
                            {
                                await starter.StartNewAsync(
                                    orchestratorFunctionName: nameof(GlobalXTransactionSyncOrchestrator),
                                    instanceId: transactionSyncInstanceId,
                                    input: new CreateDisbursementsCommand()
                                    {
                                        ActionstepUserId = globalXSettings.ActionstepSyncUserId,
                                        Transaction = transaction,
                                        ActionstepOrgKey = globalXSettings.ActionstepOrgKey,
                                        MinimumMatterIdToSync = globalXSettings.MinimumMatterIdToSync,
                                        TaxCodeIdWithGST = globalXSettings.TaxCodeIdWithGST.Value,
                                        TaxCodeIdNoGST = globalXSettings.TaxCodeIdNoGST.Value
                                    });
                            }
                            else
                            {
                                _logger.LogWarning($"Orchestration '{nameof(GlobalXTransactionSyncOrchestrator)}' with id '{transactionSyncInstanceId}' is already running, so does not need to be started");
                            }

                            if (transaction.TransactionId > latestTransactionId)
                            {
                                latestTransactionId = transaction.TransactionId;
                            }
                        }
                        catch (Exception ex)
                        {
                            allExceptions.Add(ex);
                            _logger.LogError(ex, $"Error encountered processing transaction '{transaction?.TransactionId}' for" +
                                $" org '{globalXSettings?.ActionstepOrgKey}', GlobalX Admin ID: '{globalXSettings?.GlobalXAdminId}'");
                        }
                    }
                }

                if (latestTransactionId > globalXSettings.LatestTransactionId)
                {
                    await _mediator.Send(new SetLatestGlobalXTransactionIdCommand(globalXSettings.ActionstepOrgKey, latestTransactionId));
                }
            }

            if (allExceptions.Count > 0)
            {
                // If there were any failures, throwing ensures that the TimerJob shows up as failed.
                throw new AggregateException("One or more failures encountered while processing GlobalX transactions.", allExceptions);
            }
        }
    }
}
