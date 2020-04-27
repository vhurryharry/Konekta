using WCA.Domain;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.Connection
{
    /// <summary>
    /// Occurs when an attempt to use an Actionstep refresh token failed.
    /// </summary>
    /// <seealso cref="WCA.Core.WCAException" />
    public class InvalidActionstepRefreshTokenException : WCAException
    {
        public InvalidActionstepRefreshTokenException(
            string actionstepOrgKey,
            WCAUser user,
            string error,
            string errorDescription,
            string errorUri)
        {
            ActionstepOrgKey = actionstepOrgKey;
            User = user;
            Error = error;
            ErrorDescription = errorDescription;
            ErrorUri = errorUri;
        }

        /// <summary>
        /// Gets or sets the actionstep org key related to the refresh token that was attempted to be used.
        /// </summary>
        /// <value>
        /// The actionstep org key.
        /// </value>
        public string ActionstepOrgKey { get; set; }

        /// <summary>
        /// Gets or sets the user attempting to make the Actionstep API call.
        /// </summary>
        /// <value>
        /// The user attempting to make the Actionstep API call.
        /// </value>
        public WCAUser User { get; set; }

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        /// <example>invalid_grant</example>
        public string Error { get; set; }

        /// <summary>Gets or sets the error description.</summary>
        /// <value>The error description.</value>
        /// <example>Invalid refresh_token</example>
        public string ErrorDescription { get; set; }

        /// <summary>Gets or sets the error URI.</summary>
        /// <value>The error URI.</value>
        /// <example>http://tools.ietf.org/html/rfc6749#section-4.1.3</example>
        public string ErrorUri { get; set; }
    }
}
