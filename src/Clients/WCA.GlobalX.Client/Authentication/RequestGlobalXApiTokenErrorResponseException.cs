using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Net;
using System.Runtime.Serialization;

namespace WCA.GlobalX.Client.Authentication
{
    [Serializable]
    public class RequestGlobalXApiTokenErrorResponseException : Exception
    {
        /// <summary>
        /// Thrown when and error is received attempting to retrieve a token.
        /// </summary>
        /// <param name="apiToken"></param>
        /// <param name="response"></param>
        /// <param name="statusCode"></param>
        /// <param name="receivedAt"></param>
        public RequestGlobalXApiTokenErrorResponseException(
            GlobalXCredentials globalXCredentials,
            JObject response,
            HttpStatusCode statusCode,
            Instant receivedAt)
            : base(CreateDefaultMessage(globalXCredentials, statusCode, response))
        {
            GlobalXCredentials = globalXCredentials;
            StatusCode = statusCode;
            ReceivedAt = receivedAt;

            if (!(response is null))
            {
                Response = response;
                RemoteError = response.Value<string>("error");
            }
        }

        /// <summary>
        /// The credentials used to create the request.
        /// </summary>
        public GlobalXCredentials GlobalXCredentials { get; }

        /// <summary>
        /// The full JObject response from GlobalX
        /// </summary>
        public JObject Response { get; }

        /// <summary>
        /// The HTTP status code returned by GlobalX
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        /// <example>invalid_grant</example>
        [JsonProperty("error")]
        public string RemoteError { get; }

        /// <summary>
        /// Gets the <see cref="Instant"/> that the tokens were received.
        /// </summary>
        public Instant ReceivedAt { get; }

        private static string CreateDefaultMessage(GlobalXCredentials globalXCredentials, HttpStatusCode httpStatusCode, JObject response)
        {
            var friendlyStatusCode = $"{(int)httpStatusCode} {httpStatusCode.ToString()}";

            if (response is null)
            {
                return $"GlobalX returned an error status code '{friendlyStatusCode}' when attempting to refresh the tokenSet for user '{globalXCredentials?.UserId}'. The response was empty.";
            }
            else
            {
                var remoteError = response.Value<string>("error") ?? "null";
                var remoteErrorDescription = response.Value<string>("error_description") ?? "null";
                var remoteErrorUri = response.Value<string>("error_uri") ?? "null";

                return $"GlobalX returned an error status code '{friendlyStatusCode}' when attempting to request a token from username and password." +
                    $", User ID : '{globalXCredentials?.UserId}'." +
                    $", Remote error:'{remoteError}'" +
                    $", error_description: '{remoteErrorDescription}'" +
                    $", and error_uri: '{remoteErrorUri}'";
            }
        }

        protected RequestGlobalXApiTokenErrorResponseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public RequestGlobalXApiTokenErrorResponseException()
        {
        }

        public RequestGlobalXApiTokenErrorResponseException(string message) : base(message)
        {
        }

        public RequestGlobalXApiTokenErrorResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
