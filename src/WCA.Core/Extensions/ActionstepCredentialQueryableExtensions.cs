using System;
using System.Linq;
using WCA.Domain.Actionstep;
using WCA.Domain.Models.Account;

namespace WCA.Core.Extensions
{
    public static class ActionstepCredentialQueryableExtensions
    {
        public static IQueryable<ActionstepCredential> ForOwnerAndOrg(this IQueryable<ActionstepCredential> actionstepCredentials, string userId, string orgKey)
        {
            return actionstepCredentials.Where(c => c.Owner.Id == userId && c.ActionstepOrg.Key == orgKey);
        }

        public static IQueryable<ActionstepCredential> ForOwnerAndOrg(this IQueryable<ActionstepCredential> actionstepCredentials, WCAUser wCAUser, string orgKey)
        {
            return actionstepCredentials.Where(c => c.Owner == wCAUser && c.ActionstepOrg.Key == orgKey);
        }

        public static bool UserHasValidCredentialsForOrg(this IQueryable<ActionstepCredential> actionstepCredentials, WCAUser wCAUser, string orgKey)
        {
            return actionstepCredentials.Any(c =>
                c.Owner == wCAUser
                && c.ActionstepOrg.Key == orgKey
                && c.RefreshTokenExpiryUtc > DateTime.UtcNow);
        }
    }
}
