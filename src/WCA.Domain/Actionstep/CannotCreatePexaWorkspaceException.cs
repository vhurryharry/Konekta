using System;

namespace WCA.Domain.Actionstep
{
    public class CannotCreatePexaWorkspaceException : Exception
    {
        public CannotCreatePexaWorkspaceException(string reason)
            : base(reason)
        {
        }

        public CannotCreatePexaWorkspaceException()
        {
        }

        public CannotCreatePexaWorkspaceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
