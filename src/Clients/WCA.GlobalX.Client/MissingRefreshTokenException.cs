using System;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client
{
    [Serializable]
    public class MissingRefreshTokenException : Exception
    {
        public MissingRefreshTokenException()
        {
        }

        public MissingRefreshTokenException(string message) : base(message)
        {
        }

        public MissingRefreshTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingRefreshTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}