using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Net;
using System.Runtime.Serialization;

namespace WCA.Actionstep.Client.Resources
{
    [Serializable]
    public class RefreshTokenErrorResponseException : Exception
    {
        /// <summary>
        /// Thrown when and error is received attempting to retrieve a token.
        /// </summary>
        /// <param name="tokenSet"></param>
        /// <param name="response"></param>
        /// <param name="statusCode"></param>
        /// <param name="receivedAt"></param>
        public RefreshTokenErrorResponseException(TokenSet tokenSet, JObject response, HttpStatusCode statusCode, Instant receivedAt)
            : base(CreateDefaultMessage(tokenSet, statusCode, response))
        {
            TokenSet = tokenSet;
            StatusCode = statusCode;
            ReceivedAt = receivedAt;

            if (!(response is null))
            {
                Response = response;
                RemoteError = response.Value<string>("error");
                RemoteErrorDescription = response.Value<string>("error_description");
                RemoteErrorUri = response.Value<string>("error_uri");
            }
        }

        /// <summary>
        /// The id of the TokenSet that we tried to refresh.
        /// </summary>
        public TokenSet TokenSet { get; }

        /// <summary>
        /// The full JObject response from Actionstep
        /// </summary>
        public JObject Response { get; }

        /// <summary>
        /// The HTTP status code returned by Actionstep
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        /// <example>invalid_grant</example>
        [JsonProperty("error")]
        public string RemoteError { get; }

        /// <summary>Gets or sets the error description.</summary>
        /// <value>The error description.</value>
        /// <example>Invalid refresh_token</example>
        [JsonProperty("error_description")]
        public string RemoteErrorDescription { get; }

        /// <summary>Gets or sets the error URI.</summary>
        /// <value>The error URI.</value>
        /// <example>http://tools.ietf.org/html/rfc6749#section-4.1.3</example>
        [JsonProperty("error_uri")]
        public string RemoteErrorUri { get; }

        /// <summary>
        /// Gets the <see cref="Instant"/> that the tokens were received.
        /// </summary>
        public Instant ReceivedAt { get; }

        private static string CreateDefaultMessage(TokenSet tokenSet, HttpStatusCode httpStatusCode, JObject response)
        {
            var friendlyStatusCode = $"{(int)httpStatusCode} {httpStatusCode.ToString()}";

            if (response is null)
            {
                return $"Actionstep returned an error status code '{friendlyStatusCode}' when attempting to refresh the tokenSet with id '{tokenSet?.Id}'. The response was empty.";
            }
            else
            {
                var remoteError = response.Value<string>("error") ?? "null";
                var remoteErrorDescription = response.Value<string>("error_description") ?? "null";
                var remoteErrorUri = response.Value<string>("error_uri") ?? "null";

                return $"Actionstep returned an error status code '{friendlyStatusCode}' when attempting to refresh a tokenSet." +
                    $" TokenSet ID: '{tokenSet?.Id}'." +
                    $", User ID : '{tokenSet?.UserId}'." +
                    $", OrgKey : '{tokenSet?.OrgKey}'." +
                    $", Remote error:'{remoteError}'" +
                    $", error_description: '{remoteErrorDescription}'" +
                    $", and error_uri: '{remoteErrorUri}'";
            }
        }

        protected RefreshTokenErrorResponseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public RefreshTokenErrorResponseException()
        {
        }

        public RefreshTokenErrorResponseException(string message) : base(message)
        {
        }

        public RefreshTokenErrorResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
