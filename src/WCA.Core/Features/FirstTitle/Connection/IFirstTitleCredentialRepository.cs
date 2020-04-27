using System.Threading.Tasks;
using WCA.Domain.Models.Account;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.FirstTitle.Connection
{
    public interface IFirstTitleCredentialRepository
    {
        Task<FirstTitleCredential> FindCredential(WCAUser authenticatedUser);

        Task SaveOrUpdateCredential(StoreFirstTitleCredentialsCommand storeFirstTitleCredentialsCommand);
    }
}
