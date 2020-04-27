using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using WCA.AzureFunctions.EmailToSMS.BurstSms;
using WCA.Core;
using WCA.Core.Features.SupportSystem;
using WCA.Core.Services.SupportSystem;

namespace WCA.AzureFunctions.EmailToSMS
{
    public class EmailToSMSTimerJob
    {
        private const string _invalidMessagesFolderName = "Invalid Messages";
        private const string _clientsFolderName = "Clients";
        private const string _fallbackEmailFrom = "sms@konekta.com.au";

        private readonly IMediator _mediator;
        private readonly IOptions<WCACoreSettings> _appSettings;
        private readonly ILogger<EmailToSMSTimerJob> _logger;

        public EmailToSMSTimerJob(
            IMediator mediator,
            IOptions<WCACoreSettings> appSettings,
            ILogger<EmailToSMSTimerJob> logger)
        {
            _mediator = mediator;
            _appSettings = appSettings;
            _logger = logger;
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(EmailToSMSTimerJob))]
        public async Task Run(
#pragma warning disable CA1801 // Remove unused parameter
            [TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo)
#pragma warning restore CA1801 // Remove unused parameter
        {
            if (_appSettings is null ||
                _appSettings.Value is null ||
                _appSettings.Value.EmailToSmsSettings is null ||
                string.IsNullOrEmpty(_appSettings.Value.EmailToSmsSettings.ResellerApiKey) ||
                string.IsNullOrEmpty(_appSettings.Value.EmailToSmsSettings.ResellerApiSecret) ||
                string.IsNullOrEmpty(_appSettings.Value.EmailToSmsSettings.MailboxAddress))
            {
                _logger.LogWarning("Missing Email to SMS configuration. Aborting!");
                return;
            }

            // TODO Move GraphServiceClient and auth to DI
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(_appSettings.Value.EmailToSmsSettings.MicrosoftGraphApiClientId)
                .WithTenantId(_appSettings.Value.EmailToSmsSettings.MicrosoftGraphApiTenantID)
                .WithClientSecret(_appSettings.Value.EmailToSmsSettings.MicrosoftGraphApiSecret)
                .Build();

            var graphServiceClient = new GraphServiceClient(new ClientCredentialProvider(confidentialClientApplication));

            // TODO, make this better or move to config.
            // Hard coding OWNit conveyancing for now.
            var clientIds = new Dictionary<string, int>();
            clientIds.Add("b6rtmjuc", 87627);

            var messageExceptions = new List<Exception>();

            ISMSService smsService = new BurstSMSService(
                _appSettings.Value.EmailToSmsSettings.ResellerApiKey,
                _appSettings.Value.EmailToSmsSettings.ResellerApiSecret);

            var mailboxEmailAddress = _appSettings.Value.EmailToSmsSettings.MailboxAddress;

            var currentMessageRequest = graphServiceClient.Users[mailboxEmailAddress]
                .MailFolders.Inbox
                .Messages
                .Request()
                .Select(m => (new
                {
                    m.Id,
                    m.From,
                    m.Subject,
                    m.Body,
                    m.InternetMessageHeaders,
                    m.ParentFolderId,
                    m.Categories
                }));

            // Get IDs of standard folders to be able to move messages once they're processed
            var inboxFolder = await graphServiceClient.Users[mailboxEmailAddress].MailFolders.Inbox.Request().GetAsync();
            var invalidMessagesFolder = await graphServiceClient.Users[mailboxEmailAddress].EnsureFolder("Inbox", _invalidMessagesFolderName);
            var clientsFolder = await graphServiceClient.Users[mailboxEmailAddress].EnsureFolder("Inbox", _clientsFolderName);
            var clientFolderIds = new Dictionary<string, string>();

            // Loop through pages
            do
            {
                var currentMessages = await currentMessageRequest.GetAsync();

                // Loop through messages in current page
                foreach (var message in currentMessages)
                {
                    var parsedBodyText = string.Empty;

                    try
                    {
                        var messageBodyHtmlDoc = new HtmlAgilityPack.HtmlDocument();
                        messageBodyHtmlDoc.LoadHtml(message.Body.Content);

                        // - Using InnerText strips HTML.
                        // - Then we replace Windows CRLF's in case there are any. We must strip these first because if we
                        //   stripped LF first then we'd have a bunch of CR's floating around all alone.
                        // - Then strip Unix LF's, in case there are any.
                        parsedBodyText = HttpUtility.HtmlDecode(messageBodyHtmlDoc.DocumentNode.InnerText)
                            .Replace("\r\n", "", StringComparison.Ordinal)
                            .Replace("\n", "", StringComparison.Ordinal);

                        var smsEmailJson = JsonConvert.DeserializeObject<SmsEmailJson>(parsedBodyText);

                        // Check for required values
                        if (!smsEmailJson.IsValid(out List<string> missingFields))
                        {
                            // Create a support ticket with the details.
                            var ticketResponse = await _mediator.Send(new CreateTicketCommand()
                            {
                                FromEmail = GetBestEmail(smsEmailJson),
                                TicketPriority = TicketPriority.Low,
                                Subject = GenerateTicketSubject(smsEmailJson),
                                DescriptionHtml = GenerateMessageMissingFields(smsEmailJson, missingFields)
                            });

                            // Store the category for a bad response from the gateway
                            await graphServiceClient.AddMessageCategoriesAsync(mailboxEmailAddress, message, new[] { $"Ticket: {ticketResponse.Id}", EmailCategories.MissingRequiredData });
                            await graphServiceClient.Users[mailboxEmailAddress].Messages[message.Id].Move(invalidMessagesFolder.Id).Request().PostAsync();
                        }
                        else
                        {
                            // Data appears valid, ensure folder exists for client, checking cached IDs first
                            if (smsEmailJson.SmsGo.Value == true)
                            {
                                // Send SMS. Will throw if there's a problem.
                                var clientId = clientIds[smsEmailJson.SmsKey];

                                var cleanMobileNumber = SMSHelper.CleanMobileNumber(smsEmailJson.MobileNumber);

                                if (_appSettings.Value.EmailToSmsSettings.SkipSendingSms)
                                {
                                    _logger.LogWarning(
                                        "Skipping SMS Send: Client ID: {clientId}, Mobile Number: {mobileNumber}, Message: '{message}', Send At: {sendAt}, Replies to Email: {repliesToEmail}",
                                        clientId,
                                        cleanMobileNumber,
                                        smsEmailJson.Message,
                                        smsEmailJson.SendAt,
                                        smsEmailJson.RepliesToEmail);

                                    await graphServiceClient.AddMessageCategoriesAsync(mailboxEmailAddress, message, EmailCategories.SmsSkipped);
                                }
                                else
                                {
                                    var sendAt = SMSHelper.LocalisedSendAt(smsEmailJson.SendAt, smsEmailJson.SendAtTimeZoneID, _logger);

                                    if (sendAt != smsEmailJson.SendAt)
                                    {
                                        _logger.LogInformation(
                                            "SendAt was translated from '{SendAt}' to '{ParsedSendAt}' based on supplied Time Zone ID '{TimeZoneID}'.",
                                            smsEmailJson.SendAt,
                                            sendAt,
                                            smsEmailJson.SendAtTimeZoneID);
                                    }

                                    var sendSmsResponse = await smsService.SendSms(clientId, cleanMobileNumber, smsEmailJson.Message, sendAt, smsEmailJson.RepliesToEmail);

                                    if (sendSmsResponse.Error is null)
                                    {
                                        // Create a support ticket with the details.
                                        var ticketResponse = await _mediator.Send(new CreateTicketCommand()
                                        {
                                            FromEmail = GetBestEmail(smsEmailJson),
                                            TicketPriority = TicketPriority.Low,
                                            Subject = GenerateTicketSubject(smsEmailJson),
                                            DescriptionHtml = GenerateMessageUnknownGatewayResponse(smsEmailJson)
                                        });

                                        await graphServiceClient.AddMessageCategoriesAsync(mailboxEmailAddress, message, $"Ticket: {ticketResponse.Id}", EmailCategories.UnknownError);
                                    }
                                    else if (sendSmsResponse.Error.Code == "SUCCESS")
                                    {
                                        _logger.LogInformation(
                                            "SMS Sent: Cost: {cost}, Sms Message ID: {messageId}, Code: {code}, Descripton: {description}, Send At: {sendAt}",
                                            sendSmsResponse.Cost,
                                            sendSmsResponse.MessageId,
                                            sendSmsResponse.Error?.Code,
                                            sendSmsResponse.Error?.Description,
                                            sendSmsResponse.SendAt);

                                        await graphServiceClient.AddMessageCategoriesAsync(mailboxEmailAddress, message, EmailCategories.SmsSent);
                                    }
                                    else
                                    {
                                        // Create a support ticket with the details.
                                        var ticketResponse = await _mediator.Send(new CreateTicketCommand()
                                        {
                                            FromEmail = GetBestEmail(smsEmailJson),
                                            TicketPriority = TicketPriority.Low,
                                            Subject = GenerateTicketSubject(smsEmailJson),
                                            DescriptionHtml = GenerateMessageGatewayError(message, smsEmailJson, sendSmsResponse)
                                        });

                                        // Store the category for a bad response from the gateway
                                        await graphServiceClient.AddMessageCategoriesAsync(mailboxEmailAddress, message, $"Ticket: {ticketResponse.Id}", EmailCategories.GatewayBadRequest);

                                        _logger.LogError(
                                            "SMS Gateway returned error response. Code: '{code}', Description: '{description}'.",
                                            sendSmsResponse.Error.Code,
                                            sendSmsResponse.Error.Description);
                                    }
                                }
                            }
                            else
                            {
                                await graphServiceClient.AddMessageCategoriesAsync(mailboxEmailAddress, message, EmailCategories.SmsGoFalse);
                            }

                            // Finally, move to client folder
                            string currentClientFolderId;
                            if (clientFolderIds.ContainsKey(smsEmailJson.OrgKey))
                            {
                                currentClientFolderId = clientFolderIds[smsEmailJson.OrgKey];
                            }
                            else
                            {
                                var currentClientFolder = await graphServiceClient.Users[mailboxEmailAddress].EnsureFolder(clientsFolder, smsEmailJson.OrgKey);
                                currentClientFolderId = currentClientFolder.Id;
                            }

                            await graphServiceClient.Users[mailboxEmailAddress].Messages[message.Id].Move(currentClientFolderId).Request().PostAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        messageExceptions.Add(ex);

                        // Just in case something goes wrong, we don't want a failure during cleanup to prevent processing future messages
                        try
                        {
                            if (ex is JsonReaderException jex)
                            {
                                // Create a support ticket with the details.
                                var ticketResponse = await _mediator.Send(new CreateTicketCommand()
                                {
                                    FromEmail = _fallbackEmailFrom,
                                    TicketPriority = TicketPriority.Low,
                                    Subject = $"SMS Failure - couldn't parse JSON",
                                    DescriptionHtml = GenerateMessageJsonParseError(parsedBodyText, jex)
                                });
                                await graphServiceClient.AddMessageCategoriesAsync(mailboxEmailAddress, message, $"Ticket: {ticketResponse.Id}", EmailCategories.InvalidJson);
                            }
                            else
                            {
                                var ticketResponse = await _mediator.Send(new CreateTicketCommand()
                                {
                                    FromEmail = _fallbackEmailFrom,
                                    TicketPriority = TicketPriority.Low,
                                    Subject = $"SMS Failure - Unknown error",
                                    DescriptionHtml = GenerateMessageUnknownException(message, ex)
                                });
                                await graphServiceClient.AddMessageCategoriesAsync(mailboxEmailAddress, message, EmailCategories.UnknownError);
                            }

                            // Move to generic invalid message folder
                            await graphServiceClient.Users[mailboxEmailAddress].Messages[message.Id].Move(invalidMessagesFolder.Id).Request().PostAsync();
                        }
                        catch (Exception cleanupEx)
                        {
                            _logger.LogError(cleanupEx, "Exception encountered during message cleanup!");
                            messageExceptions.Add(cleanupEx);
                        }
                    }
                }

                currentMessageRequest = currentMessages.NextPageRequest;
            } while (currentMessageRequest != null);

            if (messageExceptions.Count > 0)
            {
                throw new AggregateException("Processing of one or more messages has failed", messageExceptions);
            }
        }

        private static string GenerateTicketSubject(SmsEmailJson smsEmailJson)
        {
            return $"SMS Failure for org '{smsEmailJson.OrgKey}', matter '{smsEmailJson.Matter}', template '{smsEmailJson.Template}'";
        }

        private static string GenerateMessageMissingFields(SmsEmailJson smsEmailJson, List<string> missingFields)
        {
            return $@"The following mandatory fields were missing or invalid<br />
<ul>
  <li><strong>{string.Join($"</strong></li><br />{Environment.NewLine}  <li><strong>", missingFields)}</strong></li><br />
</ul>
<br />
<strong>The message details were</strong><br />
<pre>
{JsonConvert.SerializeObject(smsEmailJson, Formatting.Indented)}
</pre>";
        }

        private static string GenerateMessageUnknownGatewayResponse(SmsEmailJson smsEmailJson)
        {
            return $@"The SMS Gateway returned an unknown response. This requires manual investigation.<br />
<br />
<strong>The message details were</strong><br />
<pre>
{JsonConvert.SerializeObject(smsEmailJson, Formatting.Indented)}
</pre>";
        }

        private static string GenerateMessageGatewayError(Message message, SmsEmailJson smsEmailJson, SendSmsResponse sendSmsResponse)
        {
            var isHtml = message?.Body?.ContentType == BodyType.Html;
            return $@"The Konekta SMS gateway returned an error<br />
Gateway Error Code: <strong>{sendSmsResponse?.Error?.Code}</strong><br />
Gateway Error Description: <strong>{sendSmsResponse?.Error?.Description}</strong><br />
<br />
<strong>The message details were</strong><br />
<pre>
{JsonConvert.SerializeObject(smsEmailJson, Formatting.Indented)}
</pre><br />
<br />
<strong>Original Email</strong><br />
<hr>
<strong>From: </strong>&quot;{message.From?.EmailAddress?.Name}&quot; &lt;{message.From?.EmailAddress?.Address}&gt;<br />
<strong>Subject: </strong>{message?.Subject}<br />
{(isHtml ? "<pre>" : "")}
{message?.Body?.Content}
{(isHtml ? "</pre>" : "")}";
        }

        private static string GenerateMessageJsonParseError(string parsedBodyText, JsonReaderException jex)
        {
            return $@"{jex.Message}<br />
<br />
{HtmlHighlightPosition(parsedBodyText, jex.LinePosition)}";
        }

        private static string GenerateMessageUnknownException(Message message, Exception ex)
        {
            var isHtml = message?.Body?.ContentType == BodyType.Html;
            return $@"<strong>Exception Message</strong><br />
{ex?.Message}<br />
<br />
<strong>Original Email</strong><br />
<hr>
<strong>From: </strong>&quot;{message.From?.EmailAddress?.Name}&quot; &lt;{message.From?.EmailAddress?.Address}&gt;<br />
<strong>Subject: </strong>{message?.Subject}<br />
{(isHtml ? "<pre>" : "")}
{message?.Body?.Content}
{(isHtml ? "</pre>" : "")}";
        }

        private static string HtmlHighlightPosition(string parsedBodyText, int v)
        {
            const string indicator = "<span style=\"background-color:red;color:white;\">&lt;--</span>";

            if (parsedBodyText.Length <= v)
            {
                return $"{parsedBodyText}{indicator}";
            }
            else
            {
                return parsedBodyText.Substring(0, v) +
                       indicator +
                       parsedBodyText.Substring(v, parsedBodyText.Length - v);
            }
        }

        private static string GetBestEmail(SmsEmailJson smsEmailJson)
        {
            if (!string.IsNullOrEmpty(smsEmailJson.AssignedToEmail))
                return smsEmailJson.AssignedToEmail;

            if (!string.IsNullOrEmpty(smsEmailJson.RepliesToEmail))
                return smsEmailJson.RepliesToEmail;

            return _fallbackEmailFrom;
        }
    }

    public class SmsEmailJson
    {
        [JsonProperty("smsGo")]
        public bool? SmsGo { get; set; }

        [JsonProperty("matter")]
        public int? Matter { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("automation")]
        public string Automation { get; set; }

        [JsonProperty("orgKey")]
        public string OrgKey { get; set; }

        [JsonProperty("sendAt")]
        public string SendAt { get; set; }

        /// <summary>
        /// A valid Zone ID as per https://nodatime.org/TimeZones
        /// </summary>
        [JsonProperty("sendAtTimeZoneID")]
        public string SendAtTimeZoneID { get; set; }

        [JsonProperty("smsKey")]
        public string SmsKey { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty("assignedToEmail")]
        public string AssignedToEmail { get; set; }

        [JsonProperty("repliesToEmail")]
        public string RepliesToEmail { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public bool IsValid(out List<string> missingFields)
        {
            missingFields = new List<string>();

            if (string.IsNullOrEmpty(OrgKey)) missingFields.Add("orgKey");
            if (string.IsNullOrEmpty(SmsKey)) missingFields.Add("smsKey");
            if (string.IsNullOrEmpty(MobileNumber)) missingFields.Add("mobileNumber");
            if (string.IsNullOrEmpty(Message)) missingFields.Add("message");

            return SmsGo.HasValue && missingFields.Count < 1;
        }
    }
}
