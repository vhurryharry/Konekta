using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using System.Net;
using System.Threading.Tasks;

namespace WCA.Core.Extensions
{
    public static class KeyVaultExtensions
    {
        /// <summary>
        /// Get a <see cref="SecretBundle"/> using the specified parameters.
        /// </summary>
        /// <param name="keyVaultClient"></param>
        /// <param name="vaultBaseUrl"></param>
        /// <param name="secretName"></param>
        /// <returns>Returns <see langword="null"/> if the SecretBundle is not found. Otherwise will throw.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "Matches framework extension method parameter.")]
        public static async Task<SecretBundle> GetSecretOrNullAsync(this IKeyVaultClient keyVaultClient, string vaultBaseUrl, string secretName)
        {
            try
            {
                return await keyVaultClient.GetSecretAsync(vaultBaseUrl, secretName);
            }
            catch (KeyVaultErrorException ex)
            {
                // Not found means the credentials don't exist.
                // Forbidden could either mean that the secret is disabled,
                // or that there is a problem with authorization to the key vault.
                if (ex.Response?.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }
    }
}
