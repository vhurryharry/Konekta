using NodaTime;
using System;
using System.Globalization;
using WCA.Actionstep.Client.Resources;
using WCA.Domain.Actionstep;

namespace WCA.Core.Features.Actionstep.Connection
{
    public static class ActionstepCredentialExtensions
    {
        public static TokenSet ToTokenSet(this ActionstepCredential actionstepCredential)
        {
            if (actionstepCredential is null) throw new ArgumentNullException(nameof(actionstepCredential));

            if (actionstepCredential.Owner is null) throw new ArgumentException(
                $"The property '{nameof(ActionstepCredential.Owner)}' must be set to create a valid '{nameof(TokenSet)}'.");

            if (actionstepCredential.ActionstepOrg is null) throw new ArgumentException(
                 $"The property '{nameof(ActionstepCredential.ActionstepOrg)}' must be set to create a valid '{nameof(TokenSet)}'.");

            var tokenSet = new TokenSet(
                actionstepCredential.AccessToken,
                actionstepCredential.TokenType,
                actionstepCredential.ExpiresIn,
                actionstepCredential.ApiEndpoint,
                actionstepCredential.ActionstepOrg.Key,
                actionstepCredential.RefreshToken,
                Instant.FromDateTimeUtc(actionstepCredential.ReceivedAtUtc),
                actionstepCredential.Owner.Id.ToString(CultureInfo.InvariantCulture),
                actionstepCredential.Id.ToString(CultureInfo.InvariantCulture),
                actionstepCredential.IdToken,
                actionstepCredential.RevokedAtUtc.HasValue ? Instant.FromDateTimeUtc(actionstepCredential.RevokedAtUtc.Value) : (Instant?)null);

            if (actionstepCredential.LockId != Guid.Empty)
            {
                tokenSet.RefreshLockInfo = new TokenSetRefreshLockInfo(
                    Instant.FromDateTimeUtc(actionstepCredential.LockExpiresAtUtc),
                    actionstepCredential.LockId);
            }

            return tokenSet;
        }

        public static void UpdateFromTokenSet(this ActionstepCredential actionstepCredential, TokenSet tokenSet)
        {
            if (actionstepCredential is null) { throw new ArgumentNullException(nameof(actionstepCredential)); }
            if (tokenSet is null) { throw new ArgumentNullException(nameof(tokenSet)); }

            if (!string.IsNullOrEmpty(tokenSet.Id))
            {
                if (tokenSet.Id != actionstepCredential.Id.ToString(CultureInfo.InvariantCulture))
                {
                    throw new TokenSetIdDoesntMatchActionstepCredentialIdException(actionstepCredential.Id, tokenSet.Id);
                }
            }

            if (tokenSet.OrgKey != actionstepCredential.ActionstepOrg?.Key)
            {
                throw new TokenSetOrgDoesntMatchActionstepCredentialOrgException(actionstepCredential.ActionstepOrg?.Key, tokenSet.OrgKey, tokenSet.Id);
            }

            if (!string.IsNullOrEmpty(tokenSet.UserId))
            {
                if (tokenSet.UserId != actionstepCredential.Owner?.Id)
                {
                    throw new TokenSetUserDoesntMatchActionstepCredentialUserException(actionstepCredential.Owner?.Id, tokenSet.UserId, tokenSet.Id);
                }
            }

            actionstepCredential.AccessToken = tokenSet.AccessToken;
            actionstepCredential.AccessTokenExpiryUtc = tokenSet.AccessTokenExpiresAt.ToDateTimeUtc();
            actionstepCredential.TokenType = tokenSet.TokenType;
            actionstepCredential.ExpiresIn = tokenSet.ExpiresIn;
            actionstepCredential.ApiEndpoint = tokenSet.ApiEndpoint;
            actionstepCredential.RefreshToken = tokenSet.RefreshToken;
            actionstepCredential.RefreshTokenExpiryUtc = tokenSet.RefreshTokenExpiresAt.ToDateTimeUtc();
            actionstepCredential.ReceivedAtUtc = tokenSet.ReceivedAt.ToDateTimeUtc();
            actionstepCredential.IdToken = tokenSet.IdToken;

            if (tokenSet.RevokedAt.HasValue)
            {
                actionstepCredential.RevokedAtUtc = tokenSet.RevokedAt.Value.ToDateTimeUtc();
            }
            else
            {
                actionstepCredential.RevokedAtUtc = null;
            }

            if (tokenSet.RefreshLockInfo is null)
            {
                actionstepCredential.LockExpiresAtUtc = DateTime.MinValue;
                actionstepCredential.LockId = Guid.Empty;
            }
            else
            {
                actionstepCredential.LockExpiresAtUtc = tokenSet.RefreshLockInfo.LockExpiresAt.ToDateTimeUtc();
                actionstepCredential.LockId = tokenSet.RefreshLockInfo.LockId;
            }
        }
    }
}