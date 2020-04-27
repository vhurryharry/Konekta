using MediatR;
using NodaTime.Testing;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Tests.MockServices;
using WCA.Core.Features.Actionstep;
using WCA.Domain.Models.Account;
using WCA.UnitTests.TestInfrastructure;
using Xunit;

namespace WCA.UnitTests.Actionstep
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class DisconnectFromActionstepOrgTests
    {
        private readonly WebContainerFixture _containerFixture;

        public DisconnectFromActionstepOrgTests(WebContainerFixture containerFixture)
        {
            _containerFixture = containerFixture;
        }

        [Fact]
        public async Task CanDisconnectFromActionstepOrg()
        {
            var fakeClock = FakeClock.FromUtc(2019, 10, 10);
            var now = fakeClock.GetCurrentInstant();

            var testTokenSetRepository = new TestTokenSetRepository();

            // Add test token
            var testUser = new WCAUser() { Id = "0" };
            const string orgKey = "testOrgKey";

            await testTokenSetRepository.AddOrUpdateTokenSet(new TokenSet("token0", "bearer0", 3600, new Uri("https://test-endpoint/api/"), orgKey, "testRefreshToken", now, testUser.Id));

            // Make sure the test item was added so we can be sure it was removed and not missing to begin with
            Assert.Single(testTokenSetRepository.TokenSets);

            IRequestHandler<DisconnectFromActionstepOrg.DisconnectFromActionstepOrgCommand> handlerUnderTest = new DisconnectFromActionstepOrg.Handler(new DisconnectFromActionstepOrg.ValidatorCollection(), testTokenSetRepository);

            await handlerUnderTest.Handle(new DisconnectFromActionstepOrg.DisconnectFromActionstepOrgCommand() { AuthenticatedUser = testUser, ActionstepOrgKey = orgKey }, new CancellationToken());

            Assert.Empty(testTokenSetRepository.TokenSets);
        }
    }
}
