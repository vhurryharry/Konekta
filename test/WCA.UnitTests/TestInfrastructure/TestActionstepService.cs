using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;

namespace WCA.UnitTests.TestInfrastructure
{
    public class TestActionstepService : IActionstepService
    {
        private List<MockResponseInfo> _mockResponses;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public int NumberOfRequestsMade { get; private set; } = 0;

        public Uri AuthEndpoint => new Uri("https://uri/api/");

        public Uri TokenUri => new Uri(AuthEndpoint, Path.Combine(AuthEndpoint.AbsolutePath, "oauth/token"));

        public Uri AuthorizeUri => new Uri(AuthEndpoint, Path.Combine(AuthEndpoint.AbsolutePath, "oauth/authorize"));

        public Uri JwtPublicKeysUri { get; } = new Uri("https://cdn.actionstep.com/jwt-discovery-public.json", UriKind.Absolute);

        public Uri LaunchPadUri => throw new NotImplementedException();

        public Uri WebFormPostUri => throw new NotImplementedException();

        public ActionstepEnvironment ActionstepEnvironment => throw new NotImplementedException();

        public Uri EndSessionUri => throw new NotImplementedException();

        public TestActionstepService()
        {
            _mockResponses = new List<MockResponseInfo>();
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            _jsonSerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        }

        /// <summary>
        /// Sample response for testing.
        /// </summary>
        /// <param name="exactPathMatch">Regex to match this response. If matched, this response will be supplied. Only matches the path, be sure to include the starting slash!</param>
        /// <param name="httpStatusCode"></param>
        /// <param name="httpMethod"></param>
        /// <param name="responseJsonResourceName"></param>
        public void AddSampleResponse(
            string exactPathMatch,
            HttpStatusCode httpStatusCode,
            HttpMethod httpMethod,
            string responseJsonResourceName)
        {
            _mockResponses.Add(new MockResponseInfo()
            {
                ExactPathMatch = exactPathMatch,
                HttpResponseStatusCode = httpStatusCode,
                HttpMethod = httpMethod,
                ResponseJsonResourceName = responseJsonResourceName,
            });
        }

        /// <summary>
        /// Sample response for testing.
        /// </summary>
        /// <param name="matchPath">Regex to match this response. If matched, this response will be supplied. Only matches the path, be sure to include the starting slash!</param>
        /// <param name="httpStatusCode"></param>
        /// <param name="httpMethod"></param>
        /// <param name="responseJsonResourceName"></param>
        public void AddSampleResponse(
            Regex matchPath,
            HttpStatusCode httpStatusCode,
            HttpMethod httpMethod,
            string responseJsonResourceName)
        {
            _mockResponses.Add(new MockResponseInfo()
            {
                MatchPath = matchPath,
                HttpResponseStatusCode = httpStatusCode,
                HttpMethod = httpMethod,
                ResponseJsonResourceName = responseJsonResourceName,
            });
        }

        private HttpResponseMessage GetMockResponseMessage(string requestUrl, HttpMethod httpMethod)
        {
            var responseInfo = GetMockResponseInfo(requestUrl, httpMethod);
            return new HttpResponseMessage(responseInfo.HttpResponseStatusCode)
            {
                Content = new StringContent(responseInfo.ResponseJsonAsString())
            };
        }

        private MockResponseInfo GetMockResponseInfo(string requestUrl, HttpMethod httpMethod)
        {
            NumberOfRequestsMade++;

            // First search for exact matches
            var mockResponse = _mockResponses.FirstOrDefault(i => i.ExactPathMatch == requestUrl && i.HttpMethod == httpMethod);

            // If not yet found, also check Regex matches
            if (mockResponse == null)
            {
                mockResponse = _mockResponses.FirstOrDefault(i => i.MatchPath.IsMatch(requestUrl) && i.HttpMethod == httpMethod);
            }

            if (mockResponse == null)
            {
                mockResponse = new MockResponseInfo()
                {
                    HttpResponseStatusCode = HttpStatusCode.NotFound
                };
            }

            return mockResponse;
        }

        public Task Handle(IActionstepRequest actionstepRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> Handle<TResponse>(IActionstepRequest actionstepRequest)
        {
            if (actionstepRequest is null) throw new ArgumentNullException(nameof(actionstepRequest));

            using (var mockResponse = GetMockResponseMessage(actionstepRequest.RelativeResourcePath, actionstepRequest.HttpMethod))
            {
                var responseBody = await mockResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseBody, _jsonSerializerSettings);
            }
        }

        public Task<TokenSet> RefreshAccessTokenIfExpired(TokenSet tokenSet, bool forceRefresh = false)
        {
            return Task.FromResult(tokenSet);
        }

        public Task<UploadFileResponse> UploadFile(TokenSetQuery tokenSetQuery, string fileName, string tempContentFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<UploadFileResponse> UploadFile(TokenSetQuery tokenSetQuery, string fileName, Stream stream)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetJwtPublicKeyAsync(string publicKeyId)
        {
            throw new NotImplementedException();
        }

        public JObject GetJwtPublicKeyData()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SecurityKey> GetPublicKeys()
        {
            throw new NotImplementedException();
        }

        private class MockResponseInfo
        {
            public string ExactPathMatch { get; set; }
            public Regex MatchPath { get; set; }
            public HttpStatusCode HttpResponseStatusCode { get; set; }
            public HttpMethod HttpMethod { get; set; }
            public string ResponseJsonResourceName { get; set; }

            public string ResponseJsonAsString() =>
                EmbeddedResource.Read(ResponseJsonResourceName);
        }
    }
}
