using System;

namespace WCA.Domain.Actionstep
{
    public class PexaWorkspaceAlreadyExistsException: CannotCreatePexaWorkspaceException
    {
        public string WorkspaceId { get; set; } = null;

        public PexaWorkspaceAlreadyExistsException(string reason)
            : base(reason)
        {
        }

        public PexaWorkspaceAlreadyExistsException(string workspaceId, string reason)
            : base(reason)
        {
            WorkspaceId = workspaceId;
        }

        public PexaWorkspaceAlreadyExistsException()
        {
        }

        public PexaWorkspaceAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
