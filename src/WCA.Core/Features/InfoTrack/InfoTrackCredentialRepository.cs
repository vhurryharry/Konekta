using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WCA.Domain.InfoTrack;

namespace WCA.Core.Features.InfoTrack
{
    public class InfoTrackCredentialRepository : IInfoTrackCredentialRepository
    {
        private const string usernameKeyName = "Username";
        private const string credentialNamePrefix = "InfoTrack-";

        private readonly IKeyVaultClient keyVaultClient;
        private WCACoreSettings settings;

        public InfoTrackCredentialRepository(
            IKeyVaultClient keyVaultClient,
            IOptions<WCACoreSettings> options)
        {
            if (options is null) throw new System.ArgumentNullException(nameof(options));

            this.keyVaultClient = keyVaultClient ?? throw new System.ArgumentNullException(nameof(keyVaultClient));
            settings = options.Value;
        }

        public async Task<InfoTrackCredentials> FindCredential(string actionstepOrgKey)
        {
            try
            {
                var secret = await keyVaultClient.GetSecretAsync(
                    settings.CredentialAzureKeyVaultUrl,
                    $"{credentialNamePrefix}{actionstepOrgKey}");

                return new InfoTrackCredentials()
                {
                    ActionstepOrgKey = actionstepOrgKey,
                    Username = secret.Tags[usernameKeyName],
                    Password = secret.Value
                };
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

        public async Task SaveOrUpdateCredential(string actionstepOrgKey, string username, string password)
        {
            await keyVaultClient.SetSecretAsync(
                settings.CredentialAzureKeyVaultUrl,
                $"{credentialNamePrefix}{actionstepOrgKey}",
                password,
                new Dictionary<string, string>()
                {
                    { usernameKeyName, username }
                });
        }
    }
}
