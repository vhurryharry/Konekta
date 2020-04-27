using Actionstep;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Tests.MockServices;
using WCA.Core;
using WCA.UnitTests.TestInfrastructure;
using Xunit;

namespace WCA.UnitTests.Functions
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class RefreshTokenTests
    {
        private readonly MockHandler _handler = null;

        public RefreshTokenTests()
        {
            _handler = A.Fake<MockHandler>(opt => opt.CallsBaseMethods());

            A.CallTo(() => _handler.SendAsync(HttpMethod.Post, "/api/oauth/token"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseData.refresh-token.json")));
        }

        private static HttpResponseMessage Success(string content)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(content);
            return response;
        }

        /// <summary>
        /// This function does not return results so testing the flow of execution
        /// </summary>
        /// <returns></returns>
        [Fact(Skip = "Skipped while we refresh all tokens")]
        public async Task ShouldRefreshAllRefreshableTokens()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var fakeClock = FakeClock.FromUtc(2019, 10, 10);
                var now = fakeClock.GetCurrentInstant();

                var testTokenSetRepository = new TestTokenSetRepository();

                //// Add test tokens

                // Valid tokens (shouldn't be refreshed)
                await testTokenSetRepository.AddOrUpdateTokenSet(new TokenSet("token0", "bearer0", 3600, new Uri("https://test-endpoint/api/"), "testOrgKey", "testRefreshToken", now, "0", "0"));
                await testTokenSetRepository.AddOrUpdateTokenSet(new TokenSet("token1", "bearer1", 3600, new Uri("https://test-endpoint/api/"), "testOrgKey", "testRefreshToken", now, "1", "1"));

                // Nearing expiry (should be refreshed)
                await testTokenSetRepository.AddOrUpdateTokenSet(new TokenSet("token2", "bearer2", 3600, new Uri("https://test-endpoint/api/"), "testOrgKey", "testRefreshToken", now.Minus(Duration.FromDays(18)), "2", "2"));

                // Expired (shouldn't be refreshed, as it will fail anyway)
                await testTokenSetRepository.AddOrUpdateTokenSet(new TokenSet("token3", "bearer3", 3600, new Uri("https://test-endpoint/api/"), "testOrgKey", "testRefreshToken", now.Minus(Duration.FromDays(30)), "3", "3"));

                var actionstepServiceConfigurationOptions = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, actionstepServiceConfigurationOptions, testTokenSetRepository, fakeClock, memoryCache);

                var config = new ConfigurationBuilder();
                config.AddInMemoryCollection(new Dictionary<string, string>() {
                    { "WCACoreSettings:PEXASettings:Environment", "Test" }
                });

                var wcaCoreSettingsOptions = Options.Create(config.Build().Get<WCACoreSettings>());

                var refreshTokens = new RefreshAllTokens(testTokenSetRepository, actionstepService, fakeClock);

                await refreshTokens.Run(null, new NullLogger<RefreshAllTokens>());

                Assert.Equal("token0", testTokenSetRepository.TokenSets.Single(t => t.Id == "0").AccessToken);
                Assert.Equal("token1", testTokenSetRepository.TokenSets.Single(t => t.Id == "1").AccessToken);
                Assert.Equal("updatedAccessToken", testTokenSetRepository.TokenSets.Single(t => t.Id == "2").AccessToken);
                Assert.Equal("token3", testTokenSetRepository.TokenSets.Single(t => t.Id == "3").AccessToken);
            }
        }
    }
}
