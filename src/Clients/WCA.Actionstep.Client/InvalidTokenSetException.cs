using System;
using System.Runtime.Serialization;
using WCA.Actionstep.Client.Resources;

namespace WCA.Actionstep.Client
{
    [Serializable]
    public class InvalidTokenSetException : Exception
    {
        public TokenSet TokenSet { get; private set; }

        public InvalidTokenSetException()
        {
        }

        public InvalidTokenSetException(TokenSet tokenSet)
        {
            TokenSet = tokenSet;
        }

        public InvalidTokenSetException(string message, TokenSet tokenSet) : base(message)
        {
            TokenSet = tokenSet;
        }


        public InvalidTokenSetException(string message) : base(message)
        {
        }

        public InvalidTokenSetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTokenSetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}