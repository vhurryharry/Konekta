using WCA.AzureFunctions.EmailToSMS.BurstSms;
using System.Threading.Tasks;

namespace WCA.AzureFunctions.EmailToSMS
{
    public interface ISMSService
    {
        Task<SendSmsResponse> SendSms(int clientId, string recipientMobileNumber, string message, string sendAt, string repliesToEmail);
    }
}
