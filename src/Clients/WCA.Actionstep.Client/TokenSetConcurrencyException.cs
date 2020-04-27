using System;
using System.Runtime.Serialization;
using WCA.Actionstep.Client.Resources;

namespace WCA.Actionstep.Client
{
    [Serializable]
    public class TokenSetConcurrencyException : Exception
    {
        public TokenSet TokenSet { get; }

        public TokenSetConcurrencyException()
        {
        }

        public TokenSetConcurrencyException(string message) : base(message)
        {
        }

        public TokenSetConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenSetConcurrencyException(TokenSet tokenSet)
            : this(CreateDefaultMessage(tokenSet))
        {
            TokenSet = tokenSet;
        }

        public TokenSetConcurrencyException(TokenSet tokenSet, Exception innerException)
            : this(CreateDefaultMessage(tokenSet), innerException)
        {
            TokenSet = tokenSet;
        }


        private static string CreateDefaultMessage(TokenSet tokenSet) =>
            $"The TokenSet with ID '{tokenSet?.Id}' for user '{tokenSet?.UserId}' was updated by another process.";

        protected TokenSetConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}