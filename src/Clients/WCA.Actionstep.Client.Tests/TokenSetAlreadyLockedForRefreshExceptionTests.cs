using NodaTime;
using System;
using WCA.Actionstep.Client.Resources;
using Xunit;

namespace WCA.Actionstep.Client.Tests
{
    public class TokenSetAlreadyLockedForRefreshExceptionTests
    {
        [Fact]
        public void TokenSetAlreadyLockedForRefreshExceptionProducesCorrectMessage()
        {
            var tokenSet = new TokenSet(
                "AccessToken",
                "TokenType",
                3600,
                new Uri("https://uri/"),
                "OrgKey",
                "RefreshToken",
                Instant.FromUtc(2019, 10, 27, 0, 0),
                "UserId",
                "Id");

            tokenSet.RefreshLockInfo = new TokenSetRefreshLockInfo(Instant.FromUtc(2019, 10, 27, 0, 1), new Guid("F74EFD5C-70CD-4D3D-9FCD-B37FBD9F2A13"));

            var tokenSetAlreadyLockedForRefreshException = new TokenSetAlreadyLockedForRefreshException(tokenSet);

            Assert.Equal(
                "Cannot refresh TokenSet with ID 'Id' for user 'UserId' because an existing " +
                "lock was found and it has not yet expired. Lock ID: 'f74efd5c-70cd-4d3d-9fcd-b37fbd9f2a13', " +
                "Lock Expires At: '2019-10-27T00:01:00Z'",
                tokenSetAlreadyLockedForRefreshException.Message);
        }

        [Fact]
        public void TokenSetAlreadyLockedForRefreshExceptionProducesMessageIfTokenSetIsMissingData()
        {
            var tokenSet = new TokenSet(
                "AccessToken",
                "TokenType",
                3600,
                new Uri("https://uri/"),
                "OrgKey",
                "RefreshToken",
                Instant.FromUtc(2019, 10, 27, 0, 0));

            var tokenSetAlreadyLockedForRefreshException = new TokenSetAlreadyLockedForRefreshException(tokenSet);

            Assert.Equal(
                "Cannot refresh TokenSet with ID '' for user '' because an existing " +
                "lock was found and it has not yet expired. Lock ID: '', " +
                "Lock Expires At: ''",
                tokenSetAlreadyLockedForRefreshException.Message);
        }
    }
}
