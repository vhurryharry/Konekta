using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Core;
using WCA.Core.Features.GlobalX.Authentication;
using WCA.Core.Features.Pexa;
using WCA.Core.Features.Pexa.Authentication;
using WCA.Domain.Models.Account;
using WCA.GlobalX.Client;
using WCA.GlobalX.Client.Authentication;
using WCA.Web.Security;
using static WCA.Core.Features.Actionstep.Connection.AddOrUpdateActionstepCredential;

namespace WCA.Web.Features
{
    [Route("integrations")]
    public class IntegrationsController : Controller
    {
        private readonly UserManager<WCAUser> _userManager;
        private readonly WCASignInManager _signInManager;
        private readonly IMediator _mediator;
        private readonly ActionstepSettings _actionstepSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PEXASettings _pexaSettings;
        private readonly GlobalXOptions _globalXOptions;
        private readonly IClock _clock;
        private readonly IExtendedPexaService _pEXAService;
        private readonly IActionstepService _actionstepService;
        private readonly IGlobalXService _globalXService;

#pragma warning disable CA1054 // Uri parameters should not be strings: We don't want Uri throwing exceptions here, we'd rather have invalid returnURls thrown around.
        public IntegrationsController(
            UserManager<WCAUser> userManager,
            IMediator mediator,
            IOptions<WCACoreSettings> settingsAccessor,
            IHttpClientFactory httpClientFactory,
            WCASignInManager signInManager,
            IClock clock,
            IExtendedPexaService pEXAService,
            IActionstepService actionstepService,
            IGlobalXService globalXService)
        {
            if (settingsAccessor is null) throw new ArgumentNullException(nameof(settingsAccessor));

            _userManager = userManager;
            _mediator = mediator;
            _httpClientFactory = httpClientFactory;
            _actionstepSettings = settingsAccessor.Value.ActionstepSettings;
            _pexaSettings = settingsAccessor.Value.PEXASettings;
            _globalXOptions = settingsAccessor.Value.GlobalXOptions;
            _signInManager = signInManager;
            _clock = clock;
            _pEXAService = pEXAService;
            _actionstepService = actionstepService;
            _globalXService = globalXService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("actionstep/connect")]
        public RedirectResult ConnectToActionstep(string returnUrl = null)
        {
            // In this case we want the API scopes for future API requests
            var scopes = WebUtility.UrlEncode(_actionstepSettings.ApiScopes);

            // Similarly, we want an access token that's associated with our "API" ClientID
            // (as opposed to WCA's "Auth" ClientID) because the supplied access token will
            // be used for API calls and not authentication.
            var clientId = _actionstepSettings.ApiClientId;

            // Once the user logs in to Actionstep, they need to be redirected back to the
            // callback action to process the supplied Authorization Code.
            string redirectUri = WebUtility.UrlEncode(CreateActionstepCallbackUrl(returnUrl));

            // Redirecting the user to this URL initiates the OAuth2 "Authorization Code" flow.
            // We need to use the Auth URL to obtain all access tokens from Actionstep
            var actionstepAuthUrl = new StringBuilder();
            actionstepAuthUrl.Append($"{_actionstepService.AuthorizeUri}");
            actionstepAuthUrl.Append($"?response_type=code");
            actionstepAuthUrl.Append($"&scope={scopes}");
            actionstepAuthUrl.Append($"&client_id={clientId}");
            actionstepAuthUrl.Append($"&prompt=none");
            actionstepAuthUrl.Append($"&redirect_uri={redirectUri}");
            return new RedirectResult(actionstepAuthUrl.ToString());
        }

        /// <summary>
        /// Initiate user interactive OAuth2 authorization with PEXA for access token
        /// for use with the PEXA API.
        ///
        /// Not accessible anonymously. User must be logged in to WCA to initiate PEXA
        /// authorization, so that the resulting PEXA token(s) can be associated with a
        /// valid WCA user.
        ///
        /// PEXA OAuth2 will not be used for authentication to WCA.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pexa/connect")]
        public IActionResult ConnectToPexa(string returnUrl = null)
        {
            //// ReturnUrl must never be null/empty, as this will return the user to this
            //// same action, possibly resulting in a redirect loop.
            //returnUrl = returnUrl ?? Url.Page("/wca");
            //var properties = _signInManager.ConfigureExternalAuthenticationProperties(Constants.PexaAuthProvider, returnUrl);
            //return Challenge(properties, Constants.PexaAuthProvider);

            string redirectUri = WebUtility.UrlEncode(CreatePexaCallbackUrl(returnUrl));

            var actionstepAuthUrl = new StringBuilder();
            actionstepAuthUrl.Append($"{_pEXAService.AuthUrlBase.AbsoluteUri}as/authorization.oauth2");
            actionstepAuthUrl.Append($"?response_type=code");
            actionstepAuthUrl.Append($"&client_id={_pexaSettings.ClientId}");
            actionstepAuthUrl.Append($"&redirect_uri={redirectUri}");

            return new RedirectResult(actionstepAuthUrl.ToString());
        }

        [HttpGet]
        [Route("globalx/connect")]
        public IActionResult ConnectToGlobalX(string returnUrl = null)
        {
            string redirectUri = WebUtility.UrlEncode(CreateGlobalXCallbackUrl(returnUrl));

            var globalXAuthUrl = new StringBuilder();
            globalXAuthUrl.Append($"{_globalXService.BaseWebUrl}/auth/connect/authorize");
            globalXAuthUrl.Append($"?response_type=code");
            globalXAuthUrl.Append($"&scope=offline_access");
            globalXAuthUrl.Append($"&client_id={_globalXOptions.ClientId}");
            globalXAuthUrl.Append($"&redirect_uri={redirectUri}");

            return new RedirectResult(globalXAuthUrl.ToString());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/Account/ExternalLoginCallback")]
        public async Task<RedirectResult> ConnectToPexaCallback(string code, string returnUrl = null)
        {
            var (successResult, receivedUtc, payload) = await ExchangeCodeAsync(
                $"{_pEXAService.AuthUrlBase.AbsoluteUri}as/token.oauth2",
                code,
                CreatePexaCallbackUrl(returnUrl),
                _pexaSettings.ClientId,
                _pexaSettings.ClientSecret);

            if (!successResult)
            {
                throw new NotImplementedException("Not sure how to handle invalid response from OAuth2 token endpoint");
            }
            
            // Store token
            await _mediator.Send(new StorePexaApiTokenCommand()
            {
                AuthenticatedUser = await _userManager.GetUserAsync(User),
                PexaApiToken = PexaApiToken.Success(payload, receivedUtc)
            });

            if (string.IsNullOrEmpty(returnUrl))
                return new RedirectResult(Url.Page("/PopupConnectionSuccessful"));
            else
                return new RedirectResult(returnUrl);
        }

        /// <param name="absolutePath">Must include starting slash</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private string CreateCallbackUrl(string absolutePath, string returnUrl)
        {
            var returnUrlComponent = (string.IsNullOrEmpty(returnUrl))
                ? string.Empty
                : $"?returnUrl={WebUtility.UrlEncode(returnUrl)}";

            return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToString()}{absolutePath}{returnUrlComponent}";
        }

        private string CreateActionstepCallbackUrl(string returnUrl)
        {
            return CreateCallbackUrl("/integrations/actionstep/callback", returnUrl);
        }

        private string CreatePexaCallbackUrl(string returnUrl)
        {
            return CreateCallbackUrl("/Account/ExternalLoginCallback", returnUrl);
        }

        private string CreateGlobalXCallbackUrl(string returnUrl)
        {
            return CreateCallbackUrl("/integrations/globalx/callback", returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("actionstep/callback")]
        public async Task<RedirectResult> ConnectToActionstepCallback(string code, string returnUrl = null)
        {
            // We must use the API ClientId and Secret as these credentials will
            // be used to call the Actionstep API.
            var (successResult, receivedAt, tokenResponseJObject) = await ExchangeCodeAsync(
                $"{_actionstepService.TokenUri}",
                code,
                CreateActionstepCallbackUrl(returnUrl),
                _actionstepSettings.ApiClientId,
                _actionstepSettings.ApiClientSecret);

            if (!successResult)
            {
                // TODO
                throw new NotImplementedException("Not sure how to handle invalid response from OAuth2 token endpoint");
            }

            var tokenSet = new TokenSet(tokenResponseJObject, receivedAt);
            if (tokenSet.IdToken == null) throw new Exception("Unreadable API ID token response received from Actionstep.");
            IdentityResult identityResult;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                var claims = tokenSet.IdToken.Claims;
                var email = claims.SingleOrDefault(c => c.Type == "email").Value;
                user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new WCAUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = claims.SingleOrDefault(c => c.Type == "given_name").Value,
                        LastName = claims.SingleOrDefault(c => c.Type == "family_name").Value,
                    };

                    identityResult = await _userManager.CreateAsync(user);
                }

                await _signInManager.SignInAsync(user, false);
            }

            tokenSet.UserId = user.Id;

            await _mediator.Send(new AddOrUpdateActionstepCredentialCommand(tokenSet, user));

            return new RedirectResult(
                string.IsNullOrEmpty(returnUrl)
                ? "/wca/integrations"
                : returnUrl);
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("globalx/callback")]
        public async Task<RedirectResult> ConnectToGlobalXCallback(string code, string returnUrl = null)
        {
            var (successResult, receivedAt, tokenResponseJObject) = await ExchangeCodeAsync(
                $"{_globalXService.BaseWebUrl}/auth/connect/token",
                code,
                CreateGlobalXCallbackUrl(returnUrl),
                _globalXOptions.ClientId,
                _globalXOptions.ClientSecret);

            if (!successResult)
            {
                throw new NotImplementedException("Not sure how to handle invalid response from OAuth2 token endpoint");
            }

            // Store token
            var user = await _userManager.GetUserAsync(User);
            await _mediator.Send(new StoreGlobalXApiTokenCommand
            {
                AuthenticatedUser = user,
                OverrideAndClearLock = true,
                GlobalXApiToken = new GlobalXApiToken(tokenResponseJObject, receivedAt, user.Id)
            });

            if (string.IsNullOrEmpty(returnUrl))
                return new RedirectResult(Url.Page("/PopupConnectionSuccessful"));
            else
                return new RedirectResult(returnUrl);
        }

        /// <summary>
        /// Exchanges the code asynchronously.
        /// </summary>
        /// <param name="tokenEndpoint">The full token endpoint. E.g. https://domain/api/oauth/token</param>
        /// <param name="code">The authorization code as provided in the OAuth 2 flow.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns></returns>
        [NonAction]
        private async Task<(bool successResult, Instant receivedAt, JObject payload)> ExchangeCodeAsync(string tokenEndpoint, string code, string redirectUri, string clientId, string clientSecret)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", clientId },
                { "redirect_uri", redirectUri },
                { "client_secret", clientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint))
            {
#pragma warning disable CA2000 // Dispose objects before losing scope. See https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
                var httpClient = _httpClientFactory.CreateClient();
#pragma warning restore CA2000 // Dispose objects before losing scope
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                requestMessage.Content = requestContent;
                HttpResponseMessage response;
                response = await httpClient.SendAsync(requestMessage, HttpContext.RequestAborted);

                var receivedAt = response.Headers.Date.HasValue
                    ? Instant.FromDateTimeUtc(response.Headers.Date.Value.UtcDateTime)
                    : _clock.GetCurrentInstant();
                return (response.IsSuccessStatusCode, receivedAt, JObject.Parse(await response.Content.ReadAsStringAsync()));
            }
        }
    }
#pragma warning restore CA1054 // Uri parameters should not be strings
}