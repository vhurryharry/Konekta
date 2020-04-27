using FluentValidation;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using WCA.Core.Features.GlobalX.Documents;
using WCA.Core.Features.GlobalX.Settings;
using WCA.Core.Features.GlobalX.Sync;
using WCA.Domain.GlobalX;
using WCA.GlobalX.Client;
using WCA.GlobalX.Client.Documents;

namespace WCA.AzureFunctions.GlobalX.Documents
{
    public class GlobalXDocumentsTimerJob
    {
        private const string _mimeTypePdf = "application/pdf";
        private readonly IMediator _mediator;
        private readonly IGlobalXService _globalXService;
        private readonly IClock _clock;
        private readonly ILogger<GlobalXDocumentsTimerJob> _logger;

        public GlobalXDocumentsTimerJob(
            IMediator mediator,
            IGlobalXService globalXService,
            IClock clock,
            ILogger<GlobalXDocumentsTimerJob> logger)
        {
            _mediator = mediator;
            _globalXService = globalXService;
            _clock = clock;
            _logger = logger;
        }

        /// <summary>
        /// GlobalX document version monitor job. Runs periodically to retrieve new documents from GlobalX so they can be synced to Actionstep.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(GlobalXDocumentsTimerJob))]
        public async Task Run(
            [TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
            [DurableClient] IDurableOrchestrationClient starter)
        {
            if (starter is null) throw new ArgumentNullException(nameof(starter));

            var allGlobalXSettings = await _mediator.Send(new GlobalXOrgSettingsQuery()
            {
                DocumentSyncEnabled = true
            });

            var allExceptions = new List<Exception>();

            foreach (var globalXSettings in allGlobalXSettings)
            {
                var lastDocumentSyncInstant = Instant.FromDateTimeUtc(globalXSettings.LastDocumentSyncUtc);

                try
                {
                    var validator = new GlobalXOrgSettings.Validator();
                    validator.ValidateAndThrow(globalXSettings);
                }
                catch (ValidationException vex)
                {
                    allExceptions.Add(vex);
                    _logger.LogError(vex, $"Error encountered processing documents for org key'{globalXSettings?.ActionstepOrgKey}'. Settings are invalid.");
                }

                try
                {
                    var documentsQuery = new DocumentsRequest()
                    {
                        UserId = globalXSettings.GlobalXAdminId,
                        After = lastDocumentSyncInstant.WithOffset(Offset.Zero),
                        Statuses = { DocumentStatus.Complete }
                    };

                    var thisSyncTime = _clock.GetCurrentInstant();
                    await foreach (var documentWithoutVersions in _globalXService.GetDocuments(documentsQuery))
                    {
                        if (!documentWithoutVersions.DocumentId.HasValue)
                        {
                            _logger.LogError("Error encountered processing document. Response was missing DocumentId for" +
                                    " org '{ActionstepOrgKey}', GlobalX Admin ID: '{GlobalXAdminId}'",
                                    globalXSettings?.ActionstepOrgKey, globalXSettings?.GlobalXAdminId);
                            continue;
                        }

                        Document documentWithVersions;

                        try
                        {
                            documentWithVersions = await _globalXService.GetDocument(documentWithoutVersions.DocumentId.Value, globalXSettings.GlobalXAdminId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex,
                                "Error encountered retrieving document version information for DocumentId '{DocumentId}'" +
                                    " org '{ActionstepOrgKey}', GlobalX Admin ID: '{GlobalXAdminId}'",
                                documentWithoutVersions.DocumentId.Value, globalXSettings?.ActionstepOrgKey, globalXSettings?.GlobalXAdminId);
                            continue;
                        }

                        foreach (var documentVersion in documentWithVersions.DocumentVersions)
                        {
                            try
                            {
                                var documentVersionId = documentVersion.DocumentVersionId.ToString();

                                // Only process "Complete" documents
                                if (documentVersion.StatusDescription != DocumentStatus.Complete.ToString())
                                {
                                    _logger.LogInformation("Skipping document version '{DocumentVersionId}' and statys '{Status}', because it's status is not {RequiredDocumentStatus}.",
                                        documentVersionId, documentVersion.StatusDescription, DocumentStatus.Complete.ToString());
                                    continue;
                                }

                                // Only process PDFs
                                if (documentVersion.MimeType != _mimeTypePdf)
                                {
                                    _logger.LogInformation("Skipping document version '{DocumentVersionId}' and Mime type '{DocumentMimeType}', as it does not have the mime type {RequiredMimeType}.",
                                        documentVersionId, documentVersion.MimeType, _mimeTypePdf);
                                    continue;
                                }

                                // Only process document versions updated since the last sync
                                if (documentVersion.Timestamp.Value.ToInstant() < lastDocumentSyncInstant)
                                {
                                    _logger.LogInformation("Skipping document version '{DocumentVersionId}' with Timestamp '{Timestamp}', because it's timestamp is before the last sync job time of 'LastDocumentSync' which means that this document version should already have been processed.",
                                        documentVersionId, documentVersion.Timestamp.Value, lastDocumentSyncInstant);
                                    continue;
                                }

                                var instanceId = GlobalXDocumentSyncOrchestrator.InstancePrefix + documentVersionId;
                                var existingInstance = await starter.GetStatusAsync(instanceId);
                                if (existingInstance is null)
                                {
                                    _logger.LogDebug("About to start '{OrchestratorName}' for document version '{DocumentVersionId}'.",
                                        nameof(GlobalXDocumentSyncOrchestrator), documentVersionId, documentVersion.Timestamp.Value, lastDocumentSyncInstant);

                                    await starter.StartNewAsync(
                                        orchestratorFunctionName: nameof(GlobalXDocumentSyncOrchestrator),
                                        instanceId: instanceId,
                                        input: new CopyDocumentVersionToActionstepCommand()
                                        {
                                            GlobalXUserId = globalXSettings.GlobalXAdminId,
                                            ActionstepUserId = globalXSettings.ActionstepSyncUserId,
                                            ActionstepOrgKey = globalXSettings.ActionstepOrgKey,
                                            MinimumMatterIdToSync = globalXSettings.MinimumMatterIdToSync,

                                            // Use document without versions because we only care about the single version being
                                            // processed by this orchestrator. Any remaining versions are unnecessary.
                                            Document = documentWithoutVersions,
                                            DocumentVersion = documentVersion
                                        });
                                }
                                else
                                {
                                    _logger.LogInformation("Orchestration '{OrchestratorName}' for document version '{DocumentVersionId}' is already running, so does not need to be started",
                                        nameof(GlobalXDocumentSyncOrchestrator), documentVersionId);
                                }
                            }
                            catch (Exception ex)
                            {
                                allExceptions.Add(ex);
                                _logger.LogError(ex, "Error encountered processing document version '{DocumentVersionId}' for" +
                                    " org '{ActionstepOrgKey}', GlobalX Admin ID: '{GlobalXAdminId}'",
                                    documentVersion?.DocumentVersionId, globalXSettings?.ActionstepOrgKey, globalXSettings?.GlobalXAdminId);
                            }
                        }
                    }

                    await _mediator.Send(new SetLastDocumentSyncTimeCommand(globalXSettings.ActionstepOrgKey, thisSyncTime));
                }
                catch (Exception ex)
                {
                    allExceptions.Add(ex);
                    _logger.LogError(ex, "Error encountered while retrieving document versions for" +
                        " org '{ActionstepOrgKey}', GlobalX Admin ID: '{GlobalXAdminId}'",
                        globalXSettings?.ActionstepOrgKey, globalXSettings?.GlobalXAdminId);
                }
            }

            if (allExceptions.Count > 0)
            {
                // If there were any failures, throwing ensures that the TimerJob shows up as failed.
                throw new AggregateException("One or more failures encountered while processing GlobalX documents.", allExceptions);
            }
        }
    }
}
