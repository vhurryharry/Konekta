using AutoMapper;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.XmlDiffPatch;
using NodaTime;
using NodaTime.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Tests.MockServices;
using WCA.Core.Features.Conveyancing.Services;
using WCA.Core.Features.Conveyancing.WorkspaceCreation;
using WCA.Data;
using WCA.Domain.Actionstep;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;
using WCA.UnitTests.TestInfrastructure;
using Xunit;
using Xunit.Abstractions;

namespace WCA.UnitTests.Conveyancing
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class ConveyancingMatterTests
    {
        private readonly MockHandler _handler = null;
        private readonly WebContainerFixture _containerFixture;
        private readonly ITestOutputHelper _output;

        public ConveyancingMatterTests(WebContainerFixture containerFixture, ITestOutputHelper output)
        {
            _containerFixture = containerFixture;
            _output = output;

            _handler = A.Fake<MockHandler>(opt => opt.CallsBaseMethods());

            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "/api/rest/actions?id_eq=1&include=actionType,division,division.participant"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseData.get-single-action.json")));
            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "/api/rest/actions?id_in=1,7"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseData.list-multiple-actions.json")));
            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "/api/rest/datacollectionrecordvalues?action=1" +
                    "&dataCollectionRecord[dataCollection][name_in]=property,convdet,keydates" +
                    "&dataCollectionField[name_in]=titleref,lotno,planno,plantype,smtdateonly,smttime,purprice,ConveyType" +
                    "&include=dataCollectionField,dataCollection"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseData.get-data-collection-record-values.json")));
            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "/api/rest/participants"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseData.list-participants.json")));
            A.CallTo(() => _handler.SendAsync(HttpMethod.Get, "/api/rest/actionparticipants?action=1&include=participant,participantType"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseData.list-actionparticipants.json")));

            A.CallTo(() => _handler.SendAsync(HttpMethod.Post, "/api/rest/v2/workspace"))
                .ReturnsLazily(() => Success(EmbeddedResource.Read("ResponseData.create-workspace-success.xml")));
        }

        private static HttpResponseMessage Success(string content)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(content);
            return response;
        }

        [Fact]
        public async Task ShouldCreatePEXAWorkspaceRequestFromActionstep()
        {
            var testUser = await _containerFixture.GetTestUser();
            await _containerFixture.ExecuteScopeAsync(async serviceProvider =>
            {
                using (var authDelegatingHandler = new AuthDelegatingHandler() { InnerHandler = _handler })
                using (var httpClient = new HttpClient(authDelegatingHandler))
                {
                    var fakeClock = new FakeClock(new Instant());
                    var testTokenSetRepository = new TestTokenSetRepository();
                    await testTokenSetRepository.AddOrUpdateTokenSet(new TokenSet("accessToken", "bearer", 3600, new Uri("https://uri/api"), "testOrg", "refreshToken", fakeClock.GetCurrentInstant(), testUser.Id));

                    var options = new ActionstepServiceConfigurationOptions("clientId", "clientSecret");
                    var actionstepService = new ActionstepService(new NullLogger<ActionstepService>(), httpClient, options, testTokenSetRepository, fakeClock, new MemoryCache(new MemoryCacheOptions()));
                    var mapper = serviceProvider.GetService<IMapper>();
                    var actionstepToWcaMapper = serviceProvider.GetService<IActionstepToWCAMapper>();
                    PrepareTestData(serviceProvider);

                    var handler = new PEXAWorkspaceCreationRequestFromActionstepQueryHandler(actionstepService, mapper, actionstepToWcaMapper);
                    var token = new CancellationToken();

                    var query = new PEXAWorkspaceCreationRequestFromActionstepQuery
                    {
                        AuthenticatedUser = testUser,
                        MatterId = 1,
                        ActionstepOrg = "testOrg"
                    };

                    var pexaWorkspaceCreationResponse = await handler.Handle(query, token);

                    Assert.Equal("1", pexaWorkspaceCreationResponse.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.SubscriberReference);
                    Assert.Equal("Yes", pexaWorkspaceCreationResponse.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.FinancialSettlement);

                    var config = new ConfigurationBuilder();
                    config.AddInMemoryCollection(new Dictionary<string, string>() {
                        { "WCACoreSettings:PEXASettings:Environment", "Test" }
                    });

                    var pexaService = new PEXAService(httpClient, config.Build());
                    var request = new WorkspaceCreationRequestCommand(pexaWorkspaceCreationResponse.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest, "dummyToken");
                    var response = await pexaService.Handle<WorkspaceCreationResponse>(request, CancellationToken.None);
                    Assert.Equal("PEXA190167645", response.WorkspaceId);
                    Assert.Equal("In Preparation", response.WorkspaceStatus);

                    var xmldiff = new XmlDiff(XmlDiffOptions.IgnoreChildOrder | XmlDiffOptions.IgnoreNamespaces | XmlDiffOptions.IgnorePrefixes);
                    var expectedXml = EmbeddedResource.Read("ResponseData.create-workspace-result.xml");
                    var actualXml = _handler.RequestContent;

                    var expectedReader = XElement.Parse(expectedXml).CreateReader();
                    var actualReader = XElement.Parse(actualXml).CreateReader();

                    using (var diffStringWriter = new StringWriter())
                    using (var diffWriter = XmlWriter.Create(diffStringWriter))
                    {
                        var areXmlIdentical = xmldiff.Compare(expectedReader, actualReader, diffWriter);
                        diffWriter.Flush();

                        foreach (var diffLine in diffStringWriter.ToString().Split(diffStringWriter.NewLine))
                        {
                            _output.WriteLine(diffLine);
                        }

                        Assert.True(areXmlIdentical);
                    }
                }
            });
        }

        private void PrepareTestData(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetService<WCADbContext>();
            var testUser = dbContext.GetTestUser();
            var actionstepOrg = new ActionstepOrg
            {
                Key = "testOrg",
                Title = "testOrg",
                CreatedBy = testUser,
                DateCreatedUtc = DateTime.UtcNow
            };

            dbContext.ActionstepOrgs.Add(actionstepOrg);
            dbContext.SaveChanges();

            var startTime = DateTime.UtcNow;

            var actionstepCredential = new ActionstepCredential
            {
                AccessToken = "testToken",
                AccessTokenExpiryUtc = startTime.AddMinutes(20),
                ActionstepOrg = actionstepOrg,
                ApiEndpoint = new Uri("http://test-endpoint/api"),
                CreatedBy = testUser,
                DateCreatedUtc = startTime,
                ExpiresIn = 60 * 20,
                ReceivedAtUtc = startTime,
                Owner = testUser,
                RefreshToken = "testRefreshToken",
                RefreshTokenExpiryUtc = startTime.AddDays(20),
                TokenType = "bearer"
            };

            dbContext.ActionstepCredentials.Add(actionstepCredential);
            dbContext.SaveChanges();
        }
    }
}
