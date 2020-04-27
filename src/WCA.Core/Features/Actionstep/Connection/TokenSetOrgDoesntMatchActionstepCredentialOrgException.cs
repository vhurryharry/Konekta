using System;
using System.Runtime.Serialization;

namespace WCA.Core.Features.Actionstep.Connection
{
    [Serializable]
    public class TokenSetOrgDoesntMatchActionstepCredentialOrgException : Exception
    {
        public string ActionstepCredentialOrgKey { get; private set; }
        public string TokenSetOrgKey { get; private set; }
        public string TokenSetId { get; private set; }

        public TokenSetOrgDoesntMatchActionstepCredentialOrgException()
        {
        }

        public TokenSetOrgDoesntMatchActionstepCredentialOrgException(string message) : base(message)
        {
        }

        public TokenSetOrgDoesntMatchActionstepCredentialOrgException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenSetOrgDoesntMatchActionstepCredentialOrgException(string actionstepCredentialOrgKey, string tokenSetOrgKey, string tokenSetId)
            :base(CreateMessage(actionstepCredentialOrgKey, tokenSetOrgKey, tokenSetId))
        {
            ActionstepCredentialOrgKey = actionstepCredentialOrgKey;
            TokenSetOrgKey = tokenSetOrgKey;
            TokenSetId = tokenSetId;
        }

        private static string CreateMessage(string actionstepCredentialOrgKey, string tokenSetOrgKey, string tokenSetId) =>
            $"Could not update ActionstepCredentials properties from TokenSet. The Org Keys don't match (ActionstepCredential OrgKey: '{actionstepCredentialOrgKey}', TokenSet OrgKey: '{tokenSetOrgKey}', TokenSet ID: '{tokenSetId}')";

        protected TokenSetOrgDoesntMatchActionstepCredentialOrgException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}