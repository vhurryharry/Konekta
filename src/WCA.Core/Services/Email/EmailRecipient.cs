namespace WCA.Core.Services.Email
{
    public class EmailRecipient
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public EmailRecipient(string email)
        {
            Email = email;
        }

        public EmailRecipient(string email, string name)
        {
            Email = email;
            Name = name;
        }
    }
}
