using System;
using WCA.Domain;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class PexaUnexpectedErrorResponseException : WCAException
    {
        public PexaUnexpectedErrorResponseException(string message) : base(message)
        {
        }

        public PexaUnexpectedErrorResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PexaUnexpectedErrorResponseException()
        {
        }
    }
}
