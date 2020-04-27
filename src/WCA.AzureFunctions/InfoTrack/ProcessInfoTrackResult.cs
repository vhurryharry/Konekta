using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using System;
using System.Threading.Tasks;
using WCA.Core;
using WCA.Core.Features;
using WCA.Core.Features.InfoTrack;
using WCA.Core.Services.Email;

namespace WCA.AzureFunctions.InfoTrack
{
    public class ProcessInfoTrackResult
    {
        private readonly IMediator _mediator;
        private readonly IOptions<WCACoreSettings> _appSettings;

        public ProcessInfoTrackResult(
            IMediator mediator,
            IOptions<WCACoreSettings> appSettings)
        {
            _mediator = mediator;
            _appSettings = appSettings;
        }

        /// <summary>
        /// Runs the specified infotrack payload.
        /// </summary>
        /// <param name="infotrackPayload">The infotrack payload.</param>
        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName("InfoTrackOrderCallback")]
        [Singleton]
        public async Task Run([QueueTrigger("infotrack-results")] string infotrackPayload)
        {
            try
            {
                await _mediator.Send(JsonConvert.DeserializeObject<UpdateOrder.UpdateOrderCommand>(infotrackPayload));
            }
            catch (Exception ex)
            {
                var subject = $"ProcessInfoTrackResult Error: {ex.Message}";
                while (ex.InnerException != null) ex = ex.InnerException;
                var contentType = MimeType.Text;

                var message = "Queue payload:" + Environment.NewLine +
                    infotrackPayload + Environment.NewLine + Environment.NewLine +
                    "Message    : " + ex.Message + Environment.NewLine +
                    "Stack Trace: " + ex.StackTrace + Environment.NewLine + Environment.NewLine;

                if (ex.Data.Contains("message")) message += ex.Data["message"].ToString();
                if (ex.Data.Contains("contentType")) contentType = ex.Data["contentType"].ToString();

                await _mediator.Send(new SendEmailCommand
                {
                    To = { new EmailRecipient(_appSettings.Value.WCANotificationEmail) },
                    Subject = subject,
                    Message = message,
                    MessageIsHtml = contentType == "text/html"
                });

                // Rethrow to show accurate metrics of failed jobs.
                throw;
            }

        }
    }
}