using System;
using System.Runtime.Serialization;

namespace WCA.Core.Features.GlobalX
{
    [Serializable]
    public class InvalidActionstepMatterException : Exception
    {
        public InvalidActionstepMatterException()
        {
        }

        public InvalidActionstepMatterException(string message) : base(message)
        {
        }

        public InvalidActionstepMatterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidActionstepMatterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}