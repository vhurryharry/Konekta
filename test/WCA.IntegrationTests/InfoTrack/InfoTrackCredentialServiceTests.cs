using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;
using WCA.Core;
using WCA.Core.Features.InfoTrack;
using WCA.IntegrationTests.TestInfrastructure;
using Xunit;

namespace WCA.IntegrationTests.InfoTrack
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class InfoTrackCredentialServiceTests
    {
        private readonly WebContainerFixture _containerFixture;

        public InfoTrackCredentialServiceTests(WebContainerFixture containerFixture)
        {
            _containerFixture = containerFixture;
        }

        [Fact(Skip = "Requires auth and a Key Vault")]
        public async Task CanSaveAndRetrieveCredentials()
        {
            using (var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken)))
            {
                var wcaSettings = Options.Create(new WCACoreSettings()
                {
                    CredentialAzureKeyVaultUrl = "https://wca-credentials.vault.azure.net/"
                });

                var repositoryUnderTest = new InfoTrackCredentialRepository(keyVaultClient, wcaSettings);

                const string testOrgKey = "orgKey";
                const string testUsername = "testUsername";
                const string testPassword = "testPassword";

                await repositoryUnderTest.SaveOrUpdateCredential(testOrgKey, testUsername, testPassword);
                var retrievedCredential = await repositoryUnderTest.FindCredential("orgKey");

                Assert.Equal(testOrgKey, retrievedCredential.ActionstepOrgKey);
                Assert.Equal(testUsername, retrievedCredential.Username);
                Assert.Equal(testPassword, retrievedCredential.Password);
            }
        }

        public static async Task<string> GetToken(string authority, string resource, string scope)
        {
            
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(
                "TODO",
                "TODO");
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}
