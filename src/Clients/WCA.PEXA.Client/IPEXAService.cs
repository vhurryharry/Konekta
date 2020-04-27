using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.PEXA.Client.Resources;

namespace WCA.PEXA.Client
{
    public interface IPEXAService
    {
        Uri AuthUrlBase { get; }
        Uri ApiUrlBase { get; }

        PEXAEnvironment PEXAEnvironment { get; set; }

        Uri GetWorkspaceUri(string workspaceId, PexaRole workspaceRole);

        Uri GetInvitationUri(string workspaceId, PexaRole workspaceRole);

        Task<TResponse> Handle<TResponse>(PEXARequestBase request, CancellationToken cancellationToken)
            where TResponse : class;
    }
}
