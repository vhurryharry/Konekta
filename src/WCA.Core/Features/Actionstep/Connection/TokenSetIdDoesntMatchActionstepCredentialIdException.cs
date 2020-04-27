using System;
using System.Runtime.Serialization;

namespace WCA.Core.Features.Actionstep.Connection
{
    [Serializable]
    public class TokenSetIdDoesntMatchActionstepCredentialIdException : Exception
    {
        public int ActionstepCredentialId { get; private set; }
        public string TokenSetId { get; private set; }

        public TokenSetIdDoesntMatchActionstepCredentialIdException()
        {
        }

        public TokenSetIdDoesntMatchActionstepCredentialIdException(string message) : base(message)
        {
        }

        public TokenSetIdDoesntMatchActionstepCredentialIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenSetIdDoesntMatchActionstepCredentialIdException(int actionstepCredentialId, string tokenSetId)
            :base(CreateMessage(actionstepCredentialId, tokenSetId))
        {
            ActionstepCredentialId = actionstepCredentialId;
            TokenSetId = tokenSetId;
        }

        private static string CreateMessage(int actionstepCredentialId, string tokenSetId) =>
            $"Could not update ActionstepCredentials properties from TokenSet. The TokenSet id was not empty and didn't match the id of the ActionstepCredential (ActionstepCredential id: '{actionstepCredentialId}', TokenSet id: '{tokenSetId}')";

        protected TokenSetIdDoesntMatchActionstepCredentialIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}