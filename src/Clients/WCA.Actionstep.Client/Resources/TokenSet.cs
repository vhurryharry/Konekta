using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace WCA.Actionstep.Client.Resources
{
    public class TokenSet
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenResponse"></param>
        /// <param name="userId">Not required. The id of the user who these credentials belog to. For example, if a user is logging in for the first time they may not yet have an id.</param>
        /// <param name="id">Not required. The unique id of these credentials (for storage). For example, new tokens that haven't yet been persisted may not have an id.</param>
        /// <param name="receivedAt"></param>
        public TokenSet(JObject tokenResponse, Instant receivedAt, string userId = null, string id = null)
        {
            try
            {
                string missingOrEmptyMessage(string param) => $"The '{param}' property was missing or empty in the token response.";

                if (tokenResponse is null) { throw new InvalidTokenResponseException("Cannot determine Actionstep API Credentials. The token response is null."); }
                if (!tokenResponse.HasValues) { throw new InvalidTokenResponseException("Cannot determine Actionstep API Credentials. The token response is empty."); }

                Response = tokenResponse;
                ReceivedAt = receivedAt;
                UserId = userId;
                Id = id;

                AccessToken = tokenResponse.Value<string>("access_token");
                if (string.IsNullOrEmpty(AccessToken)) { throw new InvalidTokenResponseException(missingOrEmptyMessage("access_token")); }

                var rawIdToken = tokenResponse.Value<string>("id_token");

                // Check for token before parsing. That way, IdToken remains null if there is no id_token in the response.
                // This is okay, because the presence of an id_token depends on the openid scope.
                if (!string.IsNullOrEmpty(rawIdToken))
                {
                    try
                    {
                        IdToken = new JwtSecurityToken(rawIdToken);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidTokenResponseException(
                            $"The 'id_token' property could not be parsed as a valid JWT token. The value received was '{rawIdToken}'.",
                            ex);
                    }
                }

                TokenType = tokenResponse.Value<string>("token_type");
                if (string.IsNullOrEmpty(TokenType)) { throw new InvalidTokenResponseException(missingOrEmptyMessage("token_type")); }

                ExpiresIn = tokenResponse.Value<int>("expires_in");
                if (ExpiresIn < 1) { throw new InvalidTokenResponseException($"The 'expires_in' property must be greater than 0"); }

                var rawApiEndpoint = tokenResponse.Value<string>("api_endpoint");
                if (string.IsNullOrEmpty(rawApiEndpoint)) { throw new InvalidTokenResponseException(missingOrEmptyMessage("api_endpoint")); }

                try
                {
                    ApiEndpoint = new Uri(rawApiEndpoint);
                }
                catch (UriFormatException ex)
                {
                    throw new InvalidTokenResponseException(
                        $"The 'api_endpoint' property could not be parsed as a valid absolute Uri. The value received was '{rawApiEndpoint}'.",
                        ex);
                }

                OrgKey = tokenResponse.Value<string>("orgkey");
                if (string.IsNullOrEmpty(OrgKey)) { throw new InvalidTokenResponseException(missingOrEmptyMessage("orgkey")); }

                RefreshToken = tokenResponse.Value<string>("refresh_token");
                if (string.IsNullOrEmpty(RefreshToken)) { throw new InvalidTokenResponseException(missingOrEmptyMessage("refresh_token")); }
            }
            catch (Exception ex)
            {
                if (ex is InvalidTokenResponseException)
                {
                    throw;
                }

                throw new InvalidTokenResponseException("An unknown exception occurred trying to parse the token response from Actionstep", ex);
            }
        }

        public TokenSet(string accessToken, string tokenType, int expiresIn, Uri apiEndpoint, string orgKey, string refreshToken, Instant receivedAt, string userId = null, string id = null, JwtSecurityToken idToken = null, Instant? revokedAt = null)
        {
            if (string.IsNullOrEmpty(accessToken)) { throw new ArgumentException(Helper.NullOrEmptyParameterString, nameof(accessToken)); }
            AccessToken = accessToken;

            if (string.IsNullOrEmpty(tokenType)) { throw new ArgumentException(Helper.NullOrEmptyParameterString, nameof(tokenType)); }
            TokenType = tokenType;

            if (expiresIn < 1) { throw new ArgumentOutOfRangeException(nameof(expiresIn), $"The parameter must be greater than 0"); }
            ExpiresIn = expiresIn;

            if (apiEndpoint is null) { throw new ArgumentNullException(nameof(apiEndpoint)); }
            if (!apiEndpoint.IsAbsoluteUri) { throw new ArgumentException("Must be an absolute uri.", nameof(apiEndpoint)); }
            ApiEndpoint = apiEndpoint;

            if (string.IsNullOrEmpty(orgKey)) { throw new ArgumentException(Helper.NullOrEmptyParameterString, nameof(orgKey)); }
            OrgKey = orgKey;

            if (string.IsNullOrEmpty(refreshToken)) { throw new ArgumentException(Helper.NullOrEmptyParameterString, nameof(refreshToken)); }
            RefreshToken = refreshToken;

            ReceivedAt = receivedAt;
            UserId = userId;
            Id = id;
            RevokedAt = revokedAt;

            if (idToken != null) { IdToken = idToken; }
        }

        public TokenSetRefreshLockInfo LockForRefresh(Instant lockExpiresAt)
        {
            RefreshLockInfo = new TokenSetRefreshLockInfo(lockExpiresAt);
            return RefreshLockInfo;
        }

        public void ResetRefreshLock()
        {
            RefreshLockInfo = null;
        }

        /// <summary>
        /// Helper method to check whether access token is present and has not expired.
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="expiryBuffer">
        ///     Allows the caller to specify a buffer from the expiry time within with the token will be determined as expired.
        ///     For example, let's say an Access Token really expires at 14:30. If you set <paramref name="expiryBuffer"/> to 10 mins,
        ///     <see cref="AccessTokenAppearsValid(IClock, Duration)"/> will return <see langword="false"/> from 14:20 onwards.
        ///     So if the time is 14:25, <see langword="false"/> will be returned.
        /// </param>
        /// <returns></returns>
        public bool AccessTokenAppearsValid(IClock clock, Duration expiryBuffer)
        {
            if (clock is null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            return !string.IsNullOrEmpty(AccessToken)
                && !RevokedAt.HasValue
                && clock.GetCurrentInstant().Plus(expiryBuffer) < AccessTokenExpiresAt;
        }

        /// <summary>
        /// Helper method to check whether access token is present and has not expired.
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="expiryBuffer"></param>
        /// <returns></returns>
        public bool RefreshTokenAppearsValid(IClock clock)
        {
            if (clock is null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            return !string.IsNullOrEmpty(RefreshToken)
                && !RevokedAt.HasValue
                && clock.GetCurrentInstant() < RefreshTokenExpiresAt;
        }

        /// <summary>
        /// The response as received from the Actionstep auth endpoint.
        /// </summary>
        public JObject Response { get; }

        /// <summary>
        /// A unique identifier for this TokenSet.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The id of the user who these tokens belong to.
        /// </summary>
        public string UserId { get; set; }

        public TokenSetRefreshLockInfo RefreshLockInfo { get; set; }

        /// <summary>
        /// Are sure you don't want to use <see cref="AuthorizationHeaderValue"/> instead?
        /// 
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        [JsonProperty("access_token")]
        public string AccessToken { get; private set; }

        /// <summary>
        /// Gets or sets the id token
        /// </summary>
        /// <value>
        /// The ID token
        /// </value>
        [JsonProperty("id_token")]
        public JwtSecurityToken IdToken { get; private set; }

        /// <summary>
        /// Gets or sets the type of the token. Should always be "bearer"
        /// </summary>
        /// <example>bearer</example>
        [JsonProperty("token_type")]
        public string TokenType { get; private set; }

        /// <summary>
        /// Gets or sets the number of seconds that the access token will expire in.
        /// </summary>
        /// <example>3600</example>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; private set; }

        /// <summary>
        /// Gets or sets the API endpoint that should be used with this access token.
        /// Actionstep uses different API endpoints, so this value should be used
        /// instead of a static API endpoint value.
        /// </summary>
        /// <value>The API endpoint URL.</value>
        /// <example>https://api.actionsteplabs.com/api/</example>
        [JsonProperty("api_endpoint")]
        public Uri ApiEndpoint { get; private set; }

        /// <summary>
        /// Gets or sets the org key that this access token can be used with.
        /// Actionstep API tokens can only be used against one organisation.
        /// </summary>
        /// <value>
        /// The org key of the organisation that this access token applies to.
        /// </value>
        [JsonProperty("orgkey")]
        public string OrgKey { get; private set; }

        /// <summary>
        /// Gets or sets the refresh token which can be used to retrieve a new access token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; private set; }

        /// <summary>
        /// Gets or sets the date time in UTC that the tokens were received.
        /// </summary>
        /// <value>
        /// The received UTC.
        /// </value>
        public Instant ReceivedAt { get; private set; }

        [JsonIgnore]
        public Instant AccessTokenExpiresAt { get { return ReceivedAt.Plus(Duration.FromSeconds(ExpiresIn)); } }

        /// <summary>
        /// Actionstep refresh tokens have a 21 day expiry as at 28 May 2018, and as documented at:
        /// https://actionstep.atlassian.net/wiki/spaces/API/pages/12025899/Authorization
        /// </summary>
        [JsonIgnore]
        public Instant RefreshTokenExpiresAt { get { return ReceivedAt.Plus(Duration.FromDays(21)); } }

        public AuthenticationHeaderValue AuthorizationHeaderValue { get => new AuthenticationHeaderValue(TokenType, AccessToken); }

        /// <summary>
        /// A flag to indicate whether this token is known to be revoked.
        /// Use this to prevents unnecessary usage if a token is known to be bad.
        /// </summary>
        public Instant? RevokedAt { get; private set; }

        public void MarkRevoked(Instant revokedAt)
        {
            RevokedAt = revokedAt;
        }
    }
}