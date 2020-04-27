using System;
using System.Net.Http;

namespace WCA.Core.Features.InfoTrack
{
    public class InfoTrackRequestFailedException : HttpRequestException
    {
        public InfoTrackRequestFailedException()
        {
        }

        public InfoTrackRequestFailedException(string message) : base(message)
        {
        }

        public InfoTrackRequestFailedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
