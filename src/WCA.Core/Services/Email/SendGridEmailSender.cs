using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WCA.Core.Services.Email
{
    public class SendGridEmailSender : IEmailSender
    {
        private SendGridClient _sendGridClient;
        private string _emailFromAddress;
        private string _emailFromName;
        private bool _testOnly;

        public SendGridEmailSender(IOptions<WCACoreSettings> options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrEmpty(options.Value.SendGridApiKey)) throw new ArgumentException("SendGridApiKey must be set", nameof(options));

            _emailFromAddress = options.Value.EmailFromAddress;
            if (string.IsNullOrEmpty(_emailFromAddress)) throw new ArgumentException("EmailFromAddress must be set", nameof(options));

            _emailFromName = options.Value.EmailFromName;
            if (string.IsNullOrEmpty(_emailFromName)) throw new ArgumentException("EmailFromName must be set", nameof(options));

            _sendGridClient = new SendGridClient(options.Value.SendGridApiKey);

            // Maybe make configurable in the future?
            _testOnly = false;
        }

        public async Task SendEmailAsync(EmailSenderRequest emailSenderRequest)
        {
            if (emailSenderRequest is null) throw new ArgumentNullException(nameof(emailSenderRequest));

            var msg = new SendGridMessage();

            if (_testOnly)
            {
                msg.MailSettings = new MailSettings();
                msg.MailSettings.SandboxMode = new SandboxMode();
                msg.MailSettings.SandboxMode.Enable = true;
            }

            msg.SetFrom(new EmailAddress(_emailFromAddress, _emailFromName));

            // First add all of the "To" email addresses. We must first ensure that no
            // email address appears more than once, otherwise SendGrid will throw an error.
            var uniqueToAddresses = emailSenderRequest.To.GroupBy(emailAddress => emailAddress.Email)
                .Select(thisToEmail =>
                {
                    var firstEmailWithSameAddress = thisToEmail.OrderBy(e => e.Name).First();
                    return new EmailAddress() { Email = firstEmailWithSameAddress.Email, Name = firstEmailWithSameAddress.Name };
                })
                .ToList();
            msg.AddTos(uniqueToAddresses);

            // Now we'll add the "Cc" email addresses. Here we also can't have any duplicates,
            // further to this, we also can't add an address to the "Cc" list if it's already
            // in the list of "To's".
            if (!(emailSenderRequest.Cc is null))
            {
                foreach (var emailAddress in emailSenderRequest.Cc)
                {
                    if (!uniqueToAddresses.Any(uniqueToAddress =>
                        uniqueToAddress.Email.Equals(
                            emailAddress.Email,
                            StringComparison.InvariantCultureIgnoreCase))
                        )
                    {
                        msg.AddCc(new EmailAddress(emailAddress.Email, emailAddress.Name));
                    }
                }
            }

            if (!string.IsNullOrEmpty(emailSenderRequest.Message))
                msg.AddContent(emailSenderRequest.MessageIsHtml ? MimeType.Html : MimeType.Text, emailSenderRequest.Message);

            if (!string.IsNullOrEmpty(emailSenderRequest.Subject))
                msg.SetSubject(emailSenderRequest.Subject);

            if (!string.IsNullOrEmpty(emailSenderRequest.TemplateId))
                msg.SetTemplateId(emailSenderRequest.TemplateId);

            if (emailSenderRequest.Substitutions != null)
                msg.AddSubstitutions(emailSenderRequest.Substitutions);

            var sendGridResponse = await _sendGridClient.SendEmailAsync(msg);

            if ((int)sendGridResponse.StatusCode < 200 || (int)sendGridResponse.StatusCode >= 300)
            {
                throw new FailedToSendEmailException();
            }
        }
    }
}
