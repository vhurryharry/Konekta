using System;
using System.Runtime.Serialization;
using WCA.Domain;

namespace WCA.Core.Features.InfoTrack
{
    [Serializable]
    internal class InvalidInfoTrackOrderUserException : WCAException
    {
        public InvalidInfoTrackOrderUserException()
        {
        }

        public InvalidInfoTrackOrderUserException(string message) : base(message)
        {
        }

        public InvalidInfoTrackOrderUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidInfoTrackOrderUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}