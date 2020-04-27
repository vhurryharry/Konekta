using FluentValidation;
using MediatR;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WCA.Core.Features.Pexa.Authentication
{
    public class PexaApiTokenQueryHandler : IRequestHandler<PexaApiTokenQuery, PexaApiToken>
    {
        private const string _credentialNamePrefix = "PEXA-API-token-for-user-";

        private readonly IKeyVaultClient _keyVaultClient;
        private readonly PexaApiTokenQuery.Validator _validator;
        private readonly IMemoryCache _memoryCache;
        private readonly string _vaultBaseUrl;

        public PexaApiTokenQueryHandler(
            IKeyVaultClient keyVaultClient,
            PexaApiTokenQuery.Validator validator,
            IMemoryCache memoryCache,
            IOptions<WCACoreSettings> optionsAccessor)
        {
            _keyVaultClient = keyVaultClient;
            _validator = validator;
            _memoryCache = memoryCache;
            _vaultBaseUrl = optionsAccessor.Value.CredentialAzureKeyVaultUrl;
        }

        public async Task<PexaApiToken> Handle(PexaApiTokenQuery query, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(query);

            var credentialId = $"{_credentialNamePrefix}{query.AuthenticatedUser.Id}";

            if (query.BypassAndUpdateCache)
            {
                return await GetPexaApiTokenFromKeyVaultAndUpdateCache(credentialId);
            }

            if (_memoryCache.TryGetValue(credentialId, out PexaApiToken pexaApiToken))
            {
                if (pexaApiToken != null)
                {
                    return pexaApiToken;
                }
            }

            return await GetPexaApiTokenFromKeyVaultAndUpdateCache(credentialId);
        }

        private async Task<PexaApiToken> GetPexaApiTokenFromKeyVaultAndUpdateCache(string credentialId)
        {
            try
            {
                var secretBundle = await _keyVaultClient.GetSecretAsync(_vaultBaseUrl, credentialId);

                if (secretBundle != null && !string.IsNullOrEmpty(secretBundle.Value))
                {
                    var pexaApiToken = JsonConvert.DeserializeObject<PexaApiToken>(secretBundle.Value);
                    return _memoryCache.Set(credentialId, pexaApiToken, pexaApiToken.AccessTokenExpiryUtc.ToDateTimeOffset());
                }
                else
                {
                    _memoryCache.Remove(credentialId);
                }

                return null;
            }
            catch (KeyVaultErrorException ex)
            {
                // Not found means the credentials don't exist.
                // Forbidden could either mean that the secret is disabled,
                // or that there is a problem with authorization to the key vault.
                if (ex.Response?.StatusCode == HttpStatusCode.NotFound ||
                    ex.Response?.StatusCode == HttpStatusCode.Forbidden)
                {
                    return null;
                }

                throw;
            }
        }
    }
}
