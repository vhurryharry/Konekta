using System.Threading.Tasks;

namespace WCA.Core.Services.SupportSystem
{
    public interface ISupportSystem
    {
        Task<ulong> CreateTicket(NewTicketRequest newTicketRequest);
    }
}
