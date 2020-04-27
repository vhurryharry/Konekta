using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using WCA.Actionstep.AspNetCore.Authentication;
using WCA.Domain.Models.Account;
using WCA.Web.Security;
using Xunit;

namespace WCA.UnitTests.Security
{
    public class WCASigninManagerTests
    {
        [Fact]
        public void NameIsSetWithSingleWord()
        {
            const string firstName = "Name";
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ActionstepJwtClaimTypes.Name, firstName));
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var user = new WCAUser();
            WCASignInManager.SetFirstAndLastNameIfMissing(user, claimsPrincipal, NullLogger.Instance);
            Assert.Equal(firstName, user.FirstName);
            Assert.Null(user.LastName);
        }

        [Fact]
        public void NameIsSetWithTwoWords()
        {
            const string firstName = "Firstname";
            const string lastName = "Lastname";
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ActionstepJwtClaimTypes.Name, $"{firstName} {lastName}"));
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var user = new WCAUser();
            WCASignInManager.SetFirstAndLastNameIfMissing(user, claimsPrincipal, NullLogger.Instance);
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
        }

        [Fact]
        public void NameIsSetWithTwoThreeWords()
        {
            const string firstName = "Firstname";
            const string middleName = "Middlename";
            const string lastName = "Lastname";
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ActionstepJwtClaimTypes.Name, $"{firstName} {middleName} {lastName}"));
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var user = new WCAUser();
            WCASignInManager.SetFirstAndLastNameIfMissing(user, claimsPrincipal, NullLogger.Instance);
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
        }

        [Fact]
        public void NoErrorsIfNoNameAvailable()
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var user = new WCAUser();
            WCASignInManager.SetFirstAndLastNameIfMissing(user, claimsPrincipal, NullLogger.Instance);
            Assert.Null(user.FirstName);
            Assert.Null(user.LastName);
        }
    }
}
