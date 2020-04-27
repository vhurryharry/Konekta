using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public interface IActionstepRequest
    {
        /// <summary>
        /// The <see cref="TokenSetQuery"/> with access/refresh tokens to be used for this request. If the access
        /// token has expired or a 401 is returned, then the the token will be automatically refreshed and
        /// the request will be retried.
        /// </summary>
        TokenSetQuery TokenSetQuery { get; set; }

        /// <summary>
        /// The resource path relative to the base Api Endpoint. Should not start with a slash (/) in case
        /// the base Api Endpoint is not at the root of a domain. If you include a starting slash (/) it
        /// will be removed when the request is parsed.
        /// 
        /// This is required because the Api Endpoint base is returned along with access tokens and may differ
        /// per user.
        /// </summary>
        string RelativeResourcePath { get; }

        /// <summary>
        /// The HttpMethod for this request.
        /// </summary>
        HttpMethod HttpMethod { get; }

        /// <summary>
        /// An object to be serialised to JSON that makes up the payload to be sent to Actionstep.
        /// </summary>
        object JsonPayload { get; }
    }
}
