using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Services;
using WCA.Domain.CQRS;
using WCA.Domain.InfoTrack;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.InfoTrack
{
    public class CheckInfoTrackCredentials
    {
        public class CheckInfoTrackCredentialsCommand : IAuthenticatedCommand<bool>
        {
            /// <summary>
            /// Gets or sets the authenticated user as per IAuthenticatedCommand.
            /// </summary>
            /// <value>
            /// The authenticated user.
            /// </value>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public WCAUser AuthenticatedUser { get; set; }

            /// <summary>
            /// The username to check.
            /// </summary>
            public string InfoTrackUsername { get; set; }

            /// <summary>
            /// The password to check.
            /// </summary>
            public string InfoTrackPassword { get; set; }
        }

        public class Handler : IRequestHandler<CheckInfoTrackCredentialsCommand, bool>
        {
            private readonly ITelemetryLogger telemetryLogger;
            private readonly WCACoreSettings appSettings;

            public Handler(
                ITelemetryLogger telemetryLogger,
                IOptions<WCACoreSettings> appSettingsAccessor)
            {
                this.telemetryLogger = telemetryLogger;
                appSettings = appSettingsAccessor.Value;
            }

            public async Task<bool> Handle(CheckInfoTrackCredentialsCommand message, CancellationToken token)
            {
                var checkId = Guid.NewGuid().ToString();

                telemetryLogger.TrackTrace("Checking InfoTrack credentials", WCASeverityLevel.Information, new Dictionary<string, string>() {
                    { "Authenticated User", message.AuthenticatedUser.Id },
                    { "Check Correlation Id", checkId }
                });

                var client = new HttpClient()
                {
                    BaseAddress = new Uri(appSettings.InfoTrackSettings.BaseApiUrl)
                };

                var byteArray = Encoding.ASCII.GetBytes($"{message.InfoTrackUsername}:{message.InfoTrackPassword}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var dummyContent = new StringContent("{\"ClientReference\":\"123456\",\"RetailerReference\":\"CredentialCheckOnly\"}");
                dummyContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(appSettings.InfoTrackSettings.BaseApiUrl.TrimEnd('/') + "/mapping", dummyContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    telemetryLogger.TrackTrace("InfoTrack credentials are valid", WCASeverityLevel.Information, new Dictionary<string, string>() {
                        { "Authenticated User", message.AuthenticatedUser.Id },
                        { "Check Correlation Id", checkId },
                        { "InfoTrack Code", response.StatusCode.ToString() }
                    });

                    return true;
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        telemetryLogger.TrackTrace("InfoTrack credentials are NOT valid", WCASeverityLevel.Information, new Dictionary<string, string>() {
                            { "Authenticated User", message.AuthenticatedUser.Id },
                            { "Check Correlation Id", checkId },
                            { "InfoTrack Response", responseContent },
                            { "InfoTrack Code", response.StatusCode.ToString() }
                        });

                        return false;
                    }
                    else
                    {
                        telemetryLogger.TrackTrace("Unknown error checking InfoTrack credentials", WCASeverityLevel.Error, new Dictionary<string, string>() {
                            { "Authenticated User", message.AuthenticatedUser.Id },
                            { "Check Correlation Id", checkId },
                            { "InfoTrack Response", responseContent },
                            { "InfoTrack Code", response.StatusCode.ToString() }
                        });

                        throw new InfoTrackRequestFailedException($"{Constants.InfoTrackAPIErrorResponse} " +
                            $"The HTTP response code from InfoTrack was {(int)response.StatusCode}. " +
                            $"InfoTrack gave us this message: '{responseContent}'.");
                    }
                }
            }
        }
    }
}
