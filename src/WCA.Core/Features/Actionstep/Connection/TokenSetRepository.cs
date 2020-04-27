using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Core.Account;
using WCA.Core.Extensions;
using WCA.Core.Services;
using WCA.Data;
using WCA.Domain.Actionstep;

namespace WCA.Core.Features.Actionstep.Connection
{
    public class TokenSetRepository : ITokenSetRepository
    {
        private readonly ITelemetryLogger _telemetry;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IClock _clock;

        public TokenSetRepository(
            ITelemetryLogger telemetry,
            IServiceScopeFactory serviceScopeFactory,
            IClock clock)
        {
            _telemetry = telemetry;
            _serviceScopeFactory = serviceScopeFactory;
            _clock = clock;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenSet"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Thrown if a tokenSet could not be found with the specified ID.</exception>
        public async Task<TokenSet> AddOrUpdateTokenSet(TokenSet tokenSet)
        {
            // Use a separate scope for each method call to prevent DBContext caching
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<WCADbContext>();

            if (tokenSet is null) { throw new ArgumentNullException(nameof(tokenSet)); }

            _telemetry.TrackEvent(nameof(AddOrUpdateTokenSet), new Dictionary<string, string>() {
                { "TokenSetId", tokenSet.Id },
                { "RefreshLockInfo ExpiresAt", tokenSet.RefreshLockInfo?.LockExpiresAt.ToString() },
                { "RefreshLockInfo LockId", tokenSet.RefreshLockInfo?.LockId.ToString() },
            });

            ActionstepCredential actionstepCredential = null;

            var tokenSetUser = await dbContext.Users.FindAsync(tokenSet.UserId);

            if (tokenSetUser is null)
            {
                throw new UserNotFoundException("Couldn't find user attempting to add or update TokenSet.", tokenSet.UserId);
            }

            if (!string.IsNullOrEmpty(tokenSet.Id))
            {
                var tokenSetId = int.Parse(tokenSet.Id, CultureInfo.InvariantCulture);
                actionstepCredential = await dbContext.ActionstepCredentials
                    .Include(c => c.Owner)
                    .Include(c => c.ActionstepOrg)
                    .SingleOrDefaultAsync(c => c.Id == tokenSetId);
            }

            // If not found by id, attempt to find by user and org
            if (actionstepCredential is null)
            {
                actionstepCredential = await dbContext.ActionstepCredentials
                    .Include(c => c.Owner)
                    .Include(c => c.ActionstepOrg)
                    .ForOwnerAndOrg(tokenSetUser, tokenSet.OrgKey)
                    .SingleOrDefaultAsync();
            }

            var now = _clock.GetCurrentInstant().ToDateTimeUtc();

            // If still null, then we need to create it
            if (actionstepCredential is null)
            {
                actionstepCredential = new ActionstepCredential();
                actionstepCredential.Owner = tokenSetUser;
                actionstepCredential.CreatedBy = tokenSetUser;
                actionstepCredential.DateCreatedUtc = now;
                dbContext.ActionstepCredentials.Add(actionstepCredential);
            }

            if (actionstepCredential.ActionstepOrg == null || actionstepCredential.ActionstepOrg.Key == null)
            {
                var orgToAssociate = dbContext.ActionstepOrgs.Where(o => o.Key == tokenSet.OrgKey).SingleOrDefault();

                if (orgToAssociate == null)
                {
                    var newOrg = new ActionstepOrg()
                    {
                        Key = tokenSet.OrgKey,
                        Title = tokenSet.OrgKey,
                        CreatedBy = actionstepCredential.Owner,
                        DateCreatedUtc = now,
                        UpdatedBy = actionstepCredential.Owner,
                        LastUpdatedUtc = now,
                    };

                    EntityEntry<ActionstepOrg> addedOrgEntity = dbContext.ActionstepOrgs.Add(newOrg);
                    orgToAssociate = addedOrgEntity.Entity;
                }

                actionstepCredential.ActionstepOrg = orgToAssociate;
            }
            else
            {
                if (!actionstepCredential.ActionstepOrg.Key.Equals(tokenSet.OrgKey, StringComparison.InvariantCulture))
                {
                    throw new TokenSetOrgKeyMismatchException(
                        "An attempt was made to update a TokenSet, however the Actionstep org key supplied in the TokenSet doesn't match the saved org key stored.",
                        actionstepCredential.ActionstepOrg.Key,
                        tokenSet);
                }
            }

            /// Copies simple field mappings, except things like org because it's a complex relationship in <see cref="ActionstepCredential"/>.
            actionstepCredential.UpdateFromTokenSet(tokenSet);

            actionstepCredential.UpdatedBy = tokenSetUser;
            actionstepCredential.LastUpdatedUtc = now;
            actionstepCredential.ConcurrencyStamp = Guid.NewGuid();

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbConcurrencyException)
            {
                throw new TokenSetConcurrencyException(tokenSet, dbConcurrencyException);
            }

            return actionstepCredential.ToTokenSet();
        }

        public async Task<TokenSet> GetTokenSetById(string tokenSetId)
        {
            // Use a separate scope for each method call to prevent DBContext caching
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<WCADbContext>();

            var tokenSetIdInt = int.Parse(tokenSetId, CultureInfo.InvariantCulture);

            var tokenSet = await dbContext.ActionstepCredentials
                .AsNoTracking()
                .Include(c => c.ActionstepOrg)
                .Include(c => c.Owner)
                .SingleOrDefaultAsync(c => c.Id == tokenSetIdInt);

            if (tokenSet == null) return null;
            return tokenSet.ToTokenSet();
        }

        public async Task<IEnumerable<TokenSet>> GetTokensByRefreshExpiry(Instant expiresBefore)
        {
            // Use a separate scope for each method call to prevent DBContext caching
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<WCADbContext>();

            var expiresAfterDateTimeUtc = expiresBefore.ToDateTimeUtc();

            return await dbContext.ActionstepCredentials
                .AsNoTracking()
                .Include(c => c.ActionstepOrg)
                .Include(c => c.Owner)
                .Where(c => c.RefreshTokenExpiryUtc < expiresAfterDateTimeUtc)
                .Select(c => c.ToTokenSet())

                // Must return a list because the DbContext is being disposed of in this method.
                .ToListAsync();
        }

        public Task<TokenSet> GetTokenSet(TokenSetQuery tokenSetQuery)
        {
            // Use a separate scope for each method call to prevent DBContext caching
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<WCADbContext>();

            if (tokenSetQuery is null) throw new ArgumentNullException(nameof(tokenSetQuery));

            var userToGetCredentialsFor = tokenSetQuery.UserId;

            // Check to see if there is a substitute
            var substitute = dbContext
                .ActionstepCredentialSubstitutions
                .AsNoTracking()
                .Include(e => e.SubstituteWithOwner)
                .SingleOrDefault(e => e.ForOwner.Id == tokenSetQuery.UserId);

            if (substitute != null)
            {
                userToGetCredentialsFor = substitute.SubstituteWithOwner.Id;
            }

            var tokenSet = dbContext
                .ActionstepCredentials
                .AsNoTracking()
                .Include(c => c.Owner)
                .Include(c => c.ActionstepOrg)
                .ForOwnerAndOrg(userToGetCredentialsFor, tokenSetQuery.OrgKey)
                .SingleOrDefault();

            if (tokenSet == null)
            {
                return Task.FromResult<TokenSet>(null);
            }

            return Task.FromResult(tokenSet.ToTokenSet());
        }

        /// <summary>
        /// Does NOT use substitution.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TokenSet>> GetAllTokenSetsForUser(string userId)
        {
            // Use a separate scope for each method call to prevent DBContext caching
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<WCADbContext>();

            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Parameter be supplied.", nameof(userId));

            return await dbContext
                .ActionstepCredentials
                .AsNoTracking()
                .Include(c => c.Owner)
                .Include(c => c.ActionstepOrg)
                .Where(c => c.Owner.Id == userId)
                .Select(c => c.ToTokenSet())

                // Must return a list because the DbContext is being disposed of in this method.
                .ToListAsync();
        }

        public async Task Remove(string tokenSetId)
        {
            // Use a separate scope for each method call to prevent DBContext caching
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<WCADbContext>();

            int tokenSetIdInt = int.Parse(tokenSetId, CultureInfo.InvariantCulture);

            var itemToRemove = dbContext
                .ActionstepCredentials
                .SingleOrDefault(c => c.Id == tokenSetIdInt);

            if (itemToRemove != null)
            {
                dbContext.ActionstepCredentials.Remove(itemToRemove);
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Does NOT use substitution.
        /// </summary>
        /// <param name="tokenSetQuery"></param>
        /// <returns></returns>
        public async Task Remove(TokenSetQuery tokenSetQuery)
        {
            // Use a separate scope for each method call to prevent DBContext caching
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<WCADbContext>();

            if (tokenSetQuery is null) throw new ArgumentNullException(nameof(tokenSetQuery));

            var itemToRemove = dbContext
                .ActionstepCredentials
                .ForOwnerAndOrg(tokenSetQuery.UserId, tokenSetQuery.OrgKey)
                .SingleOrDefault();

            if (itemToRemove != null)
            {
                dbContext.ActionstepCredentials.Remove(itemToRemove);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

