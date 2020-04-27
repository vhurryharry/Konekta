using System;
using WCA.Domain;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.Connection
{
    /// <summary>
    /// Occurs when an API call to Actionstep was attempted with invalid credentials.
    /// Either no suitable ActionstepCredential exists, or it is invalid and cannot be refreshed.
    /// </summary>
    /// <seealso cref="WCA.Core.WCAException" />
    public class InvalidCredentialsForActionstepApiCallException : WCAException
    {
        private const string _defaultMessage = "Invalid Credentials For Actionstep Api Call";

        public InvalidCredentialsForActionstepApiCallException()
            : base(_defaultMessage) { }

        public InvalidCredentialsForActionstepApiCallException(string message)
            : base(message ?? _defaultMessage) { }

        public InvalidCredentialsForActionstepApiCallException(string message, string orgKey)
            : base(message ?? _defaultMessage)
        {
            ActionstepOrgKey = orgKey;
        }

        public InvalidCredentialsForActionstepApiCallException(Exception innerException)
            : base(_defaultMessage, innerException) { }

        public InvalidCredentialsForActionstepApiCallException(string message, Exception innerException)
            : base(message ?? _defaultMessage, innerException)
        { }


        /// <summary>
        /// Gets or sets the actionstep org key that was attempted to be accessed.
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

        /// <summary>
        /// If a substitute user was found, the substituted user
        /// </summary>
        public WCAUser SubstituteUser { get; set; }

    }
}
