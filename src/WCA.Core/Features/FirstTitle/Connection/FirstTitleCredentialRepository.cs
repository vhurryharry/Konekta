using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WCA.Domain.Models.Account;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.FirstTitle.Connection
{
    public class FirstTitleCredentialRepository : IFirstTitleCredentialRepository
    {
        private const string usernameKeyName = "Username";
        private const string credentialNamePrefix = "FirstTitle-credentials-for-user-";

        private readonly IKeyVaultClient keyVaultClient;
        private WCACoreSettings settings;

        public FirstTitleCredentialRepository(
            IKeyVaultClient keyVaultClient,
            IOptions<WCACoreSettings> options)
        {
            this.keyVaultClient = keyVaultClient;
            settings = options.Value;
        }

        public async Task<FirstTitleCredential> FindCredential(WCAUser authenticatedUser)
        {
            try
            {
                var secret = await keyVaultClient.GetSecretAsync(
                    settings.CredentialAzureKeyVaultUrl,
                    $"{credentialNamePrefix}{authenticatedUser.Id}");

                return new FirstTitleCredential()
                {
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

        public async Task SaveOrUpdateCredential(StoreFirstTitleCredentialsCommand storeFirstTitleCredentialsCommand)
        {
            await keyVaultClient.SetSecretAsync(
                settings.CredentialAzureKeyVaultUrl,
                $"{credentialNamePrefix}{storeFirstTitleCredentialsCommand.AuthenticatedUser.Id}",
                storeFirstTitleCredentialsCommand.FirstTitleCredentials.Password,
                new Dictionary<string, string>()
                {
                    { usernameKeyName, storeFirstTitleCredentialsCommand.FirstTitleCredentials.Username}
                });
        }
    }
}
