using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WCA.Actionstep.Client;

namespace WCA.Actionstep.AspNetCore.Authentication
{
    public static class ActionstepJwtExtensions
    {
        public static OptionsBuilder<JwtBearerOptions> ConfigureActionstepJwtOptions(this IServiceCollection serviceCollection)
        {
            return serviceCollection.ConfigureActionstepJwtOptions(null);
        }

        public static OptionsBuilder<JwtBearerOptions> ConfigureActionstepJwtOptions(this IServiceCollection serviceCollection, Action<ActionstepJwtOptions> configureOptions)
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

            if (!(configureOptions is null))
            {
                serviceCollection.Configure(configureOptions);
            }

            return serviceCollection.AddOptions<JwtBearerOptions>(ActionstepJwtDefaults.AuthenticationScheme)
                .Configure<IServiceScopeFactory, IOptionsMonitor<ActionstepJwtOptions>, ILogger<ActionstepJwtOptions>>((jwtBearerOptions, serviceScopeFactory, actionstepJwtOptionsAccessor, logger) =>
                {
                    if (serviceScopeFactory is null) throw new ArgumentNullException(nameof(serviceScopeFactory));

                    var actionstepJwtOptions = actionstepJwtOptionsAccessor is null ? new ActionstepJwtOptions() : actionstepJwtOptionsAccessor.CurrentValue;

                    // We need a scope because this will execute in the root context and some IActionstepService dependencies are transient/scoped.
                    using var scope = serviceScopeFactory.CreateScope();
                    var provider = scope.ServiceProvider;
                    var actionstepService = provider.GetRequiredService<IActionstepService>();

                    var validIssuerRegex = actionstepService.ActionstepEnvironment == ActionstepEnvironment.Production
                        ? new Regex(@"^https:\/\/.*\.actionstep\.com\/$", RegexOptions.Compiled)
                        : new Regex(@"^https:\/\/.*\.actionstepstaging\.com\/$", RegexOptions.Compiled);

                    jwtBearerOptions.TokenValidationParameters.IssuerValidator = (issuer, securityToken, validationParameters) =>
                        {
                            if (validIssuerRegex.IsMatch(issuer))
                            {
                                return issuer;
                            }
                            else
                            {
                                throw new SecurityTokenInvalidIssuerException(
                                    $"Issuer validation failed. Issuer: '{issuer}'. Did not match: {validIssuerRegex.ToString()}.");
                            }
                        };

                    // IssuerSigningKeyResolver is only used if SignatureValidator is null.
                    // See: https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/d895860414398b74727a7ef59c43626d2f51dd5f/src/Microsoft.IdentityModel.JsonWebTokens/JsonWebTokenHandler.cs#L1207
                    jwtBearerOptions.TokenValidationParameters.IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                    {
                        logger.LogDebug("Retrieving Actionstep Public Keys.");
                        return actionstepService.GetPublicKeys();
                    };

                    jwtBearerOptions.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            // Check for one or more "jwt" values in query string, and retrieve the first one if present.
                            if (context.Request.Query.TryGetValue("jwt", out StringValues values))
                            {
                                if (values.Count > 0)
                                {
                                    // Just grab the first one and ignore any others
                                    var token = values[0];

                                    if (!string.IsNullOrWhiteSpace(token))
                                    {
                                        // A "jwt" value was found, so add it ot the context for further processing.
                                        logger.LogDebug("Found JWT in query string, appending it to the context for processing.");
                                        context.Token = token;
                                    }
                                }
                            }

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = (context) =>
                        {
                            logger.LogDebug(context.Exception, "JWT Authentication failed.");
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static AuthenticationBuilder AddActionstepJwt(this AuthenticationBuilder authenticationBuilder)
        {
            return authenticationBuilder.AddJwtBearer(ActionstepJwtDefaults.AuthenticationScheme, ActionstepJwtDefaults.DisplayName, null);
        }
    }
}
