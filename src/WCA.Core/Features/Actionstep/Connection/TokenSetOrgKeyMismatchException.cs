using System;
using System.Runtime.Serialization;
using WCA.Actionstep.Client.Resources;

namespace WCA.Core.Features.Actionstep.Connection
{
    [Serializable]
    public class TokenSetOrgKeyMismatchException : Exception
    {
        public TokenSetOrgKeyMismatchException()
        {
        }

        public TokenSetOrgKeyMismatchException(string message) : base(message)
        {
        }

        public TokenSetOrgKeyMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenSetOrgKeyMismatchException(string message, string storedOrgKey, TokenSet tokenSet)
            : base(CreateDefaultMessage(message, storedOrgKey, tokenSet))
        {
        }

        private static string CreateDefaultMessage(string message, string storedOrgKey, TokenSet tokenSet) =>
            $"{message}" +
            $" (Stored org key: '{storedOrgKey}'" +
            $", TokenSet ID: '{tokenSet?.Id}'" +
            $", TokenSet org key: '{tokenSet?.OrgKey}'" +
            $", User ID: '{tokenSet?.UserId}')";

        protected TokenSetOrgKeyMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}