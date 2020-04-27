using FluentValidation;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;
using WCA.Core.Extensions;
using WCA.Core.Features.GlobalX.Transactions;
using static WCA.Core.Features.GlobalX.Transactions.TransactionDisbursementsCreationError;
using static WCA.Core.Features.GlobalX.ValidateActionstepMatterCommand;

namespace WCA.AzureFunctions.GlobalX.Transactions
{
    public class GlobalXTransactionSyncOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GlobalXTransactionSyncOrchestrator> _logger;

        public static string InstancePrefix { get => "gxt-"; }

        public GlobalXTransactionSyncOrchestrator(
            IMediator mediator,
            ILogger<GlobalXTransactionSyncOrchestrator> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Orchestrates the synchronisation of an individual transaction to Actionstep. Used to ensure that
        /// a disbursement is created successfully for each transaction.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(GlobalXTransactionSyncOrchestrator))]
        public async Task<TransactionDisbursementRelationship> Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            // Get transaction from input
            var createCommand = context.GetInput<CreateDisbursementsCommand>();

            if (createCommand is null) throw new TransactionNotSuppliedException();

            var validator = new CreateDisbursementsCommand.Validator();
            validator.ValidateAndThrow(createCommand);

            // Safety check to make sure that the transaction ID matches the orchestration ID
            var expectedInstanceId = InstancePrefix + createCommand.Transaction.TransactionId.ToString(CultureInfo.InvariantCulture);
            if (context.InstanceId != expectedInstanceId)
            {
                throw new InstanceIdMismatchException(nameof(GlobalXTransactionSyncOrchestrator), context.InstanceId, expectedInstanceId);
            }

            await context.CallActivityAsync(
                nameof(PublishTransactionAdded),
                new TransactionAdded(createCommand.ActionstepOrgKey, createCommand.Transaction));

            // Check the Matter is a valid Actionstep matter. This will loop and wait for user input if an invalid ID is found.
            var validatedActionstepMatterId = await context.ValidateActionstepMatterIdWithRetryAsync(
                createCommand.Transaction?.Matter,
                createCommand.ActionstepOrgKey,
                createCommand.ActionstepUserId,
                createCommand.MinimumMatterIdToSync,
                onInvalidResult: async (callbackContext, matterIdStatus, message) =>
                {
                    context.SetCustomStatus(message);
                    await callbackContext.CallActivityAsync(
                        nameof(PublishTransactionDisbursementsCreationError),
                        new TransactionDisbursementsCreationError(
                            createCommand.Transaction.TransactionId,
                            matterIdStatus switch
                            {
                                MatterIdStatus.InvalidBelowMinimum => TransactionErrorType.MatterIdBelowMinimum,
                                MatterIdStatus.InvalidNotFoundInActionstep => TransactionErrorType.MatterIdNotFoundInActionstep,
                                MatterIdStatus.InvalidUnableToParseAsInt => TransactionErrorType.MatterIdUnableToParseAsInt,
                                _ => TransactionErrorType.UnknownError,
                            },
                            message));
                },
                onMatterIdUpdated: async (callbackContext, oldMatterId, newMatterId) =>
                {
                    context.SetCustomStatus($"Matter ID has been updated to {newMatterId}.");
                    await callbackContext.CallActivityAsync(
                        nameof(PublishTransactionMatterIdUpdated),
                        new TransactionMatterIdUpdated(createCommand.Transaction.TransactionId, createCommand.ActionstepOrgKey, oldMatterId, newMatterId));
                },
                _logger);

            // No further processing if the matter doesn't pass the minimum number filter.
            if (validatedActionstepMatterId is null)
                return null;
            else
                createCommand.ActionstepMatterId = validatedActionstepMatterId.Value;

            // Create disbursement(s) for this transaction
            var transactionDisbursementRelationship = await context.CallActivityWithRetryOnEventAsync<TransactionDisbursementRelationship>(
                functionName: nameof(CreateDisbursementsForTransaction),
                input: createCommand,
                waitForEventName: Events.RetryFailedActivityEvent,
                logger: _logger,
                delayInterval: TimeSpan.FromHours(24),
                maxAutoRetries: 4,
                onException: async (ex, ctx) =>
                {
                    ctx.SetCustomStatus(ex.Message);
                    await ctx.CallActivityAsync(
                        nameof(PublishTransactionDisbursementsCreationError),
                        new TransactionDisbursementsCreationError(createCommand.Transaction.TransactionId, TransactionErrorType.UnknownError, ex.Message));
                });

            await context.CallActivityAsync(
                nameof(PublishTransactionDisbursementsCreated),
                new TransactionDisbursementsCreated(createCommand.Transaction.TransactionId, transactionDisbursementRelationship));

            context.SetCustomStatus(null);

            return transactionDisbursementRelationship;
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(PublishTransactionAdded))]
        public async Task PublishTransactionAdded([ActivityTrigger] TransactionAdded transactionAdded)
        {
            if (!(transactionAdded is null)) { await _mediator.PublishAndLogExceptions(transactionAdded, _logger); }
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(PublishTransactionDisbursementsCreationError))]
        public async Task PublishTransactionDisbursementsCreationError([ActivityTrigger] TransactionDisbursementsCreationError transactionDisbursementsCreationError)
        {
            if (!(transactionDisbursementsCreationError is null)) { await _mediator.PublishAndLogExceptions(transactionDisbursementsCreationError, _logger); }
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(PublishTransactionMatterIdUpdated))]
        public async Task PublishTransactionMatterIdUpdated([ActivityTrigger] TransactionMatterIdUpdated transactionMatterIdUpdated)
        {
            if (!(transactionMatterIdUpdated is null)) { await _mediator.PublishAndLogExceptions(transactionMatterIdUpdated, _logger); }
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(PublishTransactionDisbursementsCreated))]
        public async Task PublishTransactionDisbursementsCreated([ActivityTrigger] TransactionDisbursementsCreated transactionDisbursementsCreated)
        {
            if (!(transactionDisbursementsCreated is null)) { await _mediator.PublishAndLogExceptions(transactionDisbursementsCreated, _logger); }
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(CreateDisbursementsForTransaction))]
        public async Task<TransactionDisbursementRelationship> CreateDisbursementsForTransaction([ActivityTrigger] CreateDisbursementsCommand createDisbursementsCommand)
        {
            if (createDisbursementsCommand is null) throw new ArgumentNullException(nameof(createDisbursementsCommand));
            return await _mediator.Send(createDisbursementsCommand);
        }
    }
}
