using System.Collections.Generic;

namespace WCA.Core.Services.Email
{
    public class EmailSenderRequest
    {
        public List<EmailRecipient> To { get; } = new List<EmailRecipient>();
        public List<EmailRecipient> Cc { get; } = new List<EmailRecipient>();
        public string Subject { get; set; }
        public string Message { get; set; }
        public string TemplateId { get; set; }
        public bool MessageIsHtml { get; set; } = true;
        public Dictionary<string, string> Substitutions { get; } = new Dictionary<string, string>();
    }
}
