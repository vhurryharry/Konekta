using NodaTime;
using NodaTime.Testing;
using System;
using System.Linq;
using WCA.Domain.Actionstep;
using Xunit;

namespace WCA.UnitTests.Actionstep
{
    public class ActionstepMatterTests
    {
        [Theory]
        [InlineData("abc_123", "abc", 123)]
        [InlineData("ab_c_123", "ab_c", 123)]
        [InlineData("ab_321_123", "ab_321", 123)]
        public void CanDeconstructActionstepOrgId(string id, string expectedOrgKey, int expectedMatterId)
        {
            (string orgKey, int matterId) = ActionstepMatter.DeconstructId(id);
            Assert.Equal(expectedOrgKey, orgKey);
            Assert.Equal(expectedMatterId, matterId);
        }

        [Theory]
        [InlineData("abc_")]
        [InlineData("_abc")]
        [InlineData("abc_abc")]
        public void DeconstructActionstepOrgIdThrows(string id)
        {
            Assert.Throws<InvalidActionstepMatterAggregateIdException>(() =>
            {
                ActionstepMatter.DeconstructId(id);
            });
        }

        [Fact]
        public void CannotRequestPexaWorkspaceIfRequestInProgress()
        {
            var fakeClock = FakeClock.FromUtc(2019, 8, 20);
            fakeClock.AutoAdvance = Duration.FromSeconds(1);

            var matter = new ActionstepMatter(fakeClock.GetCurrentInstant(), "orgkey", 1, "fakeuser");
            matter.RequestPexaWorkspaceCreation(fakeClock.GetCurrentInstant(), "fakeuser");

            Assert.Throws<CannotCreatePexaWorkspaceException>(() => {
                matter.RequestPexaWorkspaceCreation(fakeClock.GetCurrentInstant(), "fakeuser");
            });
        }

        [Fact]
        public void CanRequestPexaWorkspaceAfterPreviousRequestFailed()
        {
            var fakeClock = FakeClock.FromUtc(2019, 8, 20);
            fakeClock.AutoAdvance = Duration.FromSeconds(1);

            var matter = new ActionstepMatter(fakeClock.GetCurrentInstant(), "orgkey", 1, "fakeuser");

            matter.Apply(new PexaWorkspaceCreationFailed(fakeClock.GetCurrentInstant(), "Test failure", Guid.Empty, Guid.Empty));
            matter.MarkChangesAsCommitted();

            matter.RequestPexaWorkspaceCreation(fakeClock.GetCurrentInstant(), "fakeuser");

            var firstUncommitedChange = matter.GetUncommittedChanges().First();

            Assert.True(firstUncommitedChange is PexaWorkspaceCreationRequested);
        }

        [Fact]
        public void PexaWorkspaceCreatedSuccessfullyIsCorrelatedAndCausedByCorrectly()
        {
            var fakeClock = FakeClock.FromUtc(2019, 8, 20);
            fakeClock.AutoAdvance = Duration.FromSeconds(1);

            var matter = new ActionstepMatter(fakeClock.GetCurrentInstant(), "orgkey", 1, "fakeuser");

            var creationRequested = matter.RequestPexaWorkspaceCreation(fakeClock.GetCurrentInstant(), "fakeuser");
            matter.MarkChangesAsCommitted();
            matter.MarkPexaWorkspaceCreated(fakeClock.GetCurrentInstant(), "abc123", creationRequested);
            var pexaWorkspaceCreated = matter.GetUncommittedChanges().Single();

            Assert.Equal(creationRequested.CorrelationId, pexaWorkspaceCreated.CorrelationId);
            Assert.Equal(creationRequested.EventId, pexaWorkspaceCreated.CausationId);
        }


        [Fact]
        public void PexaWorkspaceFailureCorrelatedAndCausedByCorrectly()
        {
            var fakeClock = FakeClock.FromUtc(2019, 8, 20);
            fakeClock.AutoAdvance = Duration.FromSeconds(1);

            var matter = new ActionstepMatter(fakeClock.GetCurrentInstant(), "orgkey", 1, "fakeuser");

            var creationRequested = matter.RequestPexaWorkspaceCreation(fakeClock.GetCurrentInstant(), "fakeuser");
            matter.MarkChangesAsCommitted();
            matter.MarkPexaWorkspaceCreationFailed(fakeClock.GetCurrentInstant(), "failure message", creationRequested);
            var pexaWorkspaceCreationFailed = matter.GetUncommittedChanges().Single();

            Assert.Equal(creationRequested.CorrelationId, pexaWorkspaceCreationFailed.CorrelationId);
            Assert.Equal(creationRequested.EventId, pexaWorkspaceCreationFailed.CausationId);
        }
    }
}
