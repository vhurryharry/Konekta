namespace WCA.GlobalX.Client
{
    public class GlobalXOptions
    {
        public GlobalXEnvironment Environment { get; set; }

        /// <summary>
        /// OAuth Client ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// OAuth Client Secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// The API Key is required by some API services in addition to an OAuth token.
        /// </summary>
        public string ApiKey { get; set; }
    }
}