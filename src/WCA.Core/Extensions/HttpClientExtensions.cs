using System.Net.Http;

namespace WCA.Core.Extensions
{
    public static class HttpClientExtensions
    {
        public static void SetBearerToken(this HttpClient httpClient, string token)
        {
            const string AuthorizationHeaderKey = "Authorization";

            if (httpClient.DefaultRequestHeaders.Contains(AuthorizationHeaderKey))
            {
                httpClient.DefaultRequestHeaders.Remove(AuthorizationHeaderKey);
            }

            httpClient.DefaultRequestHeaders.Add(AuthorizationHeaderKey, $"Bearer {token}");
        }
    }
}
