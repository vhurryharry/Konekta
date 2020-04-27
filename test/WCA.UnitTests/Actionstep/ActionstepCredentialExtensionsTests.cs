using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Testing;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
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
    public class ActionstepCredentialExtensionsTests
    {
        [Fact]
        public void ActionstepCredentialToTokenSetWithLock()
        {
            var testStartTime = Instant.FromUtc(2019, 10, 27, 0, 0);
            var actionstepCredential = CreateTestCredential(testStartTime, withLock: true);

            var tokenSet = actionstepCredential.ToTokenSet();

            Assert.Equal(actionstepCredential.AccessToken, tokenSet.AccessToken);
            Assert.Equal(actionstepCredential.ActionstepOrg.Key, tokenSet.OrgKey);
            Assert.Equal(actionstepCredential.ApiEndpoint, tokenSet.ApiEndpoint);
            Assert.Equal(actionstepCredential.ExpiresIn, tokenSet.ExpiresIn);
            Assert.Equal(actionstepCredential.Id.ToString(CultureInfo.InvariantCulture), tokenSet.Id);
            Assert.Equal(actionstepCredential.IdToken, tokenSet.IdToken);
            Assert.Equal(actionstepCredential.LockExpiresAtUtc, tokenSet.RefreshLockInfo.LockExpiresAt.ToDateTimeUtc());
            Assert.Equal(actionstepCredential.LockId, tokenSet.RefreshLockInfo.LockId);
            Assert.Equal(actionstepCredential.Owner.Id, tokenSet.UserId);
            Assert.Equal(actionstepCredential.ReceivedAtUtc, tokenSet.ReceivedAt.ToDateTimeUtc());
            Assert.Equal(actionstepCredential.RefreshToken, tokenSet.RefreshToken);
            Assert.Equal(actionstepCredential.TokenType, tokenSet.TokenType);
            Assert.Equal(actionstepCredential.RevokedAtUtc.Value, tokenSet.RevokedAt.Value.ToDateTimeUtc());
        }

        [Fact]
        public void ActionstepCredentialToTokenSetWithoutLock()
        {
            var testStartTime = Instant.FromUtc(2019, 10, 27, 0, 0);
            var actionstepCredential = CreateTestCredential(testStartTime, withLock: false);

            var tokenSet = actionstepCredential.ToTokenSet();

            Assert.Equal(actionstepCredential.AccessToken, tokenSet.AccessToken);
            Assert.Equal(actionstepCredential.ActionstepOrg.Key, tokenSet.OrgKey);
            Assert.Equal(actionstepCredential.ApiEndpoint, tokenSet.ApiEndpoint);
            Assert.Equal(actionstepCredential.ExpiresIn, tokenSet.ExpiresIn);
            Assert.Equal(actionstepCredential.Id.ToString(CultureInfo.InvariantCulture), tokenSet.Id);
            Assert.Equal(actionstepCredential.IdToken, tokenSet.IdToken);
            Assert.Null(tokenSet.RefreshLockInfo);
            Assert.Equal(actionstepCredential.Owner.Id, tokenSet.UserId);
            Assert.Equal(actionstepCredential.ReceivedAtUtc, tokenSet.ReceivedAt.ToDateTimeUtc());
            Assert.Equal(actionstepCredential.RefreshToken, tokenSet.RefreshToken);
            Assert.Equal(actionstepCredential.TokenType, tokenSet.TokenType);
            Assert.Equal(actionstepCredential.RevokedAtUtc.Value, tokenSet.RevokedAt.Value.ToDateTimeUtc());
        }

        [Fact]
        public void UpdateFromTokenSetWithLock()
        {
            var testStartTime = Instant.FromUtc(2019, 10, 27, 0, 0);
            var actionstepCredential = CreateTestCredential(testStartTime, withLock: true);
            var testTokenSet = CreateTestTokenSet(
                testStartTime,
                actionstepCredential.Id.ToString(CultureInfo.InvariantCulture),
                actionstepCredential.ActionstepOrg.Key,
                actionstepCredential.Owner.Id);

            testTokenSet.RefreshLockInfo = new TokenSetRefreshLockInfo(testStartTime.PlusTicks(2000), new Guid("22222222-2222-2222-2222-222222222222"));
            testTokenSet.MarkRevoked(testStartTime.PlusTicks(3000));

            actionstepCredential.UpdateFromTokenSet(testTokenSet);

            Assert.Equal(testTokenSet.AccessToken, actionstepCredential.AccessToken);
            Assert.Equal(testTokenSet.AccessTokenExpiresAt.ToDateTimeUtc(), actionstepCredential.AccessTokenExpiryUtc);
            Assert.Equal(testTokenSet.OrgKey, actionstepCredential.ActionstepOrg.Key);
            Assert.Equal(testTokenSet.ApiEndpoint, actionstepCredential.ApiEndpoint);
            Assert.Equal(testTokenSet.ExpiresIn, actionstepCredential.ExpiresIn);
            Assert.Equal(testTokenSet.Id.ToString(CultureInfo.InvariantCulture), actionstepCredential.Id.ToString(CultureInfo.InvariantCulture));
            Assert.Equal(testTokenSet.IdToken, actionstepCredential.IdToken);
            Assert.Equal(testTokenSet.RefreshLockInfo.LockExpiresAt.ToDateTimeUtc(), actionstepCredential.LockExpiresAtUtc);
            Assert.Equal(testTokenSet.RefreshLockInfo.LockId, actionstepCredential.LockId);
            Assert.Equal(testTokenSet.UserId, actionstepCredential.Owner.Id);
            Assert.Equal(testTokenSet.ReceivedAt.ToDateTimeUtc(), actionstepCredential.ReceivedAtUtc);
            Assert.Equal(testTokenSet.RefreshToken, actionstepCredential.RefreshToken);
            Assert.Equal(testTokenSet.RefreshTokenExpiresAt.ToDateTimeUtc(), actionstepCredential.RefreshTokenExpiryUtc);
            Assert.Equal(testTokenSet.TokenType, actionstepCredential.TokenType);
            Assert.Equal(testTokenSet.RevokedAt.Value.ToDateTimeUtc(), actionstepCredential.RevokedAtUtc.Value);
        }

        [Fact]
        public void UpdateFromTokenSetWithoutLock()
        {
            var testStartTime = Instant.FromUtc(2019, 10, 27, 0, 0);

            // Arrange - must make sure the original actionstepCredential has a lock
            var actionstepCredential = CreateTestCredential(testStartTime, withLock: true);

            var testTokenSet = CreateTestTokenSet(
                testStartTime,
                actionstepCredential.Id.ToString(CultureInfo.InvariantCulture),
                actionstepCredential.ActionstepOrg.Key,
                actionstepCredential.Owner.Id);

            testTokenSet.RefreshLockInfo = null;

            // Arrange (well, the stuff above too - but this is the important bit).
            testTokenSet.ResetRefreshLock();

            // Act
            actionstepCredential.UpdateFromTokenSet(testTokenSet);

            // Assert
            Assert.Equal(DateTime.MinValue, actionstepCredential.LockExpiresAtUtc);
            Assert.Equal(Guid.Empty, actionstepCredential.LockId);
        }

        [Fact]
        public void UpdateFromTokenSetThrowsIfUserDiffers()
        {
            var testStartTime = Instant.FromUtc(2019, 10, 27, 0, 0);
            var actionstepCredential = CreateTestCredential(testStartTime, withLock: true);
            var tokenId = actionstepCredential.Id.ToString(CultureInfo.InvariantCulture);
            var testTokenSet = CreateTestTokenSet(testStartTime, tokenId, actionstepCredential.ActionstepOrg.Key, "UpdatedUserId");

            var ex = Assert.Throws<TokenSetUserDoesntMatchActionstepCredentialUserException>(() =>
            {
                actionstepCredential.UpdateFromTokenSet(testTokenSet);
            });

            Assert.Equal(
                "Could not update ActionstepCredentials properties from TokenSet. " +
                "The User IDs don't match (ActionstepCredential User ID: 'UserId', " +
                "TokenSet User ID: 'UpdatedUserId', TokenSet ID: '2')",
                ex.Message);
        }

        [Fact]
        public void UpdateFromTokenSetThrowsIfIdDiffers()
        {
            var testStartTime = Instant.FromUtc(2019, 10, 27, 0, 0);
            var actionstepCredential = CreateTestCredential(testStartTime, withLock: true);
            var newTokenId = (actionstepCredential.Id + 1).ToString(CultureInfo.InvariantCulture);
            var testTokenSet = CreateTestTokenSet(testStartTime, newTokenId, actionstepCredential.ActionstepOrg.Key);

            var ex = Assert.Throws<TokenSetIdDoesntMatchActionstepCredentialIdException>(() =>
            {
                actionstepCredential.UpdateFromTokenSet(testTokenSet);
            });

            Assert.Equal(
                "Could not update ActionstepCredentials properties from TokenSet. " +
                    "The TokenSet id was not empty and didn't match the id of the " +
                    "ActionstepCredential (ActionstepCredential id: '2', TokenSet id: '3')",
                ex.Message);
        }

        [Fact]
        public void UpdateFromTokenSetThrowsIfOrgDiffers()
        {
            var testStartTime = Instant.FromUtc(2019, 10, 27, 0, 0);
            var actionstepCredential = CreateTestCredential(testStartTime, withLock: true);
            var testTokenSet = CreateTestTokenSet(testStartTime, actionstepCredential.Id.ToString(CultureInfo.InvariantCulture), "UpdatedOrgKey");

            var ex = Assert.Throws<TokenSetOrgDoesntMatchActionstepCredentialOrgException>(() =>
            {
                actionstepCredential.UpdateFromTokenSet(testTokenSet);
            });

            Assert.Equal(
                "Could not update ActionstepCredentials properties from TokenSet. " +
                "The Org Keys don't match (ActionstepCredential OrgKey: 'ActionstepOrg', " +
                "TokenSet OrgKey: 'UpdatedOrgKey', TokenSet ID: '2')",
                ex.Message);
        }

            private static TokenSet CreateTestTokenSet(Instant testStartTime, string id, string orgKey, string userId = "UpdatedUserId")
        {
            const int UPDATED_EXPIRES_IN = 2;

            var testTokenSet = new TokenSet(
                "UpdatedAccessToken",
                "UpdatedTokenType",
                UPDATED_EXPIRES_IN,
                new Uri("https://uri/updated"),
                orgKey,
                "UpdatedRefreshToken",
                testStartTime.PlusTicks(1000),
                userId,
                id,
                new JwtSecurityToken("UpdatedIssuer", "UpdatedAudience")
                );

            return testTokenSet;
        }

        private static ActionstepCredential CreateTestCredential(Instant testStartTime, bool withLock)
        {
            return new ActionstepCredential()
            {
                AccessToken = "AccessToken",
                AccessTokenExpiryUtc = testStartTime.PlusTicks(100).ToDateTimeUtc(),
                ActionstepOrg = new ActionstepOrg() { Key = "ActionstepOrg" },
                ApiEndpoint = new Uri("https://uri/"),
                CreatedBy = new WCAUser() { Email = "CreatedBy@Domain" },
                DateCreatedUtc = testStartTime.PlusTicks(200).ToDateTimeUtc(),
                ExpiresIn = 1,
                Id = 2,
                IdToken = new JwtSecurityToken("Issuer", "Audience"),
                LastUpdatedUtc = testStartTime.PlusTicks(300).ToDateTimeUtc(),
                LockExpiresAtUtc = withLock ? testStartTime.PlusTicks(400).ToDateTimeUtc() : default(DateTime),
                LockId = withLock ? new Guid("11111111-1111-1111-1111-111111111111") : Guid.Empty,
                Owner = new WCAUser() { Id = "UserId", Email = "Owner@Domain" },
                ReceivedAtUtc = testStartTime.PlusTicks(500).ToDateTimeUtc(),
                RefreshToken = "RefreshToken",
                RefreshTokenExpiryUtc = testStartTime.PlusTicks(600).ToDateTimeUtc(),
                TokenType = "TokenType",
                UpdatedBy = new WCAUser() { Email = "UpdatedBy@Domain" },
                RevokedAtUtc = testStartTime.PlusTicks(700).ToDateTimeUtc()
            };
        }
    }
}
