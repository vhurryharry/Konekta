using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Services;
using WCA.Data;
using WCA.Data.Extensions;
using WCA.Domain.InfoTrack;

namespace WCA.Core.Features.InfoTrack
{
    /// <summary>
    /// Save the ordered InfoTrack Info to Actionstep.
    /// </summary>
    public class SaveIncomingInfoTrackOrderUpdate
    {
        public class SaveIncomingInfoTrackOrderUpdateCommand : IRequest
        {
            [JsonProperty("AvailableOnline")]
            public bool AvailableOnline { get; set; }

            [JsonProperty("BillingTypeName")]
            public string BillingTypeName { get; set; }

            [JsonProperty("ClientReference")]
            public string ClientReference { get; set; }

            [JsonProperty("DateOrdered")]
            public DateTime DateOrderedUtc { get; set; }

            [JsonProperty("DateCompleted")]
            public DateTime DateCompletedUtc { get; set; }

            [JsonProperty("Description")]
            public string Description { get; set; }

            [JsonProperty("OrderId")]
            public int OrderId { get; set; }

            [JsonProperty("ParentOrderId")]
            public int ParentOrderId { get; set; }

            [JsonProperty("OrderedBy")]
            public string OrderedBy { get; set; }

            [JsonProperty("Reference")]
            public string Reference { get; set; }

            [JsonProperty("RetailerReference")]
            public string RetailerReference { get; set; }

            [JsonProperty("RetailerFee")]
            public decimal RetailerFee { get; set; }

            [JsonProperty("RetailerFeeGST")]
            public decimal RetailerFeeGST { get; set; }

            [JsonProperty("RetailerFeeTotal")]
            public decimal RetailerFeeTotal { get; set; }

            [JsonProperty("SupplierFee")]
            public decimal SupplierFee { get; set; }

            [JsonProperty("SupplierFeeGST")]
            public decimal SupplierFeeGST { get; set; }

            [JsonProperty("SupplierFeeTotal")]
            public decimal SupplierFeeTotal { get; set; }

            [JsonProperty("TotalFee")]
            public decimal TotalFee { get; set; }

            [JsonProperty("TotalFeeGST")]
            public decimal TotalFeeGST { get; set; }

            [JsonProperty("TotalFeeTotal")]
            public decimal TotalFeeTotal { get; set; }

            [JsonProperty("ServiceName")]
            public string ServiceName { get; set; }

            [JsonProperty("Status")]
            public string Status { get; set; }

            [JsonProperty("StatusMessage")]
            public string StatusMessage { get; set; }

            [JsonProperty("DownloadUrl")]
            public string DownloadUrl { get; set; }

            [JsonProperty("OnlineUrl")]
            public string OnlineUrl { get; set; }

            [JsonProperty("IsBillable")]
            public bool IsBillable { get; set; }

            [JsonProperty("FileHash")]
            public string FileHash { get; set; }

            [JsonProperty("Email")]
            public string Email { get; set; }
        }

        public class Validator : AbstractValidator<SaveIncomingInfoTrackOrderUpdateCommand>
        {
            public Validator()
            {
                RuleFor(c => c.ClientReference).NotEmpty();
                RuleFor(c => c.RetailerReference)
                    .Must(retailerReference => retailerReference.StartsWith("WCA_"))
                    .WithMessage("The Retailer Reference must begin with 'WCA_'.");
            }
        }

        public class Handler : AsyncRequestHandler<SaveIncomingInfoTrackOrderUpdateCommand>
        {
            private readonly Validator validator;
            private readonly IMediator mediator;
            private readonly WCADbContext wCADbContext;
            private readonly IServiceScopeFactory serviceScopeFactory;
            private readonly IMapper mapper;
            private readonly CloudStorageAccount cloudStorageAccount;
            private readonly ITelemetryLogger telemetryLogger;

            public Handler(
                Validator validator,
                IMediator mediator,
                WCADbContext wCADbContext,
                IServiceScopeFactory serviceScopeFactory,
                IMapper mapper,
                CloudStorageAccount cloudStorageAccount,
                ITelemetryLogger telemetryLogger)
            {
                this.validator = validator;
                this.mediator = mediator;
                this.wCADbContext = wCADbContext;
                this.serviceScopeFactory = serviceScopeFactory;
                this.mapper = mapper;
                this.cloudStorageAccount = cloudStorageAccount;
                this.telemetryLogger = telemetryLogger;
            }

            protected override async Task Handle(SaveIncomingInfoTrackOrderUpdateCommand message, CancellationToken token)
            {
                ValidationResult result = validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Unable to save Title Order Info, the command message was invalid.", result.Errors);
                }

                // Always create a new record because we want to save all processed
                // messages and track order statuses over time if required.
                var newInfoTrackOrderUpdateMessage = mapper.Map<InfoTrackOrderUpdateMessage>(message);
                newInfoTrackOrderUpdateMessage.DateCreatedUtc = DateTime.UtcNow;
                newInfoTrackOrderUpdateMessage.LastUpdatedUtc = newInfoTrackOrderUpdateMessage.DateCreatedUtc;
                wCADbContext.Add(newInfoTrackOrderUpdateMessage);
                await wCADbContext.SaveChangesAsync();

                // Only queue for processing if this message is not a duplicate of the last one.
                // First, find the previously received message if there is one
                var previousMessage = wCADbContext.InfoTrackOrderUpdateMessageHistory
                    .AsNoTracking()
                    .OrderByDescending(m => m.Id)
                    .FirstOrDefault(m => m.Id < newInfoTrackOrderUpdateMessage.Id &&
                        m.InfoTrackOrderId == newInfoTrackOrderUpdateMessage.InfoTrackOrderId);

                if (previousMessage != null && newInfoTrackOrderUpdateMessage.IsDuplicateOf(previousMessage))
                {
                    newInfoTrackOrderUpdateMessage.MarkSkippedAsDuplicate();
                    await wCADbContext.SaveChangesAsync();

                    telemetryLogger.TrackTrace(
                        "Received duplicate InfoTrack Order message. It will not be processed further.",
                        WCASeverityLevel.Information,
                        new Dictionary<string, string>()
                        {
                            { "New message order history Id:", newInfoTrackOrderUpdateMessage.Id.ToString() },
                            { "Duplicate order history Id:", previousMessage.Id.ToString() }
                        });

                    return;
                }

                var infoTrackQueue = await cloudStorageAccount
                    .CreateCloudQueueClient()
                    .GetInfoTrackResultsQueue();

                telemetryLogger.TrackTrace(
                    "Received new InfoTrack Order message. It is not a duplciate and will be sent to the queue for processing.",
                    WCASeverityLevel.Verbose,
                    new Dictionary<string, string>()
                    {
                        { "New message order history Id:", newInfoTrackOrderUpdateMessage.Id.ToString() }
                    });

                var newMessage = new CloudQueueMessage(
                    JsonConvert.SerializeObject(
                        new UpdateOrder.UpdateOrderCommand()
                        {
                            InfoTrackOrderUpdateMessageId = newInfoTrackOrderUpdateMessage.Id
                        }));

                // Message lifetime/time to live set to 30 days. If it isn't processed within 30
                // days and nobody has been alerted, then the message can be removed. The item
                // can always be re-added if needed.
                var timeToLive = TimeSpan.FromDays(30);
                await infoTrackQueue.AddMessageAsync(newMessage, timeToLive, null, null, null);
            }
        }
    }
}