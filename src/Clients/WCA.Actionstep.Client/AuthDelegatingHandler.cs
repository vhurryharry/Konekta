using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client.Resources;

namespace WCA.Actionstep.Client
{
    /// <summary>
    /// <see cref="AuthDelegatingHandler"/> allows re-sending <see cref="HttpRequestMessage"/> objects as they can only be resubmitted in a <see cref="DelegatingHandler"/>.
    /// 
    /// If a <see cref="HttpRequestMessage"/> is re-used outside of a <see cref="DelegatingHandler"/> then the following exception will be thrown:
    ///     System.InvalidOperationException : The request message was already sent. Cannot send the same request message multiple times.
    /// </summary>
    public class AuthDelegatingHandler : DelegatingHandler
    {
        public AuthDelegatingHandler()
        {
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            if (httpRequestMessage is ActionstepHttpRequestMessage actionstepHttpRequestMessage)
            {
                if (actionstepHttpRequestMessage.ActionstepService is null) throw new ArgumentException("The ActionstepService property must be set.");
                if (actionstepHttpRequestMessage.ActionstepRequest is null) throw new InvalidActionstepRequestException("The ActionstepRequest for this request was null. This shouldn't happen", actionstepHttpRequestMessage);
                if (actionstepHttpRequestMessage.ActionstepRequest.TokenSetQuery is null) throw new InvalidActionstepRequestException("The TokenSet for this request was null. This shouldn't happen", actionstepHttpRequestMessage);

                // First attempt
                var response = await ConfigureAndSendRequest(actionstepHttpRequestMessage, cancellationToken);

                /// NOTE: Disabled to avoid accidental revokation. 
                ///       Tokens are refreshed within access token expiry by a timer job.
                //if (response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    // Force a token refresh in case we've ended up in a situation where the access token is incalid but we still have a valid refresh token.
                //    actionstepHttpRequestMessage.TokenSet = await actionstepHttpRequestMessage.ActionstepService.RefreshAccessTokenIfExpired(
                //        actionstepHttpRequestMessage.TokenSet,
                //        forceRefresh: true);

                //    // And retry the request:
                //    response = await ConfigureAndSendRequest(actionstepHttpRequestMessage, cancellationToken);
                //}

                return response;
            }
            else
            {
                /// For non-<see cref="ActionstepHttpRequestMessage"/> requests we'll just fall through
                return await base.SendAsync(httpRequestMessage, cancellationToken);
            }
        }

        private async Task<HttpResponseMessage> ConfigureAndSendRequest(ActionstepHttpRequestMessage actionstepHttpRequestMessage, CancellationToken cancellationToken)
        {
            if (actionstepHttpRequestMessage is null) throw new ArgumentNullException(nameof(actionstepHttpRequestMessage));

            var actionstepRequest = actionstepHttpRequestMessage.ActionstepRequest;
            if (actionstepRequest is null) throw new InvalidActionstepRequestException("The ActionstepRequest for this request was null. This shouldn't happen", actionstepHttpRequestMessage);
            if (actionstepHttpRequestMessage.TokenSet is null) throw new InvalidActionstepRequestException("The TokenSet for this request was null. This shouldn't happen", actionstepHttpRequestMessage);
            if (actionstepHttpRequestMessage.TokenSet.ApiEndpoint is null) throw new InvalidActionstepRequestException("The Actionstep API endpoint could not be determined for this request.", actionstepHttpRequestMessage);

            // We have to set the URI in case the ApiEndpoint was updated in the TokenSet.
            var apiEndpoint = actionstepHttpRequestMessage.TokenSet.ApiEndpoint;
            actionstepHttpRequestMessage.RequestUri = new Uri(apiEndpoint, Path.Combine(apiEndpoint.AbsolutePath, actionstepRequest.RelativeResourcePath.TrimStart('/')));

            // Set Authorisation as per TokenSet
            actionstepHttpRequestMessage.Headers.Authorization = actionstepHttpRequestMessage.TokenSet.AuthorizationHeaderValue;

            return await base.SendAsync(actionstepHttpRequestMessage, cancellationToken);
        }
    }
}
