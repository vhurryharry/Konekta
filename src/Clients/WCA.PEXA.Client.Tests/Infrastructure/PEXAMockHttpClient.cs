using FakeItEasy;
using System.Net;
using System.Net.Http;

namespace WCA.PEXA.Client.Tests.Infrastructure
{
    public class PEXAMockHttpClient : HttpClient
    {
        public PEXAMockClientHandler MockHandler { get; private set; }

        public PEXAMockHttpClient(PEXAMockClientHandler handler) : base(handler)
        {
            MockHandler = handler;

            A.CallTo(() => handler.SendAsync(HttpMethod.Post, "/api/rest/v2/workspace"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseData.create-workspace-success.xml")));

            A.CallTo(() => handler.SendAsync(HttpMethod.Get, "/exception-response"))
                .ReturnsLazily(() => BadRequest(EmbeddedResource.Read("ResponseData.create-workspace-exception-response.xml")));
        }

        private static HttpResponseMessage Success(string content)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(content);
            return response;
        }

        private static HttpResponseMessage BadRequest(string content)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent(content);
            return response;
        }
    }
}
