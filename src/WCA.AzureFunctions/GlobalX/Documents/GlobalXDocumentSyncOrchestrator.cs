using FluentValidation;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WCA.Core.Extensions;
using WCA.Core.Features.GlobalX;
using WCA.Core.Features.GlobalX.Documents;
using static WCA.Core.Features.GlobalX.ValidateActionstepMatterCommand;

namespace WCA.AzureFunctions.GlobalX.Documents
{
    public class GlobalXDocumentSyncOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GlobalXDocumentSyncOrchestrator> _logger;

        public static string InstancePrefix { get => "gxd-"; }

        public GlobalXDocumentSyncOrchestrator(
            IMediator mediator,
            ILogger<GlobalXDocumentSyncOrchestrator> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Orchestrates the synchronisation of an individual document version to Actionstep. Used to ensure that
        /// each document in GlobalX is successfully uploaded to Actionstep.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(GlobalXDocumentSyncOrchestrator))]
        public async Task<DocumentRelationship> Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            // Get document version metadata from input
            var copyCommand = context.GetInput<CopyDocumentVersionToActionstepCommand>();

            if (copyCommand is null) throw new DocumentNotSuppliedException();

            var validator = new CopyDocumentVersionToActionstepCommand.Validator();
            validator.ValidateAndThrow(copyCommand);

            // Safety check to make sure that the document version ID matches the orchestration ID
            var expectedInstanceId = InstancePrefix + copyCommand.DocumentVersion.DocumentVersionId.Value.ToString();
            if (context.InstanceId != expectedInstanceId)
            {
                throw new InstanceIdMismatchException(nameof(GlobalXDocumentSyncOrchestrator), context.InstanceId, expectedInstanceId);
            }

            await context.CallActivityAsync(
                nameof(PublishDocumentVersionAdded),
                new DocumentVersionAdded(copyCommand.ActionstepOrgKey, copyCommand.Document, copyCommand.DocumentVersion));

            // Check the Matter is a valid Actionstep matter. This will loop and wait for user input if an invalid ID is found.
            var validatedActionstepMatterId = await context.ValidateActionstepMatterIdWithRetryAsync(
                copyCommand.Document?.MatterReference,
                copyCommand.ActionstepOrgKey,
                copyCommand.ActionstepUserId,
                copyCommand.MinimumMatterIdToSync,
                onInvalidResult: async (callbackContext, matterIdStatus, message) =>
                {
                    context.SetCustomStatus(message);
                    await callbackContext.CallActivityAsync(
                        nameof(PublishGlobalXDocumentCopyToActionstepError),
                        new CopyDocumentVersionToActionstepError(
                            copyCommand.DocumentVersion.DocumentVersionId.Value,
                            matterIdStatus switch
                            {
                                MatterIdStatus.InvalidBelowMinimum => CopyErrorType.MatterIdBelowMinimum,
                                MatterIdStatus.InvalidNotFoundInActionstep => CopyErrorType.MatterIdNotFoundInActionstep,
                                MatterIdStatus.InvalidUnableToParseAsInt => CopyErrorType.MatterIdUnableToParseAsInt,
                                _ => CopyErrorType.UnknownError,
                            },
                            message));
                },
                onMatterIdUpdated: async (callbackContext, oldMatterId, newMatterId) =>
                {
                    context.SetCustomStatus($"Matter ID has been updated to {newMatterId}.");
                    await callbackContext.CallActivityAsync(
                        nameof(PublishDocumentVersionMatterIdUpdated),
                        new DocumentVersionMatterIdUpdated(copyCommand.DocumentVersion.DocumentVersionId.Value, copyCommand.ActionstepOrgKey, oldMatterId, newMatterId));
                },
                _logger);

            // No further processing if the matter doesn't pass the minimum number filter.
            if (validatedActionstepMatterId is null)
                return null;
            else
                copyCommand.ActionstepMatterId = validatedActionstepMatterId.Value;

            // Attempt to perform the actual GlobalX download / Actionstep upload
            var documentRelationship = await context.CallActivityWithRetryOnEventAsync<DocumentRelationship>(
                functionName: nameof(CopyDocumentVersionToActionstep),
                input: copyCommand,
                waitForEventName: Events.RetryFailedActivityEvent,
                logger: _logger,
                delayInterval: TimeSpan.FromHours(24),
                maxAutoRetries: 4,
                onException: async (ex, ctx) =>
                {
                    var copyErrorType = ex switch
                    {
                        FailedToDownloadGlobalXDocumentException _ => CopyErrorType.DownloadFromGlobalXError,
                        FailedToUploadGlobalXDocumentToActionstepException _ => CopyErrorType.UploadToActionstepError,
                        InvalidActionstepMatterException _ => CopyErrorType.MatterIdNotFoundInActionstep,
                        _ => CopyErrorType.UnknownError,
                    };

                    ctx.SetCustomStatus(ex.Message);
                    await ctx.CallActivityAsync(
                        nameof(PublishGlobalXDocumentCopyToActionstepError),
                        new CopyDocumentVersionToActionstepError(
                            copyCommand.DocumentVersion.DocumentVersionId.Value,
                            copyErrorType,
                            ex.Message));
                },
                shouldAutoRetry: (ex, ctx) => ex switch
                {
                    InvalidActionstepMatterException _ => false,
                    _ => true,
                });

            await context.CallActivityAsync(
                nameof(PublishGlobalXDocumentCopied),
                        new DocumentVersionCopiedToActionstep(copyCommand.DocumentVersion.DocumentVersionId.Value, documentRelationship));

            context.SetCustomStatus(null);

            return documentRelationship;
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(PublishDocumentVersionAdded))]
        public async Task PublishDocumentVersionAdded([ActivityTrigger] DocumentVersionAdded documentVersionAdded)
        {
            if (!(documentVersionAdded is null)) { await _mediator.PublishAndLogExceptions(documentVersionAdded, _logger); }
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(PublishGlobalXDocumentCopyToActionstepError))]
        public async Task PublishGlobalXDocumentCopyToActionstepError([ActivityTrigger] CopyDocumentVersionToActionstepError copyDocumentVersionToActionstepError)
        {
            if (!(copyDocumentVersionToActionstepError is null)) { await _mediator.PublishAndLogExceptions(copyDocumentVersionToActionstepError, _logger); }
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(PublishDocumentVersionMatterIdUpdated))]
        public async Task PublishDocumentVersionMatterIdUpdated([ActivityTrigger] DocumentVersionMatterIdUpdated documentVersionMatterIdUpdated)
        {
            if (!(documentVersionMatterIdUpdated is null)) { await _mediator.PublishAndLogExceptions(documentVersionMatterIdUpdated, _logger); }
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(PublishGlobalXDocumentCopied))]
        public async Task PublishGlobalXDocumentCopied([ActivityTrigger] DocumentVersionCopiedToActionstep documentVersionCopiedToActionstep)
        {
            if (!(documentVersionCopiedToActionstep is null)) { await _mediator.PublishAndLogExceptions(documentVersionCopiedToActionstep, _logger); }
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(CopyDocumentVersionToActionstep))]
        public async Task<DocumentRelationship> CopyDocumentVersionToActionstep([ActivityTrigger] CopyDocumentVersionToActionstepCommand copyDocumentVersionToActionstepCommand)
        {
            if (copyDocumentVersionToActionstepCommand is null) throw new ArgumentNullException(nameof(copyDocumentVersionToActionstepCommand));
            return await _mediator.Send(copyDocumentVersionToActionstepCommand);
        }
    }
}
