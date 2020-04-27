using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Services;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.InfoTrack;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.InfoTrack
{
    public class SendMappingsToInfoTrack
    {
        public class SendMappingsToInfoTrackCommand : IAuthenticatedCommand<InfoTrackMappedDataUrl>
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
            /// The credentials to use when authenticating to InfoTrack.
            /// HTTP Basic authentication will be used.
            /// </summary>
            public InfoTrackCredentials InfoTrackCredentials { get; set; } = new InfoTrackCredentials();

            public InfoTrackMappingData InfoTrackMappingData { get; set; } = new InfoTrackMappingData();
        }

        public class Validator : AbstractValidator<SendMappingsToInfoTrackCommand>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotEmpty();
                RuleFor(c => c.InfoTrackCredentials).NotNull();
                RuleFor(c => c.InfoTrackCredentials.Username).NotEmpty();
                RuleFor(c => c.InfoTrackCredentials.Password).NotEmpty();
                RuleFor(c => c.InfoTrackMappingData).NotNull();
                RuleFor(c => c.InfoTrackMappingData.ClientReference).NotEmpty();
                RuleFor(c => c.InfoTrackMappingData.RetailerReference).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<SendMappingsToInfoTrackCommand, InfoTrackMappedDataUrl>
        {
            private readonly Validator _validator;
            private readonly ITelemetryLogger _telemetryLogger;
            private readonly ILogger<SendMappingsToInfoTrack> _logger;
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly WCACoreSettings _appSettings;

            public Handler(
                Validator validator,
                ITelemetryLogger telemetryLogger,
                ILogger<SendMappingsToInfoTrack> logger,
                IOptions<WCACoreSettings> appSettingsAccessor,
                IHttpClientFactory httpClientFactory)
            {
                _validator = validator;
                _telemetryLogger = telemetryLogger;
                _logger = logger;
                _httpClientFactory = httpClientFactory;
                _appSettings = appSettingsAccessor?.Value;
            }

            public async Task<InfoTrackMappedDataUrl> Handle(SendMappingsToInfoTrackCommand message, CancellationToken token)
            {
                if (message is null)
                {
                    throw new ArgumentNullException(nameof(message));
                }

                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Invalid input.", result.Errors);
                }

                var infoTrackMappingData = JsonConvert.SerializeObject(message.InfoTrackMappingData);

                _telemetryLogger.TrackTrace("Sending Mapped data to InfoTrack", WCASeverityLevel.Information, new Dictionary<string, string>() {
                    { "Client Reference", message.InfoTrackMappingData.ClientReference },
                    { "Retailer Reference", message.InfoTrackMappingData.RetailerReference },
                    { "Payload", infoTrackMappingData }
                });

                using (var Body = new StringContent(infoTrackMappingData))
                {
                    Body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
#pragma warning disable CA2000 // Dispose objects before losing scope - see https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
                    var client = _httpClientFactory.CreateClient();
#pragma warning restore CA2000 // Dispose objects before losing scope
                    client.BaseAddress = new Uri(_appSettings.InfoTrackSettings.BaseApiUrl);

                    var byteArray = Encoding.ASCII.GetBytes($"{message.InfoTrackCredentials.Username}:{message.InfoTrackCredentials.Password}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    if (_logger.IsEnabled(LogLevel.Trace))
                    {
                        _logger.LogTrace("About to send mapping to InfoTrack with payload: '{payload}'", infoTrackMappingData);
                    }

                    var response = await client.PostAsync(_appSettings.InfoTrackSettings.BaseApiUrl.TrimEnd('/') + "/mapping", Body);
                    var infoTrackResponse = await response.Content.ReadAsStringAsync();

                    if (_logger.IsEnabled(LogLevel.Trace))
                    {
                        _logger.LogTrace(
                            "Received response from InfoTrack. Status code: '{code}', response payload: '{payload}'",
                            response.StatusCode,
                            infoTrackResponse);
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            throw new InvalidCredentialsForInfoTrackException(message.InfoTrackCredentials.ActionstepOrgKey, message.AuthenticatedUser);
                        }
                        else
                        {
                            throw new InfoTrackRequestFailedException($"{Constants.InfoTrackAPIErrorResponse} " +
                                $"The HTTP response code from InfoTrack was {(int)response.StatusCode}. " +
                                $"InfoTrack gave us this message: '{infoTrackResponse}'.");
                        }
                    }

                    var infoTrackPostResults = JsonConvert.DeserializeObject<InfoTrackMappedDataUrl>(infoTrackResponse);
                    return infoTrackPostResults;
                }
            }
        }

        public class InfoTrackMappedDataUrl
        {
            public string Token { get; set; }
            public string URL { get; set; }
        }
    }
}
