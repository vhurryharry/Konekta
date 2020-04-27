using System.Threading.Tasks;

namespace WCA.Core.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailSenderRequest emailSenderRequest);
    }
}
