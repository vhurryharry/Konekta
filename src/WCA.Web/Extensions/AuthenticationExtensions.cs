using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Core;
using WCA.Web;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// TODO: Move to WCA.Actionstep.AspNetCore project alongside <see cref="WCA.Actionstep.AspNetCore.Authentication.ActionstepJwtExtensions.AddActionstepJwtLogin(AuthenticationBuilder, Action{WCA.Actionstep.AspNetCore.Authentication.ActionstepJwtOptions})"/>.
    /// </summary>
    public static class AuthenticationExtensions
    {
        private const string ActionstepAuthenticationScheme = "Actionstep";
        private const string ActionstepAuthenticationSchemeDisplayName = "Actionstep";

        public static AuthenticationBuilder AddActionstepLogin(this AuthenticationBuilder authenticationBuilder)
        {
            return authenticationBuilder.AddOpenIdConnect(ActionstepAuthenticationScheme, ActionstepAuthenticationSchemeDisplayName, null);
        }

        public static OptionsBuilder<OpenIdConnectOptions> ConfigureActionstepLoginOptions(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

            return serviceCollection.AddOptions<OpenIdConnectOptions>(ActionstepAuthenticationScheme)
                .Configure<IServiceScopeFactory, IOptions<ActionstepSettings>, ILogger<Startup>>((openIdConnectOptions, serviceScopeFactory, actionstepSettingsAccessor, logger) =>
                {
                    if (serviceScopeFactory is null) throw new ArgumentNullException(nameof(serviceScopeFactory));
                    if (openIdConnectOptions is null) throw new ArgumentNullException(nameof(openIdConnectOptions));
                    if (actionstepSettingsAccessor is null) throw new ArgumentNullException(nameof(actionstepSettingsAccessor));

                    // We need a scope because this will execute in the root context and some IActionstepService dependencies are transient/scoped.
                    using var scope = serviceScopeFactory.CreateScope();
                    var provider = scope.ServiceProvider;
                    var actionstepService = provider.GetRequiredService<IActionstepService>();

                    var actionstepSettings = actionstepSettingsAccessor.Value;

                    var actionstepSettingsValidator = new ActionstepSettingsValidatorCollection();
                    var validationResults = actionstepSettingsValidator.Validate(actionstepSettings);
                    if (!validationResults.IsValid)
                    {
                        var errors = string.Join(' ', validationResults.Errors.Select(e => e.ErrorMessage).ToArray());

                        logger.LogError($"There is a problem with the Actionstep configuration. The following error(s) were encountered: {errors}.");
                        return;
                    }

                    var validIssuer = actionstepService.AuthEndpoint.Host;

                    openIdConnectOptions.ResponseMode = OpenIdConnectResponseMode.Query;
                    openIdConnectOptions.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
                    openIdConnectOptions.Authority = actionstepService.AuthEndpoint.AbsoluteUri;
                    openIdConnectOptions.ClientId = actionstepSettings.AuthClientId;
                    openIdConnectOptions.ClientSecret = actionstepSettings.AuthClientSecret;
                    openIdConnectOptions.ResponseType = OpenIdConnectResponseType.Code;
                    openIdConnectOptions.GetClaimsFromUserInfoEndpoint = true;
                    openIdConnectOptions.SaveTokens = true;

                    // We must only use the OpenID Connect scopes for authentication
                    // to ensure that ?prompt=none works correctly. This allows the
                    // user to bypass the login username and password prompt in
                    // Actionstep if they already have an Actionstep session open.
                    // Here we only specify the "email" scope, because "openid" and
                    // "profile" are added automatically by
                    // Microsoft.IdentityModel.Protocols.OpenIdConnect.
                    openIdConnectOptions.Scope.Add("email");

                    openIdConnectOptions.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProvider = context =>
                        {
                            // To eliminate the password prompt when a user already has a session
                            // with Actionstep, we need to supply the "&prompt=none" parameter
                            // to the request to the authorize url.
                            context.ProtocolMessage.SetParameter("prompt", "none");

                            // To eliminate the organisation prompt when the organisation has been
                            // specified in the parameters passed to the page that raised the
                            // authentication challenge (e.g. /wca/infotrack?matterId=7&actionstepOrg=wcamaster),
                            // we pass through the orgkey parameter if a value is available
                            if (context.Properties.Items.ContainsKey("orgkey"))
                            {
                                context.ProtocolMessage.SetParameter("orgkey", context.Properties.Items["orgkey"]);
                            }

                            return Task.FromResult(0);
                        }
                    };

                    openIdConnectOptions.ProtocolValidator.RequireNonce = false;
                    openIdConnectOptions.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(actionstepSettings.AuthClientSecret)
                    );
                    openIdConnectOptions.TokenValidationParameters.ValidIssuer = validIssuer;

                    openIdConnectOptions.Configuration = new OpenIdConnectConfiguration()
                    {
                        AuthorizationEndpoint = actionstepService.AuthorizeUri.AbsoluteUri,
                        TokenEndpoint = actionstepService.TokenUri.AbsoluteUri,
                        EndSessionEndpoint = actionstepService.EndSessionUri.AbsoluteUri,
                        ResponseTypesSupported = { OpenIdConnectResponseType.Code },
                        ResponseModesSupported = { OpenIdConnectResponseMode.Query },
                        GrantTypesSupported = { OpenIdConnectGrantTypes.AuthorizationCode }
                    };

                    openIdConnectOptions.GetClaimsFromUserInfoEndpoint = false;
#pragma warning restore CA1031 // Do not catch general exception types
                });
        }
    }
}
