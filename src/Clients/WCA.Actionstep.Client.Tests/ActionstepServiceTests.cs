using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Testing;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Actionstep.Client.Tests.MockServices;
using Xunit;

namespace WCA.Actionstep.Client.Tests
{
    public class ActionstepServiceTests
    {
        private readonly MockHandler _handler = null;
        private int _refreshTokenRefreshCounter = 0;
        private int _accessTokenRefreshCounter = 0;

        public ActionstepServiceTests()
        {
            _handler = A.Fake<MockHandler>(opt => opt.CallsBaseMethods());

            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "https://cdn.actionstep.com/jwt-discovery-public.json"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseJsonData.jwt-discovery-public.json")));

            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "https://api.uri/api/rest/actions?id_eq=1&include=actionType,division,division.participant"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseJsonData.get-single-action.json")));

            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "https://api.uri/api/rest/actions?id_in=1,7"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseJsonData.list-multiple-actions.json")));

            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "https://api.uri/api/rest/datacollectionrecordvalues?action=7" +
                    "&dataCollectionRecord[dataCollection][name_in]=property,convdet,keydates" +
                    "&dataCollectionField[name_in]=titleref,lotno,planno,plantype,smtdateonly,smttime,purprice" +
                    "&include=dataCollectionField,dataCollection"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseJsonData.get-data-collection-record-values.json")));

            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "https://api.uri/api/rest/participants"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseJsonData.list-participants.json")));

            A.CallTo(() => _handler.SendAsync(HttpMethod.Post, "https://api.actionstepstaging.com/api/oauth/token"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseJsonData.refresh-token.json")));

            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "https://api.uri/api/auto-increment/oauth/token"))
                .ReturnsLazily(() =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    var data = new
                    {
                        access_token = $"updatedAccessToken-{_accessTokenRefreshCounter}",
                        token_type = "bearer",
                        expires_in = 3600,
                        api_endpoint = "https://api.actionstepstaging.com/api/",
                        orgkey = "testOrg",
                        refresh_token = $"updatedRefreshToken-{_refreshTokenRefreshCounter++}"
                    };

                    response.Content = new StringContent(JsonConvert.SerializeObject(data));
                    return response;
                });

            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "https://api.actionstepstaging.com/api/expired-token/oauth/token"))
                .ReturnsLazily(() =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(EmbeddedResource.Read("ResponseJsonData.expired-token.json"));
                    return response;
                });

            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "invalid-token"))
                .ReturnsLazily(() =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(EmbeddedResource.Read("ResponseJsonData.invalid-token.json"));
                    return response;
                });
        }

        [Fact]
        public void CanGetJwtPublicKeysUri()
        {
            using (var httpClient = new HttpClient())
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var tokenSetRepository = new TestTokenSetRepository();
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, tokenSetRepository, fakeClock, memoryCache);

                Assert.Equal(new Uri("https://cdn.actionstep.com/jwt-discovery-public.json", UriKind.Absolute), actionstepService.JwtPublicKeysUri);
            }
        }

        [Fact]
        public void GetJwtPublicKeyReturnsValue()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var tokenSetRepository = new TestTokenSetRepository();
                IActionstepService actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, tokenSetRepository, fakeClock, memoryCache);

                var receivedPublicKeyJObject = actionstepService.GetJwtPublicKeyData();
                var receivedPublicKey = receivedPublicKeyJObject.Value<string>(JwtPublicKeyIds.ProdPublicKey);

                const string EXPECTED_PUBLIC_KEY = "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAp2ikRwK8OpklNc3U9y2n\n3JE+B1sQftl8TbZs++PHiActfqHqVbdh2n8Pjaft2NDGB9BXOEO9I/F0J3jZJT+H\n2FdO8HoFPLfeMvcrRKvXa2UmSXRrZLA6YIf+wzgI0wgbb+lkEH3LUgHk0XxalnRE\nMmNUpAWtO0ZX2jh8tkp+3ymhzCtXHwYpiI1iJUDUje/kBXvKfdP+QcDgkshehY/w\nph5SEXS/a//r2TWRPbFHlzi96fL7ySZIjTWyQtZ1UwzjTqkA0gRYQLtXEQW5tLgJ\niiwivJ1UiGoGTfp4nj+ZU7OVYV5I/k+t4cv+s1jYAP/hmdW98932DavR9Mmlb80g\nXwIDAQAB\n-----END PUBLIC KEY-----";

                Assert.Equal(EXPECTED_PUBLIC_KEY, receivedPublicKey);
            }
        }

        [Fact]
        public void GetJwtPublicKeyThrowsOnHttpProblem()
        {
            var localHandler = A.Fake<MockHandler>(opt => opt.CallsBaseMethods());

            A.CallTo(() => localHandler.SendAsync(HttpMethod.Get, "https://cdn.actionstep.com/jwt-discovery-public.json"))
                .ReturnsLazily(() => new HttpResponseMessage(HttpStatusCode.BadRequest));

            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = localHandler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var tokenSetRepository = new TestTokenSetRepository();
                IActionstepService actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, tokenSetRepository, fakeClock, memoryCache);

                var ex = Assert.Throws<InvalidJwtDiscoveryResponseException>(() => { actionstepService.GetJwtPublicKeyData(); });

                Assert.Equal("There was a problem retrieving the JWT Discovery public key data.", ex.Message);
                Assert.IsType<HttpRequestException>(ex.InnerException);
            }
        }

        [Fact]
        public void GetJwtPublicKeyThrowsOnEmptyResponse()
        {
            var localHandler = A.Fake<MockHandler>(opt => opt.CallsBaseMethods());

            A.CallTo(() => localHandler.SendAsync(HttpMethod.Get, "https://cdn.actionstep.com/jwt-discovery-public.json"))
                .ReturnsLazily(() => Success(""));

            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = localHandler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var tokenSetRepository = new TestTokenSetRepository();
                IActionstepService actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, tokenSetRepository, fakeClock, memoryCache);

                var ex = Assert.Throws<InvalidJwtDiscoveryResponseException>(() => { actionstepService.GetJwtPublicKeyData(); });

                Assert.Equal("The JWT Discovery public key response was empty.", ex.Message);
            }
        }

        [Fact]
        public void GetJwtPublicKeyThrowsOnInvalidJson()
        {
            var localHandler = A.Fake<MockHandler>(opt => opt.CallsBaseMethods());

            A.CallTo(() => localHandler.SendAsync(HttpMethod.Get, "https://cdn.actionstep.com/jwt-discovery-public.json"))
                .ReturnsLazily(() => Success("Not a JSON string. This is invalid JSON."));

            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = localHandler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var tokenSetRepository = new TestTokenSetRepository();
                IActionstepService actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, tokenSetRepository, fakeClock, memoryCache);

                var ex = Assert.Throws<InvalidJwtDiscoveryResponseException>(() => { actionstepService.GetJwtPublicKeyData(); });

                Assert.Equal("There was a problem parsing the JWT Discovery public key response.", ex.Message);
            }
        }

        [Fact]
        public async void ShouldGetActionWithMatterId()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var tokenSetRepository = new TestTokenSetRepository();
                var testTokenSet = GetTestTokenSet();
                await tokenSetRepository.AddOrUpdateTokenSet(testTokenSet);
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, tokenSetRepository, fakeClock, memoryCache);
                var response = await actionstepService.Handle<GetActionResponse>(new GetActionRequest
                {
                    ActionId = 1,
                    TokenSetQuery = new TokenSetQuery(testTokenSet.UserId, testTokenSet.OrgKey)
                });

                Assert.Equal(1, response.Action.Id);
                Assert.Null(response.Action.IsBillableOverride);
                Assert.False(response.Action.IsDeleted);
                Assert.True(response.Action.IsFavorite);
            }
        }

        [Fact(Skip = "Skipped while we refresh all tokens")]
        public async Task ShouldRefreshTokenNearingTimeout()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var tokenHandler = new TestTokenSetRepository();
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var testTokenRepository = new TestTokenSetRepository();
                var testToken = GetTestTokenSet(isExpired: true);
                await testTokenRepository.AddOrUpdateTokenSet(testToken);

                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, testTokenRepository, fakeClock, memoryCache);

                var response = await actionstepService.Handle<GetActionResponse>(new GetActionRequest
                {
                    ActionId = 1,
                    TokenSetQuery = new TokenSetQuery(testToken.UserId, testToken.OrgKey)
                });

                // Should be called 3 times. First from this test method to arrange this test token, second from the handler to set the lock, and third time to unlock/update with refreshed data.
                Assert.Equal(3, testTokenRepository.AddOrUpdateTokenSetCount);
            }
        }

        [Fact]
        public async Task SecondTokenRefreshWaitsForFirst()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var tokenHandler = new TestTokenSetRepository();
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var testTokenRepository = new TestTokenSetRepository();
                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, testTokenRepository, fakeClock, memoryCache);

                var now = fakeClock.GetCurrentInstant();

                /// We're using the TokenType purely as a mechanism to talk to <see cref="TestTokenSetRepository"/>.
                /// This allows us to delay one refresh while queueing another to ensure the locking/retry works correctly
                /// with concurrent refresh requests.
                var tokenTypeTest1 = "bearer-test1";
                var tokenTypeTest2 = "bearer-test2";

                // First, store a locked expired token
                var tokenId = "locking test token";
                var tokenSet1 = new TokenSet(
                    "testAccessToken1",
                    tokenTypeTest1,
                    3600,
                    new Uri("https://api.uri/api/auto-increment/"),
                    "testOrgKey",
                    "testRefreshToken1",
                    now.Minus(Duration.FromMinutes(120)),
                    "testUser",
                    tokenId);

                tokenSet1.LockForRefresh(now.Plus(Duration.FromMinutes(60)));
                var storedTokenSet1 = await testTokenRepository.AddOrUpdateTokenSet(tokenSet1);

                // Now we set up a second expired token with the same id.
                var tokenSet2 = new TokenSet(
                    "testAccessToken2",
                    tokenTypeTest2,
                    3600,
                    new Uri("https://api.uri/api/auto-increment/"),
                    "testOrgKey",
                    "testRefreshToken2",
                    now.Minus(Duration.FromMinutes(120)),
                    "testUser",
                    tokenId);

                // Try to refresh using the second token (which has the same tokenId). It should be blocked by the locked token1 from above.
                var tokenSet2RefreshTask = actionstepService.RefreshAccessTokenIfExpired(tokenSet2, forceRefresh: false);

                /// Delay to give the <see cref="RefreshAccessTokenIfExpired"/> a chance to 
                await Task.Delay(50);

                // Now store a valid token of test type 1, this will unlock the token
                tokenSet1 = new TokenSet(
                    "testAccessToken1",
                    tokenTypeTest1,
                    3600,
                    new Uri("https://api.uri/api/auto-increment/"),
                    "testOrgKey",
                    "testRefreshToken1",
                    now,
                    "testUser",
                    tokenId);

                await testTokenRepository.AddOrUpdateTokenSet(tokenSet1);

                tokenSet2RefreshTask.Wait();

                // The token from the refresh should be the one we put in the repository.
                Assert.Equal(tokenTypeTest1, tokenSet2RefreshTask.Result.TokenType);
            }
        }

        [Fact(Skip = "Skipped while we refresh all tokens")]
        public async Task ShouldRetryRefreshingInvalidToken()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var tokenHandler = new TestTokenSetRepository();
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var testTokenRepository = new TestTokenSetRepository();
                var testTokenSet = GetTestTokenSet(isInvalid: true);
                await testTokenRepository.AddOrUpdateTokenSet(testTokenSet);
                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, testTokenRepository, fakeClock, memoryCache);

                var response = await actionstepService.Handle<GetActionResponse>(new GetActionRequest
                {
                    ActionId = 1,
                    TokenSetQuery = new TokenSetQuery(testTokenSet.UserId, testTokenSet.OrgKey)
                });

                // Should be called 3 times. First from this test method to arrange this test token, second from the handler to set the lock, and third time to unlock/update with refreshed data.
                Assert.Equal(3, testTokenRepository.AddOrUpdateTokenSetCount);
            }
        }

        [Fact]
        public async Task ShouldGetActionWithMultipleMatterIds()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var testTokenRepository = new TestTokenSetRepository();
                var testTokenSet = GetTestTokenSet();
                await testTokenRepository.AddOrUpdateTokenSet(testTokenSet);

                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, testTokenRepository, fakeClock, memoryCache);
                var listActionsRequest = new ListActionsRequest();
                listActionsRequest.ActionstepIds.AddRange(new[] { 1, 7 });
                listActionsRequest.TokenSetQuery = new TokenSetQuery(testTokenSet.UserId, testTokenSet.OrgKey);

                var response = await actionstepService.Handle<ListActionsResponse>(listActionsRequest);

                Assert.Equal(2, response.Actions.Count);
                Assert.True(response.Actions.Exists(a => a.Id == 1));
                Assert.True(response.Actions.Exists(a => a.Id == 7));
            }
        }

        [Fact]
        public async void ShouldGetDataCollectionValues()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var testTokenRepository = new TestTokenSetRepository();
                var testTokenSet = GetTestTokenSet();
                await testTokenRepository.AddOrUpdateTokenSet(testTokenSet);

                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, testTokenRepository, fakeClock, memoryCache);

                var dataCollectionRecordValuesRequest = new ListDataCollectionRecordValuesRequest();
                dataCollectionRecordValuesRequest.ActionstepId = 7;
                dataCollectionRecordValuesRequest.TokenSetQuery = new TokenSetQuery(testTokenSet.UserId, testTokenSet.OrgKey);
                dataCollectionRecordValuesRequest.DataCollectionRecordNames.AddRange(new[] { "property", "convdet", "keydates" });
                dataCollectionRecordValuesRequest.DataCollectionFieldNames.AddRange(new[] { "titleref", "lotno", "planno", "plantype", "smtdateonly", "smttime", "purprice" });

                var response = await actionstepService.Handle<ListDataCollectionRecordValuesResponse>(dataCollectionRecordValuesRequest);
                Assert.Equal(6, response.DataCollectionRecordValues.Count);
                Assert.Equal("test-title-ref", response["property", "titleref"]);
            }
        }

        [Fact]
        public async void ShouldListActionstepParticipants()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var testTokenRepository = new TestTokenSetRepository();
                var testTokenSet = GetTestTokenSet();
                await testTokenRepository.AddOrUpdateTokenSet(testTokenSet);

                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, testTokenRepository, fakeClock, memoryCache);

                var request = new ListParticipantsRequest
                {
                    TokenSetQuery = new TokenSetQuery(testTokenSet.UserId, testTokenSet.OrgKey)
                };

                var response = await actionstepService.Handle<ListParticipantsResponse>(request);
                var admin = response.Participants.SingleOrDefault(p => p.Id == 1);

                Assert.NotNull(admin);
                var expectedBirthTimestamp = new LocalDate(2000, 1, 1);
                var expectedDeathTimestamp = new LocalDate(2001, 1, 1);
                Assert.Equal(expectedBirthTimestamp, admin.BirthTimestamp);
                Assert.Equal(expectedDeathTimestamp, admin.DeathTimestamp);
            }
        }

        [Fact]
        public async void ShouldRefreshActionstepToken()
        {
            using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
            using (var httpClient = new HttpClient(authDelegatingHandler))
            using (var memoryCache = new MemoryCache(new MemoryCacheOptions()))
            {
                var tokenHandler = new TestTokenSetRepository();
                var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
                var testTokenRepository = new TestTokenSetRepository();
                var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, testTokenRepository, fakeClock, memoryCache);

                // Expired token
                var tokenSet = new TokenSet(
                    accessToken: "testAccessToken",
                    tokenType: "bearer",
                    expiresIn: 60 * 20,
                    apiEndpoint: new Uri("https://api.uri/api/"),
                    orgKey: "testOrgKey",
                    refreshToken: "testRefreshToken",
                    userId: "1",
                    id: "tokenId",
                    receivedAt: fakeClock.GetCurrentInstant().Minus(Duration.FromDays(365)));

                var response = await actionstepService.RefreshAccessTokenIfExpired(tokenSet);
                Assert.NotNull(response);
                Assert.Equal("updatedAccessToken", response.AccessToken);
                Assert.Equal(3600, response.ExpiresIn);
                Assert.Equal("updatedRefreshToken", response.RefreshToken);
                Assert.Equal("testOrg", response.OrgKey);
                Assert.Equal(new Uri("https://test-endpoint/api/"), response.ApiEndpoint);
                Assert.Equal("tokenId", response.Id);
                Assert.Equal(2, testTokenRepository.AddOrUpdateTokenSetCount);
            }
        }

        private static HttpResponseMessage Success(string content)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(content);
            return response;
        }

        private TokenSet GetTestTokenSet(bool isExpired = false, bool isInvalid = false)
        {
            var fakeClock = new FakeClock(Instant.FromUtc(2019, 05, 07, 2, 3));
            var now = fakeClock.GetCurrentInstant();

            return new TokenSet(
                accessToken: isInvalid ? "testInvalidAccessToken" : "testAccessToken",
                tokenType: "bearer",
                expiresIn: 3600,
                apiEndpoint: new Uri("https://api.uri/api/"),
                orgKey: "testOrgKey",
                refreshToken: "testRefreshToken",
                userId: "1",
                receivedAt: isExpired ? now.Minus(Duration.FromDays(365)) : now);
        }
    }
}
