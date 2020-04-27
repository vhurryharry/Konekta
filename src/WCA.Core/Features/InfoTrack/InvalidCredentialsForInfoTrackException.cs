using WCA.Domain;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.InfoTrack
{
    /// <summary>
    /// Occurs when the system has attempted to use InfoTrack credentials for
    /// a given org or user, but the request failed due to an unaothorized error.
    /// </summary>
    /// <seealso cref="WCA.Core.WCAException" />
    public class InvalidCredentialsForInfoTrackException : WCAException
    {
        public InvalidCredentialsForInfoTrackException(string actionstepOrgKey, WCAUser user)
        {
            ActionstepOrgKey = actionstepOrgKey;
            User = user;
        }

        /// <summary>
        /// Gets or sets the actionstep org key for which InfoTrack credentials were attempted to be used.
        /// </summary>
        /// <value>
        /// The actionstep org key.
        /// </value>
        public string ActionstepOrgKey { get; private set; }

        /// <summary>
        /// Gets or sets the user who requested the InfoTrack credentials or related operation.
        /// </summary>
        /// <value>
        /// The user who requested the InfoTrack credentials, or use of those credentials.
        /// </value>
        public WCAUser User { get; private set; }
    }
}
