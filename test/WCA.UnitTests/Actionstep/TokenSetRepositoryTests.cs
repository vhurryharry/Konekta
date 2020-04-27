using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NodaTime;
using NodaTime.Testing;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WCA.Actionstep.Client.Resources;
using WCA.Core.Account;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Data;
using WCA.Domain.Actionstep;
using WCA.Domain.Models.Account;
using WCA.UnitTests.TestInfrastructure;
using Xunit;

namespace WCA.UnitTests.Actionstep
{
    public class TokenSetRepositoryTests
    {
        [Fact]
        public async Task GetTokensByRefreshExpiryReturnsCorrectItems()
        {
            var options = new DbContextOptionsBuilder<WCADbContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetTokensByRefreshExpiryReturnsCorrectItems))
                .Options;

            var clock = FakeClock.FromUtc(2019, 10, 1, 8, 0, 0);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(f => f.CreateScope()).Returns(() =>
            {
                var serviceScope = new Mock<IServiceScope>();
                serviceScope.Setup(s => s.ServiceProvider.GetService(It.IsAny<Type>())).Returns(new WCADbContextTransient(options));
                return serviceScope.Object;
            });

            // Run the test against one instance of the context
            using (var context = new WCADbContextTransient(options))
            {
                var entityEntry = context.Users.Add(new WCAUser());
                await context.SaveChangesAsync();

                var service1 = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);

                /// Expiry will be 21 days after <see cref="TokenSet.ReceivedAt"/>.
                /// These two should expire on 22 Oct 2019
                /// Org keys must be different because only one set of credentials can be stored per user+org combination.
                await service1.AddOrUpdateTokenSet(new TokenSet("at", "testToken0", 3600, new Uri("https://uri/"), "org0", "rt", Instant.FromUtc(2019, 10, 1, 8, 0), entityEntry.Entity.Id));
                await service1.AddOrUpdateTokenSet(new TokenSet("at", "testToken1", 3600, new Uri("https://uri/"), "org1", "rt", Instant.FromUtc(2019, 10, 1, 8, 0), entityEntry.Entity.Id));

                /// These two should expire on 23 Oct 2019
                await service1.AddOrUpdateTokenSet(new TokenSet("at", "testToken2", 3600, new Uri("https://uri/"), "org2", "rt", Instant.FromUtc(2019, 10, 2, 8, 0), entityEntry.Entity.Id));
                await service1.AddOrUpdateTokenSet(new TokenSet("at", "testToken3", 3600, new Uri("https://uri/"), "org3", "rt", Instant.FromUtc(2019, 10, 2, 8, 0), entityEntry.Entity.Id));
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            var service2 = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);
            var tokensAboutToExpire = await service2.GetTokensByRefreshExpiry(Instant.FromUtc(2019, 10, 22, 9, 0));
            Assert.Equal(2, tokensAboutToExpire.Count());
            Assert.NotNull(tokensAboutToExpire.Single(t => t.TokenType.Equals("testToken0", StringComparison.InvariantCulture)));
            Assert.NotNull(tokensAboutToExpire.Single(t => t.TokenType.Equals("testToken1", StringComparison.InvariantCulture)));
        }

        [Fact]
        public async Task AddOrUpdateTokenSetThrowsIfOwnerNotFound()
        {
            var options = new DbContextOptionsBuilder<WCADbContext>()
                .UseInMemoryDatabase(databaseName: nameof(AddOrUpdateTokenSetThrowsIfOwnerNotFound))
                .Options;

            var clock = FakeClock.FromUtc(2019, 10, 1, 8, 0, 0);

            // Run the test against one instance of the context
            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(f => f.CreateScope()).Returns(() =>
            {
                var serviceScope = new Mock<IServiceScope>();
                serviceScope.Setup(s => s.ServiceProvider.GetService(It.IsAny<Type>())).Returns(new WCADbContextTransient(options));
                return serviceScope.Object;
            });

            var service = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);

            var ex = await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            {
                await service.AddOrUpdateTokenSet(new TokenSet("at", "testToken0", 3600, new Uri("https://uri/"), "org0", "rt", Instant.FromUtc(2019, 10, 1, 8, 0), userId: "doesNotExistInDb"));
            });

            Assert.Equal("Couldn't find user attempting to add or update TokenSet. (User ID: 'doesNotExistInDb')", ex.Message);
        }

        [Fact]
        public async Task GetTokenWithSubstitution()
        {
            var options = new DbContextOptionsBuilder<WCADbContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetTokenWithSubstitution))
                .Options;

            var clock = FakeClock.FromUtc(2019, 10, 1, 8, 0, 0);
            var user1Id = string.Empty;

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(f => f.CreateScope()).Returns(() =>
            {
                var serviceScope = new Mock<IServiceScope>();
                serviceScope.Setup(s => s.ServiceProvider.GetService(It.IsAny<Type>())).Returns(new WCADbContextTransient(options));
                return serviceScope.Object;
            });

            // Run the test against one instance of the context
            using (var context = new WCADbContextTransient(options))
            {
                var entityEntry1 = context.Users.Add(new WCAUser() { Email = "user1@domain" });
                var entityEntry2 = context.Users.Add(new WCAUser() { Email = "user2@domain" });
                await context.SaveChangesAsync();
                user1Id = entityEntry1.Entity.Id;
                var startTime = DateTime.UtcNow;

                context.ActionstepCredentialSubstitutions.Add(new ActionstepCredentialSubstitution()
                {
                    CreatedBy = entityEntry1.Entity,
                    ForOwner = entityEntry1.Entity,
                    SubstituteWithOwner = entityEntry2.Entity
                });
                context.SaveChanges();

                // Will be disposed of in implementation
                var service1 = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);

                // Create tokens for each user
                await service1.AddOrUpdateTokenSet(new TokenSet("at", "testToken0", 3600, new Uri("https://uri/"), "org", "rt", Instant.FromUtc(2019, 10, 1, 8, 0), entityEntry1.Entity.Id));
                await service1.AddOrUpdateTokenSet(new TokenSet("at", "testToken1", 3600, new Uri("https://uri/"), "org", "rt", Instant.FromUtc(2019, 10, 1, 8, 0), entityEntry2.Entity.Id));
            }

            // Use a separate instance of the context to verify correct data was saved to database
            var service2 = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);
            var substitutedToken = await service2.GetTokenSet(new TokenSetQuery(user1Id, "org"));

            // Should return second token which was substituted instead of first
            Assert.Equal("testToken1", substitutedToken?.TokenType);
        }

        [Fact]
        public async Task CanAddTokenSet()
        {
            var options = new DbContextOptionsBuilder<WCADbContext>()
                .UseInMemoryDatabase(databaseName: nameof(CanAddTokenSet))
                .Options;

            var clock = FakeClock.FromUtc(2019, 10, 1, 8, 0, 0);
            var tokenSetId = string.Empty;

            // Run the test against one instance of the context
            using (var context = new WCADbContextTransient(options))
            {
                var entityEntry1 = context.Users.Add(new WCAUser() { Email = "user1@domain" });
                await context.SaveChangesAsync();
                var startTime = DateTime.UtcNow;

                var serviceScopeFactory = new Mock<IServiceScopeFactory>();
                serviceScopeFactory.Setup(f => f.CreateScope()).Returns(() =>
                {
                    var serviceScope = new Mock<IServiceScope>();
                    serviceScope.Setup(s => s.ServiceProvider.GetService(It.IsAny<Type>())).Returns(new WCADbContextTransient(options));
                    return serviceScope.Object;
                });

                var tokenSetRepository = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);

                // Create token to test retrieval
                var addedTokenSet = await tokenSetRepository.AddOrUpdateTokenSet(new TokenSet(nameof(CanAddTokenSet), "testToken", 3600, new Uri("https://uri/"), "org", "rt", Instant.FromUtc(2019, 10, 1, 8, 0), entityEntry1.Entity.Id));

                var savedCredential = context.ActionstepCredentials.AsNoTracking().ToArray()[0];

                Assert.Equal(nameof(CanAddTokenSet), savedCredential.AccessToken);
            }
        }

        [Fact]
        public async Task CanUpdateTokenSetById()
        {
            var options = new DbContextOptionsBuilder<WCADbContext>()
                .UseInMemoryDatabase(databaseName: nameof(CanUpdateTokenSetById))
                .Options;

            var clock = FakeClock.FromUtc(2019, 10, 1, 8, 0, 0);
            var now = clock.GetCurrentInstant();
            var tokenSetId = string.Empty;

            // Run the test against one instance of the context
            using (var context = new WCADbContextTransient(options))
            {
                var createdByEntityEntry = context.Users.Add(new WCAUser() { Email = "createdBy@domain" });
                var updatedByEntityEntry = context.Users.Add(new WCAUser() { Email = "updatedBy@domain" });
                var ownerEntityEntry = context.Users.Add(new WCAUser() { Email = "owner@domain" });
                var EXPECTED_DATE_CREATED = now.PlusTicks(100).ToDateTimeUtc();

                var credentialEntityEntry = context.ActionstepCredentials.Add(new ActionstepCredential()
                {
                    AccessToken = "AccessToken-Before",
                    RefreshToken = "RefreshToken-Before",
                    TokenType = "TokenType-Before",
                    ExpiresIn = 3600,
                    ReceivedAtUtc = now.PlusTicks(300).ToDateTimeUtc(),
                    AccessTokenExpiryUtc = now.PlusTicks(400).ToDateTimeUtc(),
                    RefreshTokenExpiryUtc = now.PlusTicks(500).ToDateTimeUtc(),
                    ActionstepOrg = new ActionstepOrg() { Key = "ActionstepOrg" },
                    ApiEndpoint = new Uri("https://uri/api-endpoint-before-update"),
                    CreatedBy = createdByEntityEntry.Entity,
                    UpdatedBy = createdByEntityEntry.Entity,
                    Owner = ownerEntityEntry.Entity,
                });

                await context.SaveChangesAsync();

                var serviceScopeFactory = new Mock<IServiceScopeFactory>();
                serviceScopeFactory.Setup(f => f.CreateScope()).Returns(() =>
                {
                    var serviceScope = new Mock<IServiceScope>();
                    serviceScope.Setup(s => s.ServiceProvider.GetService(It.IsAny<Type>())).Returns(new WCADbContextTransient(options));
                    return serviceScope.Object;
                });

                var tokenSetRepository = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);

                var EXPECTED_ACCESS_TOKEN = "AccessToken-After";
                var EXPECTED_TOKEN_TYPE = "TokenType-After";
                var EXPECTED_EXPIRES_IN = 4600;
                var EXPECTED_API_ENDPOINT = new Uri("https://uri/api-endpoint-after-update");
                var EXPECTED_ACTIONSTEP_ORG = "ActionstepOrg";
                var EXPECTED_REFRESH_TOKEN = "RefreshToken-After";
                var EXPECTED_RECEIVED_AT = now.Plus(Duration.FromDays(1));

                const int DEFAULT_REFRESH_TOKEN_EXPIRY_DAYS = 21;
                var EXPECTED_REFRESH_TOKEN_EXPIRY = EXPECTED_RECEIVED_AT.Plus(Duration.FromDays(DEFAULT_REFRESH_TOKEN_EXPIRY_DAYS)).ToDateTimeUtc();

                var EXPECTED_ACCESS_TOKEN_EXPIRY = EXPECTED_RECEIVED_AT.Plus(Duration.FromSeconds(EXPECTED_EXPIRES_IN)).ToDateTimeUtc();
                var EXPECTED_LOCK_EXPIRES_AT = DateTime.MinValue;
                var EXPECTED_LOCK_ID = Guid.Empty;
                var EXPECTED_USER_ID = ownerEntityEntry.Entity.Id;

                // Updated by should change to user - as supplied to AddOrUpdateTokenSet
                var EXPECTED_UPDATED_BY = ownerEntityEntry.Entity;
                var TOKEN_SET_ID = credentialEntityEntry.Entity.Id;

                // Create token to test retrieval
                var addedTokenSet = await tokenSetRepository.AddOrUpdateTokenSet(
                    new TokenSet(
                        EXPECTED_ACCESS_TOKEN,
                        EXPECTED_TOKEN_TYPE,
                        EXPECTED_EXPIRES_IN,
                        EXPECTED_API_ENDPOINT,
                        EXPECTED_ACTIONSTEP_ORG,
                        EXPECTED_REFRESH_TOKEN,
                        EXPECTED_RECEIVED_AT,
                        EXPECTED_USER_ID,
                        TOKEN_SET_ID.ToString(CultureInfo.InvariantCulture)));

                var updatedCredential = context.ActionstepCredentials
                    .AsNoTracking()
                    .Include(c => c.ActionstepOrg)
                    .Include(c => c.Owner)
                    .Include(c => c.CreatedBy)
                    .Include(c => c.UpdatedBy)
                    .Single();

                Assert.Equal(EXPECTED_ACCESS_TOKEN, updatedCredential.AccessToken);
                Assert.Equal(EXPECTED_ACCESS_TOKEN_EXPIRY, updatedCredential.AccessTokenExpiryUtc);
                Assert.Equal(EXPECTED_ACTIONSTEP_ORG, updatedCredential.ActionstepOrg.Key);
                Assert.Equal(EXPECTED_API_ENDPOINT, updatedCredential.ApiEndpoint);
                Assert.Equal(createdByEntityEntry.Entity.Id, updatedCredential.CreatedBy.Id);
                Assert.Equal(EXPECTED_EXPIRES_IN, updatedCredential.ExpiresIn);
                Assert.Equal(TOKEN_SET_ID, updatedCredential.Id);
                Assert.Null(updatedCredential.IdToken);
                Assert.Equal(EXPECTED_LOCK_EXPIRES_AT, updatedCredential.LockExpiresAtUtc);
                Assert.Equal(EXPECTED_LOCK_ID, updatedCredential.LockId);
                Assert.Equal(ownerEntityEntry.Entity.Id, updatedCredential.Owner.Id);
                Assert.Equal(EXPECTED_RECEIVED_AT.ToDateTimeUtc(), updatedCredential.ReceivedAtUtc);
                Assert.Equal(EXPECTED_REFRESH_TOKEN, updatedCredential.RefreshToken);
                Assert.Equal(EXPECTED_REFRESH_TOKEN_EXPIRY, updatedCredential.RefreshTokenExpiryUtc);
                Assert.Equal(EXPECTED_TOKEN_TYPE, updatedCredential.TokenType);
                Assert.Equal(EXPECTED_UPDATED_BY.Id, updatedCredential.UpdatedBy.Id);
            }
        }

        [Fact]
        public async Task CanUpdateTokenSetByUserAndOrg()
        {
            var options = new DbContextOptionsBuilder<WCADbContext>()
                            .UseInMemoryDatabase(databaseName: nameof(CanUpdateTokenSetByUserAndOrg))
                            .Options;

            var clock = FakeClock.FromUtc(2019, 10, 1, 8, 0, 0);
            var now = clock.GetCurrentInstant();
            var tokenSetId = string.Empty;

            // Run the test against one instance of the context
            using (var context = new WCADbContextTransient(options))
            {
                var createdByEntityEntry = context.Users.Add(new WCAUser() { Email = "createdBy@domain" });
                var updatedByEntityEntry = context.Users.Add(new WCAUser() { Email = "updatedBy@domain" });
                var ownerEntityEntry = context.Users.Add(new WCAUser() { Email = "owner@domain" });
                var EXPECTED_DATE_CREATED = now.PlusTicks(100).ToDateTimeUtc();

                var credentialEntityEntry = context.ActionstepCredentials.Add(new ActionstepCredential()
                {
                    AccessToken = "AccessToken-Before",
                    RefreshToken = "RefreshToken-Before",
                    TokenType = "TokenType-Before",
                    ExpiresIn = 3600,
                    ReceivedAtUtc = now.PlusTicks(300).ToDateTimeUtc(),
                    AccessTokenExpiryUtc = now.PlusTicks(400).ToDateTimeUtc(),
                    RefreshTokenExpiryUtc = now.PlusTicks(500).ToDateTimeUtc(),
                    ActionstepOrg = new ActionstepOrg() { Key = "ActionstepOrg" },
                    ApiEndpoint = new Uri("https://uri/api-endpoint-before-update"),
                    CreatedBy = createdByEntityEntry.Entity,
                    UpdatedBy = createdByEntityEntry.Entity,
                    Owner = ownerEntityEntry.Entity,
                });

                await context.SaveChangesAsync();

                var serviceScopeFactory = new Mock<IServiceScopeFactory>();
                serviceScopeFactory.Setup(f => f.CreateScope()).Returns(() =>
                {
                    var serviceScope = new Mock<IServiceScope>();
                    serviceScope.Setup(s => s.ServiceProvider.GetService(It.IsAny<Type>())).Returns(new WCADbContextTransient(options));
                    return serviceScope.Object;
                });

                var tokenSetRepository = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);

                var EXPECTED_ACCESS_TOKEN = "AccessToken-After";
                var EXPECTED_TOKEN_TYPE = "TokenType-After";
                var EXPECTED_EXPIRES_IN = 4600;
                var EXPECTED_API_ENDPOINT = new Uri("https://uri/api-endpoint-after-update");
                var EXPECTED_ACTIONSTEP_ORG = "ActionstepOrg";
                var EXPECTED_REFRESH_TOKEN = "RefreshToken-After";
                var EXPECTED_RECEIVED_AT = now.Plus(Duration.FromDays(1));

                const int DEFAULT_REFRESH_TOKEN_EXPIRY_DAYS = 21;
                var EXPECTED_REFRESH_TOKEN_EXPIRY = EXPECTED_RECEIVED_AT.Plus(Duration.FromDays(DEFAULT_REFRESH_TOKEN_EXPIRY_DAYS)).ToDateTimeUtc();

                var EXPECTED_LAST_UPDATED = clock.GetCurrentInstant().ToDateTimeUtc();
                var EXPECTED_ACCESS_TOKEN_EXPIRY = EXPECTED_RECEIVED_AT.Plus(Duration.FromSeconds(EXPECTED_EXPIRES_IN)).ToDateTimeUtc();
                var EXPECTED_LOCK_EXPIRES_AT = DateTime.MinValue;
                var EXPECTED_LOCK_ID = Guid.Empty;
                var EXPECTED_USER_ID = ownerEntityEntry.Entity.Id;

                // Updated by should change to user - as supplied to AddOrUpdateTokenSet
                var EXPECTED_UPDATED_BY = ownerEntityEntry.Entity;
                var TOKEN_SET_ID = credentialEntityEntry.Entity.Id;

                // Create token to test retrieval
                var addedTokenSet = await tokenSetRepository.AddOrUpdateTokenSet(
                    new TokenSet(
                        EXPECTED_ACCESS_TOKEN,
                        EXPECTED_TOKEN_TYPE,
                        EXPECTED_EXPIRES_IN,
                        EXPECTED_API_ENDPOINT,
                        EXPECTED_ACTIONSTEP_ORG,
                        EXPECTED_REFRESH_TOKEN,
                        EXPECTED_RECEIVED_AT,
                        EXPECTED_USER_ID));

                var updatedCredential = context.ActionstepCredentials
                    .AsNoTracking()
                    .Include(c => c.ActionstepOrg)
                    .Include(c => c.Owner)
                    .Include(c => c.CreatedBy)
                    .Include(c => c.UpdatedBy)
                    .Single();

                Assert.Equal(EXPECTED_ACCESS_TOKEN, updatedCredential.AccessToken);
                Assert.Equal(EXPECTED_ACCESS_TOKEN_EXPIRY, updatedCredential.AccessTokenExpiryUtc);
                Assert.Equal(EXPECTED_ACTIONSTEP_ORG, updatedCredential.ActionstepOrg.Key);
                Assert.Equal(EXPECTED_API_ENDPOINT, updatedCredential.ApiEndpoint);
                Assert.Equal(createdByEntityEntry.Entity.Id, updatedCredential.CreatedBy.Id);
                Assert.Equal(EXPECTED_EXPIRES_IN, updatedCredential.ExpiresIn);
                Assert.Equal(TOKEN_SET_ID, updatedCredential.Id);
                Assert.Null(updatedCredential.IdToken);
                Assert.Equal(EXPECTED_LOCK_EXPIRES_AT, updatedCredential.LockExpiresAtUtc);
                Assert.Equal(EXPECTED_LOCK_ID, updatedCredential.LockId);
                Assert.Equal(ownerEntityEntry.Entity.Id, updatedCredential.Owner.Id);
                Assert.Equal(EXPECTED_RECEIVED_AT.ToDateTimeUtc(), updatedCredential.ReceivedAtUtc);
                Assert.Equal(EXPECTED_REFRESH_TOKEN, updatedCredential.RefreshToken);
                Assert.Equal(EXPECTED_REFRESH_TOKEN_EXPIRY, updatedCredential.RefreshTokenExpiryUtc);
                Assert.Equal(EXPECTED_TOKEN_TYPE, updatedCredential.TokenType);
                Assert.Equal(EXPECTED_UPDATED_BY.Id, updatedCredential.UpdatedBy.Id);
            }
        }

        [Fact]
        public async Task CanGetTokenById()
        {
            var options = new DbContextOptionsBuilder<WCADbContext>()
                .UseInMemoryDatabase(databaseName: nameof(CanGetTokenById))
                .Options;

            var clock = FakeClock.FromUtc(2019, 10, 1, 8, 0, 0);
            var user1Id = string.Empty;
            var tokenSetId = string.Empty;

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(f => f.CreateScope()).Returns(() =>
            {
                var serviceScope = new Mock<IServiceScope>();
                serviceScope.Setup(s => s.ServiceProvider.GetService(It.IsAny<Type>())).Returns(new WCADbContextTransient(options));
                return serviceScope.Object;
            });

            // Run the test against one instance of the context
            using (var context = new WCADbContextTransient(options))
            {
                var entityEntry1 = context.Users.Add(new WCAUser() { Email = "user1@domain" });
                await context.SaveChangesAsync();
                user1Id = entityEntry1.Entity.Id;
                var startTime = DateTime.UtcNow;

                var service1 = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);

                // Create token to test retrieval
                var addedTokenSet = await service1.AddOrUpdateTokenSet(new TokenSet("at", "testToken", 3600, new Uri("https://uri/"), "org", "rt", Instant.FromUtc(2019, 10, 1, 8, 0), entityEntry1.Entity.Id));
                tokenSetId = addedTokenSet.Id;
            }

            // Use a separate instance of the context to verify correct data was saved to database
            var service2 = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);
            var retrievedTokenSet = await service2.GetTokenSetById(tokenSetId);

            // Should return second token which was substituted instead of first
            Assert.Equal("testToken", retrievedTokenSet.TokenType);
        }

        [Fact]
        public async Task GetTokenReturnsNullIfNotFound()
        {
            var options = new DbContextOptionsBuilder<WCADbContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetTokenReturnsNullIfNotFound))
                .Options;

            var clock = FakeClock.FromUtc(2019, 10, 1, 8, 0, 0);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(f => f.CreateScope()).Returns(() =>
            {
                var serviceScope = new Mock<IServiceScope>();
                serviceScope.Setup(s => s.ServiceProvider.GetService(It.IsAny<Type>())).Returns(new WCADbContextTransient(options));
                return serviceScope.Object;
            });

            var service = new TokenSetRepository(new TestTelemetryLogger(), serviceScopeFactory.Object, clock);
            var tokenShouldntBeFound = await service.GetTokenSet(new TokenSetQuery("User doesn't exist", "Org doesn't exist"));
            Assert.Null(tokenShouldntBeFound);
        }
    }
}
