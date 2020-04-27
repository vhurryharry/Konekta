
using System.Threading.Tasks;

namespace WCA.GlobalX.Client.Authentication
{
    /// <summary>
    /// Stores GlobalX Credentials (username/password pair).
    /// </summary>
    public interface IGlobalXCredentialsRepository
    {
        /// <summary>
        /// Update a <see cref="GlobalXCredentials"/> based on its identifier.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        Task AddOrUpdateGlobalXCredentials(GlobalXCredentials credentials);

        /// <summary>
        /// Retrieve <see cref="GlobalXCredentials"/> stored for a given user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<GlobalXCredentials> GetCredentialsForUser(string userId);
    }
}
