using FakeItEasy;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCA.Actionstep.Client.Resources;

namespace WCA.Actionstep.Client.Tests.MockServices
{
    public class TestTokenSetRepository : ITokenSetRepository
    {
        public List<TokenSet> TokenSets { get; } = new List<TokenSet>();

        public int AddOrUpdateTokenSetCount { get; private set; } = 0;
        public int GetTokenSetByIdCount { get; private set; } = 0;

        public Task<TokenSet> AddOrUpdateTokenSet(TokenSet tokenSet)
        {
            AddOrUpdateTokenSetCount++;

            if (tokenSet is null) throw new ArgumentNullException(nameof(tokenSet));

            var existingToken = TokenSets.Find(t => t.Id == tokenSet.Id);

            if (existingToken != null)
            {
                TokenSets.Remove(existingToken);
            }

            if (string.IsNullOrEmpty(tokenSet.Id))
            {
                tokenSet.Id = Guid.NewGuid().ToString();
            }

            TokenSets.Add(tokenSet);

            return Task.FromResult(tokenSet);
        }

        public Task<IEnumerable<TokenSet>> GetAllTokenSetsForUser(string userId)
        {
            return Task.FromResult(TokenSets.Where(t => t.UserId.Equals(userId, StringComparison.Ordinal)));
        }

        public Task<IEnumerable<TokenSet>> GetTokensByRefreshExpiry(Instant expiresBefore)
        {
            return Task.FromResult(TokenSets.Where(t => t.RefreshTokenExpiresAt < expiresBefore).ToList().AsEnumerable());
        }

        public Task<TokenSet> GetTokenSet(TokenSetQuery tokenSetQuery)
        {
            return Task.FromResult(TokenSets.SingleOrDefault(t => t.UserId == tokenSetQuery.UserId && t.OrgKey == tokenSetQuery.OrgKey));
        }

        public Task<TokenSet> GetTokenSetById(string tokenSetId)
        {
            GetTokenSetByIdCount++;

            return Task.FromResult(TokenSets.Find(t => t.Id == tokenSetId));
        }

        public Task Remove(string tokenSetId)
        {
            var itemToRemove = TokenSets.SingleOrDefault(t => t.Id.Equals(tokenSetId, StringComparison.Ordinal));

            if (itemToRemove != null)
            {
                TokenSets.Remove(itemToRemove);

            }

            return Task.CompletedTask;
        }

        public Task Remove(TokenSetQuery tokenSetQuery)
        {
            var itemToRemove = TokenSets.SingleOrDefault(t => t.UserId == tokenSetQuery.UserId && t.OrgKey == tokenSetQuery.OrgKey);

            if (itemToRemove != null)
            {
                TokenSets.Remove(itemToRemove);

            }

            return Task.CompletedTask;
        }
    }
}