using System;
using WCA.Domain;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class PexaBadRequestResponseException : WCAException
    {
        public PexaBadRequestResponseException(string message) : base(message)
        {
        }

        public PexaBadRequestResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PexaBadRequestResponseException()
        {
        }
    }
}
