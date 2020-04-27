using System;
using WCA.Domain;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.FirstTitle.Connection
{
    public class MissingFirstTitleCredentialsException: WCAException
    {
        private const string _defaultMessage = "Could not find appropriate First Title credentials to call the First Title API.";

        public MissingFirstTitleCredentialsException()
            : base(_defaultMessage) { }

        public MissingFirstTitleCredentialsException(WCAUser wCAUser)
        {
            User = wCAUser;
        }

        public MissingFirstTitleCredentialsException(string message)
            : base(message ?? _defaultMessage) { }

        public MissingFirstTitleCredentialsException(Exception innerException)
            : base(_defaultMessage, innerException) { }

        public MissingFirstTitleCredentialsException(string message, Exception innerException)
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
