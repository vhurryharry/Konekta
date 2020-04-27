using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;

namespace WCA.GlobalX.Client.Authentication
{
    public class GlobalXApiToken
    {
        /// <summary>
        /// Required for serialization.
        /// </summary>
        public GlobalXApiToken()
        { }

        public GlobalXApiToken(JObject response, Instant receivedAt, string userId = null)
        {
            if (response is null) throw new ArgumentNullException(nameof(response));

            ReceivedAt = receivedAt;

            AccessToken = response.Value<string>("access_token");
            RefreshToken = response.Value<string>("refresh_token");
            TokenType = response.Value<string>("token_type");
            ExpiresIn = response.Value<int>("expires_in");

            UserId = userId;
        }

        public GlobalXApiToken(string accessToken, string tokenType, int expiresIn, string refreshToken, Instant receivedAt, string userId = null)
        {
            if (string.IsNullOrEmpty(accessToken)) { throw new ArgumentException("The parameter was null or empty.", nameof(accessToken)); }
            AccessToken = accessToken;

            if (string.IsNullOrEmpty(tokenType)) { throw new ArgumentException("The parameter was null or empty.", nameof(tokenType)); }
            TokenType = tokenType;

            if (expiresIn < 1) { throw new ArgumentOutOfRangeException(nameof(expiresIn), $"The parameter must be greater than 0"); }
            ExpiresIn = expiresIn;

            if (string.IsNullOrEmpty(refreshToken)) { throw new ArgumentException("The parameter was null or empty.", nameof(refreshToken)); }
            RefreshToken = refreshToken;

            ReceivedAt = receivedAt;
            UserId = userId;
        }

        /// <summary>
        /// The id of the user who these tokens belong to.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

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
        /// Gets or sets the refresh token returned by GlobalX.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the date time in UTC that the tokens were received.
        /// </summary>
        /// <value>
        /// The received UTC.
        /// </value>
        [JsonProperty("received_at")]
        public Instant ReceivedAt { get; set; }

        [JsonIgnore]
        public Instant AccessTokenExpiryUtc
        {
            get
            {
                return ReceivedAt.Plus(Duration.FromSeconds(ExpiresIn));
            }
        }

        /// <summary>
        /// A flag to indicate whether this token is known to be revoked.
        /// Use this to prevents unnecessary usage if a token is known to be bad.
        /// </summary>
        [JsonProperty("revoked_at")]
        public Instant? RevokedAt { get; set; }

        /// <summary>
        /// GlobalX refresh tokens have a 30 day expiry
        /// </summary>
        [JsonIgnore]
        public Instant RefreshTokenExpiryUtc 
        { 
            get 
            { 
                return ReceivedAt.Plus(Duration.FromDays(30)); 
            } 
        }

        /// <summary>
        /// Checks whether the Access token has expired, based on the clock supplied.
        /// </summary>
        /// <param name="clock"></param>
        /// <returns><see cref="true"/> True if the access token has expired, otherwise <see cref="false"/>. </returns>
        public bool AccessTokenHasExpired(IClock clock)
        {
            if (clock is null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            return string.IsNullOrEmpty(AccessToken)
                || RevokedAt.HasValue
                || clock.GetCurrentInstant() >= AccessTokenExpiryUtc;
        }

        /// <summary>
        /// Helper method to check whether refresh token is present and has not expired.
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public bool CheckIfRefreshTokenHasExpired(IClock clock)
        {
            if (clock is null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            return string.IsNullOrEmpty(RefreshToken)
                || RevokedAt.HasValue
                || clock.GetCurrentInstant() >= RefreshTokenExpiryUtc;
        }

        public void MarkRevoked(Instant revokedAt)
        {
            RevokedAt = revokedAt;
        }
    }
}
