using System;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client.Authentication
{
    [Serializable]
    public class CannotCreateGlobalXApiTokenLockTableException : Exception
    {
        public CannotCreateGlobalXApiTokenLockTableException()
        {
        }

        public CannotCreateGlobalXApiTokenLockTableException(string message) : base(message)
        {
        }

        public CannotCreateGlobalXApiTokenLockTableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotCreateGlobalXApiTokenLockTableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}