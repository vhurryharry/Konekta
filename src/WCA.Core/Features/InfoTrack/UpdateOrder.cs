using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Core.Services;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.InfoTrack;

namespace WCA.Core.Features.InfoTrack
{
    /// <summary>
    /// Process an InfoTrackOrder udpate message and update Actionstep.
    ///
    /// This needs to be split in to the following to allow individual operations to be re-performed:
    ///  - Disbursements
    ///  - Actionstep InfoTrack Orders data collection
    ///  - Document upload
    /// </summary>
    public class UpdateOrder
    {
        public class UpdateOrderCommand : ICommand
        {
            public int InfoTrackOrderUpdateMessageId { get; set; }
        }

        public class Handler : AsyncRequestHandler<UpdateOrderCommand>
        {
            private readonly IOptions<WCACoreSettings> _appSettings;
            private readonly ITokenSetRepository _tokenSetRepository;
            private readonly IActionstepService _actionstepService;
            private readonly IMediator _mediator;
            private readonly WCADbContext _wCADbContext;
            private readonly IMapper _mapper;
            private readonly ITelemetryLogger _telemetryLogger;
            private readonly IInfoTrackCredentialRepository _infoTrackCredentialRepository;
            private readonly ILogger _logger;

            public Handler(
                IActionstepService actionstepService,
                IMediator mediator,
                WCADbContext wCADbContext,
                IMapper mapper,
                ITelemetryLogger telemetryLogger,
                IInfoTrackCredentialRepository infoTrackCredentialRepository,
                ILoggerFactory loggerFactory,
                IOptions<WCACoreSettings> appSettings,
                ITokenSetRepository tokenSetRepository)
            {
                _actionstepService = actionstepService;
                _mediator = mediator;
                _wCADbContext = wCADbContext;
                _mapper = mapper;
                _telemetryLogger = telemetryLogger;
                _infoTrackCredentialRepository = infoTrackCredentialRepository;
                _appSettings = appSettings;
                _tokenSetRepository = tokenSetRepository;
                _logger = loggerFactory.CreateLogger<Handler>();
            }

            protected override async Task Handle(UpdateOrderCommand command, CancellationToken token)
            {
                if (command is null) throw new ArgumentNullException(nameof(command));

                var infoTrackOrderUpdateMessage = _wCADbContext.InfoTrackOrderUpdateMessageHistory.Find(command.InfoTrackOrderUpdateMessageId);
                infoTrackOrderUpdateMessage.MarkProcessInProgress();
                await _wCADbContext.SaveChangesAsync();

                var executionId = Guid.NewGuid();
                _telemetryLogger.TrackTrace(
                    $"{typeof(Handler).FullName}: Entered UpdateOrder.",
                    WCASeverityLevel.Information,
                    new Dictionary<string, string>() {
                        { "InfoTrackOrderId", infoTrackOrderUpdateMessage.InfoTrackOrderId.ToString(CultureInfo.InvariantCulture) },
                        { "Actionstep matter", infoTrackOrderUpdateMessage.InfoTrackClientReference },
                        { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                        { "ExecutionId", executionId.ToString() }
                    });

                int matterId = int.Parse(infoTrackOrderUpdateMessage.InfoTrackClientReference, CultureInfo.InvariantCulture);

                string[] retailerReference = infoTrackOrderUpdateMessage.InfoTrackRetailerReference.Split('|');
                var orgKey = string.Empty;
                var wcaUserId = string.Empty;
                if (retailerReference.Length >= 2)
                {
                    // Substring removes the "WCA_" prefix. We know the prefix is present because
                    // the message passed Validation to get here.
                    orgKey = retailerReference[0].Substring(4);
                    wcaUserId = retailerReference[1];
                }

                if (string.IsNullOrEmpty(wcaUserId))
                {
                    throw new InvalidInfoTrackOrderUserException(
                        "InfoTrack order information was supplied without WCA user ID. " +
                        "Cannot process the order.");
                }

                var userWhoPlacedOrder = await _wCADbContext.Users.FindAsync(wcaUserId);
                if (userWhoPlacedOrder == null)
                {
                    throw new InvalidInfoTrackOrderUserException(
                        "InfoTrack order information was supplied with an invalid WCA user ID. " +
                        "Cannot process the order.");
                }

                // Update the current record for this order
                var infoTrackOrder = await _wCADbContext.InfoTrackOrders.FindAsync(infoTrackOrderUpdateMessage.InfoTrackOrderId);

                if (infoTrackOrder == null)
                {
                    infoTrackOrder = new InfoTrackOrder();
                    infoTrackOrder.DateCreatedUtc = DateTime.UtcNow;
                    infoTrackOrder.LastUpdatedUtc = infoTrackOrder.DateCreatedUtc;
                    infoTrackOrder.CreatedBy = userWhoPlacedOrder;
                    infoTrackOrder.UpdatedBy = userWhoPlacedOrder;
                    await _wCADbContext.AddAsync(infoTrackOrder);
                }

                var previousFileHash = infoTrackOrder.InfoTrackFileHash;
                var previousOrderTotalFeeTotal = infoTrackOrder.InfoTrackTotalFeeTotal;

                infoTrackOrder = _mapper.Map(infoTrackOrderUpdateMessage, infoTrackOrder);
                infoTrackOrder.OrderedByWCAUser = userWhoPlacedOrder;
                infoTrackOrder.ActionstepMatterId = matterId;
                infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;

                var existingOrg = await _wCADbContext.ActionstepOrgs.FindAsync(orgKey);
                infoTrackOrder.ActionstepOrg = existingOrg
                    ?? throw new InvalidOperationException(
                        "InfoTrack order information references an invalid Actonstep org key. " +
                        "WCA doesn't have any information or API credentials for the specified " +
                        "org key. Cannot continue.");

                // Update current record with values from history - checkpoint
                await _wCADbContext.SaveChangesAsync();

                var tokenSetQuery = new TokenSetQuery(userWhoPlacedOrder.Id, orgKey);

                var processingErrors = new StringBuilder();

                // Save InfoTrack Title Order Resources (Files) to Actionstep
                // We only want to upload documents if the filehash has changed, and if there's
                // actually a download url.
                if (string.IsNullOrEmpty(infoTrackOrderUpdateMessage.InfoTrackDownloadUrl))
                {
                    _telemetryLogger.TrackTrace(
                        $"{typeof(Handler).FullName}: InfoTrackDownloadUrl is empty. No document to upload.",
                        WCASeverityLevel.Verbose,
                        new Dictionary<string, string>() {
                            { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                            { "Actionstep org key", orgKey },
                            { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                            { "ExecutionId", executionId.ToString() }
                        });

                    infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;
                    infoTrackOrder.ActionstepDocumentUploadStatusUpdatedUtc = infoTrackOrder.LastUpdatedUtc;
                    infoTrackOrder.ActionstepDocumentUploadStatus = ActionstepDocumentUploadStatus.NotApplicable;
                    await _wCADbContext.SaveChangesAsync();
                }
                else if (previousFileHash == infoTrackOrderUpdateMessage.InfoTrackFileHash)
                {
                    _telemetryLogger.TrackTrace(
                        $"{typeof(Handler).FullName}: FileHash has not changed. Skipping document upload.",
                        WCASeverityLevel.Verbose,
                        new Dictionary<string, string>() {
                            { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                            { "Actionstep org key", orgKey },
                            { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                            { "ExecutionId", executionId.ToString() }
                        });

                    // Don't update ActionstepDocumentUploadStatus here because the existing status on the record
                    // is still valid and informative.
                }
                else
                {
                    // If we're here, we have a download url, and the FileHash is new (either becaues it's changed or
                    // there was no prior FileHash).
                    try
                    {
                        infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;
                        infoTrackOrder.ActionstepDocumentUploadStatusUpdatedUtc = infoTrackOrder.LastUpdatedUtc;
                        infoTrackOrder.ActionstepDocumentUploadStatus = ActionstepDocumentUploadStatus.UploadInProgress;
                        await _wCADbContext.SaveChangesAsync();

                        var infoTrackCredentials = await _infoTrackCredentialRepository.FindCredential(orgKey);

                        await _mediator.Send(new SaveResources.SaveResourcesCommand
                        {
                            AuthenticatedUser = userWhoPlacedOrder,
                            MatterId = matterId,
                            ActionstepOrgKey = orgKey,
                            InfoTrackCredentials = infoTrackCredentials,
                            ResourceURL = infoTrackOrderUpdateMessage.InfoTrackDownloadUrl,
                            FolderName = "_SEARCHES",
                            FileNameWithoutExtensionIfNotAvailableFromHeader = infoTrackOrder.InfoTrackServiceName,
                            FileNameAddition = " - Updated at " + DateTime.UtcNow.ToString("dd MMM yyyy hh-mm tt", CultureInfo.InvariantCulture)
                        });

                        infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;
                        infoTrackOrder.ActionstepDocumentUploadStatusUpdatedUtc = infoTrackOrder.LastUpdatedUtc;
                        infoTrackOrder.ActionstepDocumentUploadStatus = ActionstepDocumentUploadStatus.UploadedSuccessfully;
                        await _wCADbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;
                        infoTrackOrder.ActionstepDocumentUploadStatusUpdatedUtc = infoTrackOrder.LastUpdatedUtc;
                        infoTrackOrder.ActionstepDocumentUploadStatus = ActionstepDocumentUploadStatus.UploadFailed;
                        await _wCADbContext.SaveChangesAsync();

                        var message = $"Failed to upload InfoTrack document to Actionstep for ID: {command.InfoTrackOrderUpdateMessageId}";
                        _logger.LogError(ex, message, null);
                        _telemetryLogger.TrackTrace(
                            message,
                            WCASeverityLevel.Error,
                            new Dictionary<string, string>() {
                            { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                            { "Actionstep org key", orgKey },
                            { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                            { "Exception Message", ex.Message },
                            { "Stack Trace", ex.StackTrace.ToString(CultureInfo.InvariantCulture)},
                            { "ExecutionId", executionId.ToString() }
                            });

                        processingErrors.Append(message + "<br/>");
                    }
                }

                if (infoTrackOrder.ActionstepDisbursementStatus == ActionstepDisbursementStatus.CreatedSuccessfully)
                {
                    _telemetryLogger.TrackTrace(
                        $"{typeof(Handler).FullName}: ActionstepDisbursementStatus is CreatedSucessfully, skipping processing of disbursements.",
                        WCASeverityLevel.Information,
                        new Dictionary<string, string>() {
                            { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                            { "Actionstep org key", orgKey },
                            { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                            { "ExecutionId", executionId.ToString() }
                        });
                } else {
                    ValidateFees(infoTrackOrderUpdateMessage);

                    // Negative values are allowed as this represents a refund.
                    if (infoTrackOrderUpdateMessage.InfoTrackTotalFeeTotal != 0)
                    {
                        _telemetryLogger.TrackTrace(
                            $"{typeof(Handler).FullName}: Disbursements have not yet been created and TotalFeeTotal != 0. Processing disbursements now.",
                            WCASeverityLevel.Information,
                            new Dictionary<string, string>() {
                                { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                                { "Actionstep org key", orgKey },
                                { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                                { "InfoTrackTotalFeeTotal", infoTrackOrderUpdateMessage.InfoTrackTotalFeeTotal.ToString(CultureInfo.InvariantCulture) },
                                { "ExecutionId", executionId.ToString() }
                            });

                        try
                        {
                            infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;
                            infoTrackOrder.ActionstepDisbursementStatusUpdatedUtc = infoTrackOrder.LastUpdatedUtc;
                            infoTrackOrder.ActionstepDisbursementStatus = ActionstepDisbursementStatus.CreationInProgress;
                            await _wCADbContext.SaveChangesAsync();

                            (int? gstTaxCodeId, int? nonGstTaxCodeId) = await GetTaxCodeIds(
                                _actionstepService,
                                tokenSetQuery,
                                _telemetryLogger,
                                infoTrackOrderUpdateMessage.InfoTrackOrderId,
                                orgKey,
                                matterId,
                                executionId);

                            // Save under disbursments
                            // If GST values match, we'll create a single disbursement. If they don't match,
                            // we need to create two different disbursements to account for the differing GST amounts.
                            dynamic disbursement;

                            // If they have the same GST status, then we can combine to a single disbursement.
                            // I.e. either if they both have GST, or if neither of them have GST.
                            bool retailerHasGST = infoTrackOrderUpdateMessage.InfoTrackRetailerFeeGST != 0;
                            bool supplierHasGST = infoTrackOrderUpdateMessage.InfoTrackSupplierFeeGST != 0;
                            bool splitDisbursements = false;
                            if (retailerHasGST == supplierHasGST)
                            {
                                disbursement = PrepareSingleDisbursement(infoTrackOrderUpdateMessage, matterId, gstTaxCodeId, nonGstTaxCodeId);
                            }
                            else
                            {
                                splitDisbursements = true;
                                disbursement = PrepareSplitDisbursements(infoTrackOrderUpdateMessage, matterId, gstTaxCodeId, nonGstTaxCodeId);
                            }

                            if (disbursement != null)
                            {
                                _telemetryLogger.TrackTrace(
                                    $"{typeof(Handler).FullName}: Creating disbursements.",
                                    WCASeverityLevel.Information,
                                    new Dictionary<string, string>() {
                                        { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                                        { "Actionstep org key", orgKey },
                                        { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                                        { "InfoTrackTotalFeeTotal", infoTrackOrderUpdateMessage.InfoTrackTotalFeeTotal.ToString(CultureInfo.InvariantCulture) },
                                        { "SplitDisbursements", splitDisbursements.ToString(CultureInfo.InvariantCulture) },
                                        { "ExecutionId", executionId.ToString() }
                                    });

                                var response = await _actionstepService.Handle<dynamic>(new GenericActionstepRequest(
                                    tokenSetQuery,
                                    $"/rest/disbursements",
                                    HttpMethod.Post,
                                    disbursement));

                                infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;
                                infoTrackOrder.ActionstepDisbursementStatusUpdatedUtc = infoTrackOrder.LastUpdatedUtc;
                                infoTrackOrder.ActionstepDisbursementStatus = ActionstepDisbursementStatus.CreatedSuccessfully;
                                await _wCADbContext.SaveChangesAsync();
                            }
                            else
                            {
                                _telemetryLogger.TrackTrace(
                                    $"{typeof(Handler).FullName}: After processing it was determined that no disbursements need to be created.",
                                    WCASeverityLevel.Information,
                                    new Dictionary<string, string>() {
                                        { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                                        { "Actionstep org key", orgKey },
                                        { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                                        { "InfoTrackTotalFeeTotal", infoTrackOrderUpdateMessage.InfoTrackTotalFeeTotal.ToString(CultureInfo.InvariantCulture) },
                                        { "ExecutionId", executionId.ToString() }
                                    });

                                infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;
                                infoTrackOrder.ActionstepDisbursementStatusUpdatedUtc = infoTrackOrder.LastUpdatedUtc;
                                infoTrackOrder.ActionstepDisbursementStatus = ActionstepDisbursementStatus.NotApplicable;
                                await _wCADbContext.SaveChangesAsync();
                            }

                        }
                        catch (Exception ex)
                        {
                            infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;
                            infoTrackOrder.ActionstepDisbursementStatusUpdatedUtc = infoTrackOrder.LastUpdatedUtc;
                            infoTrackOrder.ActionstepDisbursementStatus = ActionstepDisbursementStatus.CreationFailed;
                            await _wCADbContext.SaveChangesAsync();

                            var message = $"Failed to process disbursements for InfoTrack order for ID: {command.InfoTrackOrderUpdateMessageId}";
                            _logger.LogError(ex, message, null);
                            _telemetryLogger.TrackTrace(
                                message,
                                WCASeverityLevel.Error,
                                new Dictionary<string, string>() {
                                    { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                                    { "Actionstep org key", orgKey },
                                    { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                                    { "Exception Message", ex.Message },
                                    { "Stack Trace", ex.StackTrace.ToString(CultureInfo.InvariantCulture)},
                                    { "ExecutionId", executionId.ToString() }
                                });

                            processingErrors.Append(message + "<br/>");
                        }
                    }
                    else
                    {
                        _telemetryLogger.TrackTrace(
                            $"{typeof(Handler).FullName}: Disbursement amounts remain at 0. Skipping disbursements.",
                            WCASeverityLevel.Information,
                            new Dictionary<string, string>() {
                                { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                                { "Actionstep org key", orgKey },
                                { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                                { "InfoTrackTotalFeeTotal", infoTrackOrderUpdateMessage.InfoTrackTotalFeeTotal.ToString(CultureInfo.InvariantCulture) },
                                { "ExecutionId", executionId.ToString() }
                            });

                        infoTrackOrder.LastUpdatedUtc = DateTime.UtcNow;
                        infoTrackOrder.ActionstepDisbursementStatusUpdatedUtc = infoTrackOrder.LastUpdatedUtc;
                        infoTrackOrder.ActionstepDisbursementStatus = ActionstepDisbursementStatus.NotApplicable;
                        await _wCADbContext.SaveChangesAsync();
                    }
                }

                var errorMessage = processingErrors.ToString();
                var processingErrorsEncountered = !String.IsNullOrEmpty(errorMessage);

                if (processingErrorsEncountered)
                {
                    infoTrackOrderUpdateMessage.MarkProcessedWithErrors();
                }
                else
                {
                    infoTrackOrderUpdateMessage.MarkProcessed();
                }

                await _wCADbContext.SaveChangesAsync();

                _telemetryLogger.TrackTrace(
                    processingErrorsEncountered ? $"{typeof(Handler).FullName}: Finished UpdateOrder with errors." : $"{typeof(Handler).FullName}: Finished UpdateOrder.",
                    WCASeverityLevel.Information,
                    new Dictionary<string, string>() {
                        { "InfoTrackOrderUpdateMessageId", command.InfoTrackOrderUpdateMessageId.ToString(CultureInfo.InvariantCulture) },
                        { "Actionstep org key", orgKey },
                        { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                        { "ExecutionId", executionId.ToString() }
                    });
                
                if (processingErrorsEncountered)
                {
                    var ex = new ApplicationException("Errors were encountered while processing message");
                    ex.Data.Add("message", errorMessage);
                    ex.Data.Add("contentType", "text/html");
                    throw ex;
                }

            }

            private void ValidateFees(InfoTrackOrderUpdateMessage infoTrack)
            {
                var errors = new List<string>();
                if (infoTrack.InfoTrackRetailerFee + infoTrack.InfoTrackRetailerFeeGST != infoTrack.InfoTrackRetailerFeeTotal)
                {
                    errors.Add($"Retailer Fee ({infoTrack.InfoTrackRetailerFee}) "
                        + $" + Retailer Fee GST ({infoTrack.InfoTrackRetailerFeeGST}) "
                        + $"is not equal to Retailer Fee Total ({infoTrack.InfoTrackRetailerFeeTotal})");
                }

                if (infoTrack.InfoTrackSupplierFee + infoTrack.InfoTrackSupplierFeeGST != infoTrack.InfoTrackSupplierFeeTotal)
                {
                    errors.Add($"Supplier Fee ({infoTrack.InfoTrackSupplierFee}) "
                        + $" + Supplier Fee GST ({infoTrack.InfoTrackSupplierFeeGST}) "
                        + $"is not equal to Supplier Fee Total ({infoTrack.InfoTrackSupplierFeeTotal})");
                }

                if (infoTrack.InfoTrackTotalFee + infoTrack.InfoTrackTotalFeeGST != infoTrack.InfoTrackTotalFeeTotal)
                {
                    errors.Add($"Total Fee ({infoTrack.InfoTrackTotalFee}) "
                        + $" + Total Fee GST ({infoTrack.InfoTrackTotalFeeGST}) "
                        + $"is not equal to Total Fee Total ({infoTrack.InfoTrackTotalFeeTotal})");
                }

                if (infoTrack.InfoTrackRetailerFeeTotal + infoTrack.InfoTrackSupplierFeeTotal != infoTrack.InfoTrackTotalFeeTotal)
                {
                    errors.Add($"Retailer Fee Total ({infoTrack.InfoTrackRetailerFeeTotal}) "
                        + $" + Supplier Fee Total ({infoTrack.InfoTrackSupplierFeeTotal}) "
                        + $"is not equal to Total Fee Total ({infoTrack.InfoTrackTotalFeeTotal})");
                }

                if (errors.Count > 0)
                {
                    var message = new StringBuilder();
                    message.Append($"There was a problem validating the fees for InfoTrack Order ID {infoTrack.InfoTrackOrderId} <br/>");
                    errors.ForEach(e => message.Append(e + "<br/>"));

                    var ex = new ApplicationException("A problem was encountered validating the InfoTrack fees");
                    ex.Data.Add("message", message.ToString());
                    ex.Data.Add("contentType", "text/html");
                    throw ex;
                }

            }

            private async Task<(int? gstTaxCodeId, int? nonGstTaxCodeId)> GetTaxCodeIds(
                IActionstepService actionstepService,
                TokenSetQuery tokenSetQuery,
                ITelemetryLogger telemetryLogger,
                int infoTrackOrderId,
                string orgKey,
                int matterId,
                Guid executionId)
            {
                // Get GST and Non GST tax codes for disbursement creation

                var taxCodeInfo = await actionstepService.Handle<dynamic>(new GenericActionstepRequest(tokenSetQuery, $"/rest/taxcodes?fields=code", HttpMethod.Get));

                var gstTaxCodeNames = new[] { "S 10.0", "GST" };
                int? gstTaxCodeId = LookupTaxCodeId(taxCodeInfo?.taxcodes, gstTaxCodeNames);

                if (gstTaxCodeId == null)
                {
                    telemetryLogger.TrackTrace(
                        "Couldn't determine GST Tax Code when creating InfoTrack disbursement. " +
                        "Actionstep default will be used instead.",
                        WCASeverityLevel.Warning,
                        new Dictionary<string, string>() {
                            { "GST Tax Code names attempted", string.Join(", ", gstTaxCodeNames) },
                            { "InfoTrack Order ID", infoTrackOrderId.ToString(CultureInfo.InvariantCulture) },
                            { "Actionstep org key", orgKey },
                            { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                            { "ExecutionId", executionId.ToString() }
                        });
                }

                var nonGstTaxCodeNames = new[] { "N-T", "GST Free", "Other GST Free", "No Tax" };

                // TODO: Make this properly configurable! This is just a quick hack :(
                // See:
                //  - PBI:    https://dev.azure.com/workcloudapps/WorkCloudApps/_backlogs/backlog/WorkCloudApps%20Team/Backlog%20items/?workitem=700
                //  - Ticket: https://workcloud.freshdesk.com/a/tickets/10010?note=6238878382
                if (!string.IsNullOrEmpty(orgKey) && orgKey.Equals("swslawyers", StringComparison.InvariantCulture))
                {
                    nonGstTaxCodeNames = new[] { "BAS Excluded", "N-T", "GST Free", "Other GST Free", "No Tax" };
                }

                int? nonGstTaxCodeId = LookupTaxCodeId(taxCodeInfo.taxcodes, nonGstTaxCodeNames);
                if (nonGstTaxCodeId == null)
                {
                    telemetryLogger.TrackTrace(
                        "Couldn't determine Non-GST Tax Code when creating InfoTrack disbursement. " +
                        "Actionstep default will be used instead.",
                        WCASeverityLevel.Warning,
                        new Dictionary<string, string>() {
                            { "Non-GST Tax Code names attempted", string.Join(", ", nonGstTaxCodeNames) },
                            { "InfoTrack Order ID", infoTrackOrderId.ToString(CultureInfo.InvariantCulture) },
                            { "Actionstep org key", orgKey },
                            { "Actionstep matter", matterId.ToString(CultureInfo.InvariantCulture) },
                            { "ExecutionId", executionId.ToString() }
                        });
                }

                return (gstTaxCodeId, nonGstTaxCodeId);
            }

            private static int? LookupTaxCodeId(IEnumerable<dynamic> taxcodes, string[] orderedtaxCodeNames)
            {
                if (taxcodes == null)
                {
                    return null;
                }

                foreach (var taxCodeName in orderedtaxCodeNames)
                {
                    var taxCode = taxcodes?
                        .FirstOrDefault(c => ((string)c.code).Equals(taxCodeName, StringComparison.InvariantCultureIgnoreCase));

                    if (taxCode != null)
                    {
                        return (int?)taxCode.id;
                    }
                }

                return null;
            }

            private static dynamic PrepareSplitDisbursements(InfoTrackOrderUpdateMessage message, int matterId, int? gstTaxCodeId, int? nonGstTaxCodeId)
            {
                var disbursements = new List<dynamic>();
                string cleanInfoTrackDescription = GetCleanInfoTrackDescription(message);

                if (message.InfoTrackSupplierFeeTotal != 0)
                {
                    disbursements.Add(
                        new
                        {
                            description = $"InfoTrack order {message.InfoTrackOrderId}{cleanInfoTrackDescription} (supplier fee)",
                            quantity = 1,
                            unitPrice = message.InfoTrackSupplierFeeTotal,
                            unitPriceIncludesTax = message.InfoTrackSupplierFeeGST != 0 ? "T" : "F",
                            links = new
                            {
                                action = matterId,
                                taxCode = message.InfoTrackSupplierFeeGST != 0 ? gstTaxCodeId : nonGstTaxCodeId
                            }
                        });
                }

                if (message.InfoTrackRetailerFeeTotal != 0)
                {
                    disbursements.Add(
                        new
                        {
                            description = $"InfoTrack order {message.InfoTrackOrderId}{cleanInfoTrackDescription} (retailer fee)",
                            quantity = 1,
                            unitPrice = message.InfoTrackRetailerFeeTotal,
                            unitPriceIncludesTax = message.InfoTrackRetailerFeeGST != 0 ? "T" : "F",
                            links = new
                            {
                                action = matterId,
                                taxCode = message.InfoTrackRetailerFeeGST != 0 ? gstTaxCodeId : nonGstTaxCodeId
                            }
                        });
                }

                if (disbursements.Count < 1)
                {
                    return null;
                }

                return new
                {
                    disbursements = disbursements.ToArray()
                };
            }

            /// <summary>
            /// Helper to clean up the InfoTrack description. Because we prepend this with our own descriptor,
            /// this avoids "InfoTrack" appearing twice as sometimes the description contains "InfoTrack" in it as well.
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            private static string GetCleanInfoTrackDescription(InfoTrackOrderUpdateMessage message)
            {
                var cleanInfoTrackDescription = message.InfoTrackDescription?.Trim() ?? string.Empty;
                var stripPrefix = "InfoTrack: ";
                if (cleanInfoTrackDescription.StartsWith(stripPrefix, StringComparison.InvariantCulture))
                {
                    cleanInfoTrackDescription = cleanInfoTrackDescription.Substring(stripPrefix.Length);
                }

                if (cleanInfoTrackDescription.Length > 0)
                {
                    cleanInfoTrackDescription = ": " + cleanInfoTrackDescription;
                }

                return cleanInfoTrackDescription;
            }

            private static dynamic PrepareSingleDisbursement(InfoTrackOrderUpdateMessage message, int matterId, int? gstTaxCodeId, int? nonGstTaxCodeId)
            {
                return new
                {
                    disbursements = new dynamic[] {
                        new
                        {
                            description = $"InfoTrack order {message.InfoTrackOrderId}{GetCleanInfoTrackDescription(message)}",
                            quantity = 1,
                            unitPrice = message.InfoTrackTotalFeeTotal,
                            unitPriceIncludesTax = message.InfoTrackTotalFeeGST != 0 ? "T" : "F",
                            links = new
                            {
                                action = matterId,
                                taxCode = message.InfoTrackTotalFeeGST != 0 ? gstTaxCodeId : nonGstTaxCodeId
                            }
                        }
                    }
                };
            }
        }
    }
}
