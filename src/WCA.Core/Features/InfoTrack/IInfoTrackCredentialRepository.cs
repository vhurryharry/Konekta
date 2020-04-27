using System.Threading.Tasks;
using WCA.Domain.InfoTrack;

namespace WCA.Core.Features.InfoTrack
{
    public interface IInfoTrackCredentialRepository
    {
        Task<InfoTrackCredentials> FindCredential(string actionstepOrgKey);

        Task SaveOrUpdateCredential(string actionstepOrgKey, string username, string password);
    }
}
