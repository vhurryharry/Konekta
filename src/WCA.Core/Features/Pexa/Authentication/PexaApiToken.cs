using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;

namespace WCA.Core.Features.Pexa.Authentication
{
    public class PexaApiToken
    {
        /// <summary>
        /// Required for JSON deserialization
        /// </summary>
        public PexaApiToken() { }

        private PexaApiToken(JObject response, Instant receivedAt, bool responseIsError)
        {
            ReceivedAt = receivedAt;

            if (responseIsError)
            {
                Error = response.Value<string>("error");
                ErrorDescription = response.Value<string>("error_description");

                var uriValue = response.Value<string>("error_uri");
                if (uriValue != null)
                {
                    try
                    {
                        ErrorUri = new Uri(uriValue);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch { /* Swallow. We don't want this to fail creation of a new PexaApiToken */ }
#pragma warning restore CA1031 // Do not catch general exception types
                }
            }
            else
            {
                AccessToken = response.Value<string>("access_token");
                RefreshToken = response.Value<string>("refresh_token");
                TokenType = response.Value<string>("token_type");
                ExpiresIn = response.Value<int>("expires_in");
            }
        }

        public static PexaApiToken Success(JObject response, Instant receivedAt)
        {
            return new PexaApiToken(response, receivedAt, responseIsError: false);
        }

        public static PexaApiToken Failed(JObject response, Instant receivedAt)
        {
            return new PexaApiToken(response, receivedAt, responseIsError: true);
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the id token
        /// </summary>
        /// <value>
        /// The ID token
        /// </value>
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        /// <summary>
        /// Gets or sets the type of the token. Should always be "bearer"
        /// </summary>
        /// <example>bearer</example>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds that the access token will expire in.
        /// </summary>
        /// <example>3600</example>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the refresh token returned by PEXA. This does not appear to be usable for anything,
        /// as refreshing the access_token by using a refresh_token is unsupported.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        /// <example>invalid_grant</example>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>Gets or sets the error description.</summary>
        /// <value>The error description.</value>
        /// <example>Invalid refresh_token</example>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>Gets or sets the error URI.</summary>
        /// <value>The error URI.</value>
        /// <example>http://tools.ietf.org/html/rfc6749#section-4.1.3</example>
        [JsonProperty("error_uri")]
        public Uri ErrorUri { get; set; }

        /// <summary>
        /// Gets or sets the date time in UTC that the tokens were received.
        /// </summary>
        /// <value>
        /// The received UTC.
        /// </value>
        [JsonProperty("received_at")]
        public Instant ReceivedAt { get; set; }

        [JsonIgnore]
        public bool IsFromCache { get; internal set; }

        [JsonIgnore]
        public Instant AccessTokenExpiryUtc
        {
            get
            {
                return ReceivedAt.Plus(Duration.FromSeconds(ExpiresIn));
            }
        }

        /// <summary>
        /// Checks whether the Access token has expired, based on the clock supplied.
        /// </summary>
        /// <param name="clock"></param>
        /// <returns><see cref="true"/> True if the access token has expired, otherwise <see cref="false"/>. </returns>
        public bool CheckIfAccessTokenHasExpired(IClock clock)
        {
            return clock.GetCurrentInstant() >= AccessTokenExpiryUtc;
        }
    }
}
