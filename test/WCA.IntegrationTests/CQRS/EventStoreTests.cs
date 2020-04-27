using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Testing;
using System;
using System.Threading.Tasks;
using WCA.Domain.CQRS;
using WCA.IntegrationTests.TestInfrastructure;
using Xunit;

namespace WCA.IntegrationTests.CQRS
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class EventStoreTests
    {
        private readonly WebContainerFixture _containerFixture;

        public EventStoreTests(WebContainerFixture containerFixture)
        {
            _containerFixture = containerFixture;
        }

        private class TestEvent : EventBase
        {
            public TestEvent(Instant eventCreatedAt)
                : base(eventCreatedAt)
            { }

            public string SomeTestValue { get; set; }
        }

        [Fact]
        public void EventHasEventIdSet()
        {
            var fakeClock = FakeClock.FromUtc(2019, 8, 20);
            fakeClock.AutoAdvance = Duration.FromSeconds(1);

            var testEvent = new TestEvent(fakeClock.GetCurrentInstant())
            {
                SomeTestValue = "Test Value 1"
            };

            Assert.NotEqual(Guid.Empty, testEvent.EventId);
        }

        [Fact]
        public async Task EventIdIsPersisted()
        {
            await _containerFixture.ExecuteScopeAsync(serviceProvider =>
            {
                var eventStore = serviceProvider.GetService<IEventStore>();
                var fakeClock = FakeClock.FromUtc(2019, 8, 20);
                fakeClock.AutoAdvance = Duration.FromSeconds(1);

                var aggregateId = "64DF4C71-FC0A-45B4-91FE-E5393A68E7F3";

                var testEvent = new TestEvent(fakeClock.GetCurrentInstant())
                {
                    SomeTestValue = "Test Value 1"
                };

                eventStore.SaveEvents(aggregateId, new[] { testEvent }, 0);

                var retrievedEvents = eventStore.GetEventsForAggregate(aggregateId);

                Assert.Equal(testEvent.EventId, (retrievedEvents[0] as TestEvent).EventId);

                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Ensures timestamp is serialised correctly as we're using NodaTime.
        /// In other words, makes sure that NodaTime serialisation is configured correctly for events.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task EventCreatedAtIsPersisted()
        {
            await _containerFixture.ExecuteScopeAsync(serviceProvider =>
            {
                var eventStore = serviceProvider.GetService<IEventStore>();
                var createdAt = Instant.FromUtc(2019, 8, 20, 1, 2);

                var aggregateId = "D509CD73-5001-4DD6-9EB0-96648F06A858";

                var testEvent = new TestEvent(createdAt)
                {
                    SomeTestValue = "Test Value 1"
                };

                eventStore.SaveEvents(aggregateId, new[] { testEvent }, 0);

                var retrievedEvents = eventStore.GetEventsForAggregate(aggregateId);

                Assert.Equal(createdAt, (retrievedEvents[0] as TestEvent).EventCreatedAt);

                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task CanSaveAndReadSingleEventForAggregate()
        {
            await _containerFixture.ExecuteScopeAsync(serviceProvider =>
            {
                var eventStore = serviceProvider.GetService<IEventStore>();
                var fakeClock = FakeClock.FromUtc(2019, 8, 20);
                fakeClock.AutoAdvance = Duration.FromSeconds(1);

                var aggregateId = "88E14BDE-9559-4A42-9535-0DF49CB9CBED";

                var testEvent = new TestEvent(fakeClock.GetCurrentInstant())
                {
                    SomeTestValue = "Test Value 1"
                };

                eventStore.SaveEvents(aggregateId, new[] { testEvent }, 0);

                var retrievedEvents = eventStore.GetEventsForAggregate(aggregateId);

                Assert.Equal(testEvent.SomeTestValue, (retrievedEvents[0] as TestEvent).SomeTestValue);

                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task RetrievedEventsAreVersionedCorrectly()
        {
            await _containerFixture.ExecuteScopeAsync(serviceProvider =>
            {
                var eventStore = serviceProvider.GetService<IEventStore>();
                var fakeClock = FakeClock.FromUtc(2019, 8, 20);
                fakeClock.AutoAdvance = Duration.FromSeconds(1);

                var aggregateId = "3B2358C4-FC85-4EAB-870F-5B908DD8B8A5";

                var testEvent0 = new TestEvent(fakeClock.GetCurrentInstant());
                var testEvent1 = new TestEvent(fakeClock.GetCurrentInstant());
                var testEvent2 = new TestEvent(fakeClock.GetCurrentInstant());

                eventStore.SaveEvents(aggregateId, new[] {
                    testEvent0,
                    testEvent1,
                    testEvent2
                });

                var retrievedEvents = eventStore.GetEventsForAggregate(aggregateId);

                Assert.Equal(0, retrievedEvents[0].Version);
                Assert.Equal(1, retrievedEvents[1].Version);
                Assert.Equal(2, retrievedEvents[2].Version);

                return Task.CompletedTask;
            });
        }
    }
}
