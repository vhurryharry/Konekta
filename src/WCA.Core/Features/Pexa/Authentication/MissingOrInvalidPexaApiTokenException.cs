using System;
using WCA.Domain;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Pexa.Authentication
{
    /// <summary>
    /// Occurs when an API call to Actionstep was attempted with invalid credentials.
    /// Either no suitable ActionstepCredential exists, or it is invalid and cannot be refreshed.
    /// </summary>
    /// <seealso cref="WCA.Core.WCAException" />
    public class MissingOrInvalidPexaApiTokenException : WCAException
    {
        private const string _defaultMessage = "Could not find appropriate PEXA access token to call the PEXA API.";

        public MissingOrInvalidPexaApiTokenException()
            : base(_defaultMessage) { }

        public MissingOrInvalidPexaApiTokenException(WCAUser wCAUser)
        {
            User = wCAUser;
        }

        public MissingOrInvalidPexaApiTokenException(string message)
            : base(message ?? _defaultMessage) { }

        public MissingOrInvalidPexaApiTokenException(Exception innerException)
            : base(_defaultMessage, innerException) { }

        public MissingOrInvalidPexaApiTokenException(string message, Exception innerException)
            : base(message ?? _defaultMessage, innerException)
        { }

        /// <summary>
        /// Gets or sets the user attempting to make the Actionstep API call.
        /// </summary>
        /// <value>
        /// The user attempting to make the Actionstep API call.
        /// </value>
        public WCAUser User { get; set; }
    }
}
