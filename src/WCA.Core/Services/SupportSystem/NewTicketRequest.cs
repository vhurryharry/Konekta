namespace WCA.Core.Services.SupportSystem
{
    public class NewTicketRequest
    {
        public string FromEmail { get; set; }
        public string Subject { get; set; }

        public TicketPriority TicketPriority { get; set; }

        /// <summary>
        /// HTML is allowed.
        /// </summary>
        public string Description { get; set; }
    }
}
