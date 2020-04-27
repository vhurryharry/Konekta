using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core;
using WCA.Core.Features.InfoTrack;
using WCA.Core.Services.Email;
using WCA.Data;
using WCA.Domain.Actionstep;
using WCA.UnitTests.TestInfrastructure;
using Xunit;
using static WCA.Core.Features.InfoTrack.StoreInfoTrackCredentialsForOrg;

namespace WCA.UnitTests.InfoTrack
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class StoreInfoTrackCredentialsForOrgTests
    {
        private readonly WebContainerFixture _containerFixture;

        public StoreInfoTrackCredentialsForOrgTests(WebContainerFixture containerFixture)
        {
            _containerFixture = containerFixture;
        }

        [Fact]
        public async void CredentialsCanBeStored()
        {
            await _containerFixture.ExecuteScopeAsync(async serviceProvider =>
            {
                var mockEmailSender = new Mock<IEmailSender>();
                var db = serviceProvider.GetService<WCADbContext>();

                // Prepare test data
                var testUser = db.GetTestUser();
                var testOrg = db.ActionstepOrgs.Add(new ActionstepOrg()
                {
                    Key = "CredentialsCanBeStored",
                    Title = "CredentialsCanBeStored",
                    CreatedBy = testUser,
                    UpdatedBy = testUser,
                    DateCreatedUtc = DateTime.Parse("2018-10-06", CultureInfo.InvariantCulture),
                    LastUpdatedUtc = DateTime.Parse("2018-10-06", CultureInfo.InvariantCulture),
                }).Entity;
                await db.SaveChangesAsync();

                var startTime = DateTime.UtcNow;

                // Store test data
                var testCredential = db.ActionstepCredentials.Add(new ActionstepCredential()
                {
                    AccessToken = "CredentialsCanBeStored-AccessToken",
                    AccessTokenExpiryUtc = startTime.AddHours(1),
                    Owner = testUser,
                    ActionstepOrg = testOrg,
                    RefreshToken = "CredentialsCanBeStored-RefreshToken",
                    RefreshTokenExpiryUtc = startTime.AddMinutes(5),
                    TokenType = "CredentialsCanBeStored-TokenType",
                    ApiEndpoint = new Uri("https://api.endpoint/api/"),
                    ExpiresIn = 3600,
                    ReceivedAtUtc = startTime,
                    CreatedBy = testUser,
                    UpdatedBy = testUser,
                    DateCreatedUtc = DateTime.Parse("2018-10-06", CultureInfo.InvariantCulture),
                    LastUpdatedUtc = DateTime.Parse("2018-10-06", CultureInfo.InvariantCulture)
                }).Entity;

                await db.SaveChangesAsync();

                var coreOptions = Options.Create(new WCACoreSettings()
                {
                    ActionstepSettings = new ActionstepSettings()
                    {
                        ApiClientId = "ApiClientId",
                        ApiClientSecret = "ApiClientSecret"
                    },
                    InfoTrackSettings = new InfoTrackSettings()
                    {
                        NewInfoTrackEmailNotifications = "infotrackemail@test"
                    },
                    WCANotificationEmail = "wcanotifications@test"
                });


                var testInfoTrackCredentialService = new TestInfoTrackCredentialRepository();

                var handler = (IRequestHandler<StoreInfoTrackCredentialsForOrgCommand>)new StoreInfoTrackCredentialsForOrg.Handler(
                    db,
                    new Validator(),
                    coreOptions,
                    testInfoTrackCredentialService,
                    mockEmailSender.Object);

                // Act
                var expectedUsername = "testUsername";
                var expectedPassword = "testPassword";
                await handler.Handle(new StoreInfoTrackCredentialsForOrgCommand()
                {
                    ActionstepOrgKey = testOrg.Key,
                    AuthenticatedUser = testUser,
                    InfoTrackUsername = expectedUsername,
                    InfoTrackPassword = expectedPassword
                },
                new CancellationToken());

                // Assert
                mockEmailSender.Verify(s => s.SendEmailAsync(It.IsAny<EmailSenderRequest>()), Times.Once);
                var result = await testInfoTrackCredentialService.FindCredential(testOrg.Key);
                Assert.Equal(expectedUsername, result.Username);
                Assert.Equal(expectedPassword, result.Password);
            });
        }

        [Fact]
        public async void IfUserDoesntHaveCredentialsUnauthorizedExceptionIsThrown()
        {
            // We can't use Assert.Throws because we're running inside of ExecuteScopeAsync
            var unauthorizedAccessExceptionThrown = false;

            await _containerFixture.ExecuteScopeAsync(serviceProvider =>
            {
                var mockEmailSender = new Mock<IEmailSender>();
                var db = serviceProvider.GetService<WCADbContext>();

                // Prepare test data
                var testUser = db.GetTestUser();
                var coreOptions = Options.Create(new WCACoreSettings()
                {
                    ActionstepSettings = new ActionstepSettings()
                    {
                        ApiClientId = "ApiClientId",
                        ApiClientSecret = "ApiClientSecret"
                    }
                });

                var handler = new Handler(
                    db,
                    new Validator(),
                    coreOptions,
                    null,
                    mockEmailSender.Object);

                var handleMethod = handler.GetType().GetMethod(
                    "Handle",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                // Act
                try
                {
                    Task task = (Task)handleMethod.Invoke(handler, new object[]
                    {
                        new StoreInfoTrackCredentialsForOrgCommand()
                        {
                            ActionstepOrgKey = "OrgKeyThatDoesntExist",
                            AuthenticatedUser = testUser,
                            InfoTrackUsername = "DummyUsername",
                            InfoTrackPassword = "DummyPassword"
                        },
                        new CancellationToken()
                    });
                    task.Wait();

                }
                catch (AggregateException aggregateException)
                {
                    // Because of the async stuff, the UnauthorizedAccessException
                    // is wrapped in an AggregateException
                    if (aggregateException.InnerException is UnauthorizedAccessException)
                    {
                        unauthorizedAccessExceptionThrown = true;
                    }
                }

                return Task.CompletedTask;
            });

            Assert.True(unauthorizedAccessExceptionThrown);
        }

        [Fact]
        public async void IfUsersCredentialsRefreshTokanHasExpiredUnauthorizedExceptionIsThrown()
        {
            // We can't use Assert.Throws because we're running inside of
            // ExecuteScopeAsync
            var unauthorizedAccessExceptionThrown = false;

            await _containerFixture.ExecuteScopeAsync(async serviceProvider =>
            {
                var mockEmailSender = new Mock<IEmailSender>();
                var db = serviceProvider.GetService<WCADbContext>();

                // Prepare test data
                var testUser = db.GetTestUser();
                var testOrg = db.ActionstepOrgs.Add(new ActionstepOrg()
                {
                    Key = "IfUsersCredentialsRefreshTokanHasExpiredUnauthorizedExceptionIsThrown",
                    Title = "IfUsersCredentialsRefreshTokanHasExpiredUnauthorizedExceptionIsThrown",
                    CreatedBy = testUser,
                    UpdatedBy = testUser,
                    DateCreatedUtc = DateTime.Parse("2018-10-06", CultureInfo.InvariantCulture),
                    LastUpdatedUtc = DateTime.Parse("2018-10-06", CultureInfo.InvariantCulture),
                }).Entity;
                await db.SaveChangesAsync();

                var startTime = DateTime.UtcNow;

                // Store test data
                var testCredential = db.ActionstepCredentials.Add(new ActionstepCredential()
                {
                    AccessToken = "IfUsersCredentialsRefreshTokanHasExpiredUnauthorizedExceptionIsThrown-AccessToken",
                    AccessTokenExpiryUtc = startTime.AddHours(1),
                    Owner = testUser,
                    ActionstepOrg = testOrg,
                    RefreshToken = "IfUsersCredentialsRefreshTokanHasExpiredUnauthorizedExceptionIsThrown-RefreshToken",

                    // Test expired refresh token, so this date must be in the past
                    // as the handler checks against DateTime.UtcNow
                    RefreshTokenExpiryUtc = startTime.Subtract(TimeSpan.FromMinutes(5)),

                    TokenType = "IfUsersCredentialsRefreshTokanHasExpiredUnauthorizedExceptionIsThrown-TokenType",
                    ApiEndpoint = new Uri("https://api.endpoint/api/"),
                    ExpiresIn = 3600,
                    ReceivedAtUtc = startTime,
                    CreatedBy = testUser,
                    UpdatedBy = testUser,
                    DateCreatedUtc = DateTime.Parse("2018-10-06", CultureInfo.InvariantCulture),
                    LastUpdatedUtc = DateTime.Parse("2018-10-06", CultureInfo.InvariantCulture)
                }).Entity;

                await db.SaveChangesAsync();

                var coreOptions = Options.Create(new WCACoreSettings()
                {
                    ActionstepSettings = new ActionstepSettings()
                    {
                        ApiClientId = "ApiClientId",
                        ApiClientSecret = "ApiClientSecret"
                    }
                });

                var handler = new Handler(
                    db,
                    new Validator(),
                    coreOptions,
                    null,
                    mockEmailSender.Object);

                var handleMethod = handler.GetType().GetMethod(
                    "Handle",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                // Act 
                try
                {
                    Task task = (Task)handleMethod.Invoke(handler, new object[]
                    {
                        new StoreInfoTrackCredentialsForOrgCommand()
                        {
                            ActionstepOrgKey = "IfUsersCredentialsRefreshTokanHasExpiredUnauthorizedExceptionIsThrown",
                            AuthenticatedUser = testUser,
                            InfoTrackUsername = "DummyUsername",
                            InfoTrackPassword = "DummyPassword"
                        },
                        new CancellationToken()
                    });
                    task.Wait();

                }
                catch (AggregateException aggregateException)
                {
                    // Because of the async stuff, the UnauthorizedAccessException
                    // is wrapped in an AggregateException
                    if (aggregateException.InnerException is UnauthorizedAccessException)
                    {
                        unauthorizedAccessExceptionThrown = true;
                    }
                }
            });

            Assert.True(unauthorizedAccessExceptionThrown);
        }
    }
}
