using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Domain.Models.Account;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Pexa
{
    public interface IExtendedPexaService
    {
        Uri AuthUrlBase { get; }
        Uri ApiUrlBase { get; }

        PEXAEnvironment PEXAEnvironment { get; set; }

        Uri GetWorkspaceUri(string workspaceId, PexaRole workspaceRole);

        Uri GetInvitationUri(string workspaceId, PexaRole workspaceRole);

        Task<TResponse> Handle<TResponse>(PEXARequestBase request, WCAUser user, CancellationToken cancellationToken)
            where TResponse : class;
    }
}
