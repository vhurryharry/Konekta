using System.Collections.Generic;
using System.Threading.Tasks;
using NodaTime;
using WCA.Actionstep.Client.Resources;

namespace WCA.Actionstep.Client
{
    public interface ITokenSetRepository
    {
        /// <summary>
        /// Retrieve a TokenSet by it's identifier.
        /// </summary>
        /// <param name="tokenSetId">The unique identifier for the <see cref="TokenSet"/>.</param>
        /// <returns>The <see cref="TokenSet"/> with the given identifier if it exists. Otherwise <see langword="null" />.</returns>
        Task<TokenSet> GetTokenSetById(string tokenSetId);

        /// <summary>
        /// Retrieve a TokenSet by it's user id and org key.
        /// </summary>
        /// <param name="tokenSetQuery">The query object that contains the parameters to search for.</param>
        /// <returns>The <see cref="TokenSet"/> with for the given query if it exists. Otherwise <see langword="null" />.</returns>
        Task<TokenSet> GetTokenSet(TokenSetQuery tokenSetQuery);

        /// <summary>
        /// Retrieve all <see cref="TokenSet"/>'s stored for a given user. For example, if a user has
        /// connected to multiple orgs, they will all be returned.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<TokenSet>> GetAllTokenSetsForUser(string userId);

        /// <summary>
        /// Update a <see cref="TokenSet"/> based on its identifier.
        /// </summary>
        /// <param name="tokenSet"></param>
        /// <returns></returns>
        Task<TokenSet> AddOrUpdateTokenSet(TokenSet tokenSet);

        /// <summary>
        /// Get tokens whose refresh tokens expire before the supplied Instant.
        /// </summary>
        /// <param name="expiresBefore"></param>
        /// <returns></returns>
        Task<IEnumerable<TokenSet>> GetTokensByRefreshExpiry(Instant expiresBefore);

        /// <summary>
        /// Removes a given TokenSet from the repository.
        /// </summary>
        /// <param name="tokenSetId"></param>
        /// <returns></returns>
        Task Remove(string tokenSetId);

        /// <summary>
        /// Removes a given TokenSet from the repository.
        /// </summary>
        /// <param name="tokenSetQuery"></param>
        /// <returns></returns>
        Task Remove(TokenSetQuery tokenSetQuery);
    }
}
