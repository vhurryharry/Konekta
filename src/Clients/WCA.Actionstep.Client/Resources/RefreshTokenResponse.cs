using Newtonsoft.Json;
using NodaTime;

namespace WCA.Actionstep.Client.Resources
{
    public class RefreshTokenResponse
    {
        /// <summary>
        /// See the Actionstep API documentation for more details
        /// https://actionstep.atlassian.net/wiki/spaces/API/pages/12025899/Authorization
        /// 
        /// As at 17/05/2019 the documentation states that:
        ///    Access Tokens have a 60 minute lifespan and can easily be renewed via the
        ///    OAuth2 token endpoint by using a Refresh Token which has a 21 day lifespan.
        ///    A Refresh Token is returned with every Access Token issued.
        /// </summary>
        public const int RefreshTokenLifespanInDays = 21;

        [JsonIgnore]
        public string CustomId { get; set; }

        [JsonIgnore]
        public Instant RefreshedAt { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// The token will expire in this many seconds after it was received.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("api_endpoint")]
        public string ApiEndpoint { get; set; }

        [JsonProperty("orgkey")]
        public string OrgKey { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public Instant RefreshTokenExpiresAt { get => RefreshedAt.Plus(Duration.FromDays(RefreshTokenLifespanInDays)); }
        public Instant AccessTokenExpiresAt { get => RefreshedAt.Plus(Duration.FromSeconds(ExpiresIn)); }
    }
}
