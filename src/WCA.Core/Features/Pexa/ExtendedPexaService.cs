
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Features.Conveyancing.WorkspaceCreation;
using WCA.Core.Features.Pexa.Authentication;
using WCA.Core.Services;
using WCA.Domain.Models.Account;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Pexa
{
    public class ExtendedPexaService: IExtendedPexaService
    {
        public ExtendedPexaService(
            HttpClient httpClient, 
            IConfiguration configuration,
            IMediator mediator,
            IClock clock,
            ITelemetryLogger telemetryLogger)
        {
            _pEXAService = new PEXAService(httpClient, configuration);
            _mediator = mediator;
            _clock = clock;
            _telemetryLogger = telemetryLogger;
        }

        private PEXAService _pEXAService { get; }

        private readonly IMediator _mediator;
        private readonly IClock _clock;
        private readonly ITelemetryLogger _telemetryLogger;

        public Uri AuthUrlBase { 
            get
            {
                return _pEXAService.AuthUrlBase;
            }
        }
        public Uri ApiUrlBase { 
            get
            {
                return _pEXAService.ApiUrlBase;
            }
        }

        public PEXAEnvironment PEXAEnvironment { 
            get
            {
                return _pEXAService.PEXAEnvironment;
            }
            set
            {
                _pEXAService.PEXAEnvironment = value;
            }
        }

        public Uri GetWorkspaceUri(string workspaceId, PexaRole workspaceRole)
        {
            return _pEXAService.GetWorkspaceUri(workspaceId, workspaceRole);
        }

        public Uri GetInvitationUri(string workspaceId, PexaRole workspaceRole)
        {
            return _pEXAService.GetInvitationUri(workspaceId, workspaceRole);
        }

        public async Task<TResponse> Handle<TResponse>(PEXARequestBase request, WCAUser user, CancellationToken cancellationToken)
            where TResponse : class
        {
            var pexaApiToken = await _mediator.Send(new PexaApiTokenQuery() { AuthenticatedUser = user });

            if (pexaApiToken != null)
            {
                _telemetryLogger.TrackTrace("Retrieved PEXA API token from credentials store", WCASeverityLevel.Verbose,
                new Dictionary<string, string> {
                    { "accessToken", pexaApiToken.AccessToken },
                    { "accessTokenExpiryUtc", pexaApiToken.AccessTokenExpiryUtc.ToString() },
                });
            }

            if (pexaApiTokenIsValid(pexaApiToken))
            {
                try
                {
                    request.BearerToken = pexaApiToken.AccessToken;
                    return await Handle<TResponse>(request, cancellationToken);
                }
                catch (PEXAException ex)
                {
                    if (pexaApiToken.IsFromCache && (ex.StatusCode == StatusCodes.Status401Unauthorized || ex.StatusCode == StatusCodes.Status403Forbidden))
                    {
                        var pexaTokenFromVault = await _mediator.Send(new PexaApiTokenQuery() { AuthenticatedUser = user, BypassAndUpdateCache = true });

                        if (pexaTokenFromVault.AccessToken != pexaApiToken.AccessToken && pexaApiTokenIsValid(pexaTokenFromVault))
                        {
                            // Access token from vault is different to the cached version, so try with that token before failing
                            request.BearerToken = pexaTokenFromVault.AccessToken;

                            return await Handle<TResponse>(request, cancellationToken);
                        }

                        throw new MissingOrInvalidPexaApiTokenException(user);

                    }
                    else if (ex.StatusCode == StatusCodes.Status400BadRequest)
                    {
                        throw new PexaBadRequestResponseException("Encountered an error during PEXA request", ex);
                    }
                    else
                    {
                        throw new PexaUnexpectedErrorResponseException("Encountered unexpected error during PEXA request", ex);
                    }

                }
            }

            throw new MissingOrInvalidPexaApiTokenException(user);
        }

        private async Task<TResponse> Handle<TResponse>(PEXARequestBase request, CancellationToken cancellationToken)
            where TResponse : class
        {
            return await _pEXAService.Handle<TResponse>(request, cancellationToken);
        }

        private bool pexaApiTokenIsValid(PexaApiToken pexaApiTokenToTest)
        {
            return !(pexaApiTokenToTest == null
                    || string.IsNullOrEmpty(pexaApiTokenToTest.AccessToken)
                    || _clock.GetCurrentInstant() > pexaApiTokenToTest.AccessTokenExpiryUtc);
        }
    }
}
