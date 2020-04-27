using System;
using System.Runtime.Serialization;

namespace WCA.Core.Features.Actionstep.Connection
{
    [Serializable]
    public class TokenSetUserDoesntMatchActionstepCredentialUserException : Exception
    {
        public string ActionstepCredentialUserId { get; private set; }
        public string TokenSetUserId { get; private set; }
        public string TokenSetId { get; private set; }

        public TokenSetUserDoesntMatchActionstepCredentialUserException()
        {
        }

        public TokenSetUserDoesntMatchActionstepCredentialUserException(string message) : base(message)
        {
        }

        public TokenSetUserDoesntMatchActionstepCredentialUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenSetUserDoesntMatchActionstepCredentialUserException(string actionstepCredentialUserId, string tokenSetUserId, string tokenSetId)
            :base(CreateMessage(actionstepCredentialUserId, tokenSetUserId, tokenSetId))
        {
            ActionstepCredentialUserId = actionstepCredentialUserId;
            TokenSetUserId = tokenSetUserId;
            TokenSetId = tokenSetId;
        }

        private static string CreateMessage(string actionstepCredentialUserId, string tokenSetUserId, string tokenSetId) =>
            $"Could not update ActionstepCredentials properties from TokenSet. The User IDs don't match (ActionstepCredential User ID: '{actionstepCredentialUserId}', TokenSet User ID: '{tokenSetUserId}', TokenSet ID: '{tokenSetId}')";

        protected TokenSetUserDoesntMatchActionstepCredentialUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}