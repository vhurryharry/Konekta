
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Core.Extensions;
using WCA.Core.Services;
using WCA.GlobalX.Client.Authentication;

namespace WCA.Core.Features.GlobalX.Authentication
{
    public class GlobalXCredentialsRepository : IGlobalXCredentialsRepository
    {
        private const string _credentialNamePrefix = "GlobalX-credentials-for-user-";
        private const string _usernameKey = "Username";
        private readonly ITelemetryLogger _telemetry;
        private readonly IKeyVaultClient _keyVaultClient;
        private readonly WCACoreSettings _appSettings;

        public GlobalXCredentialsRepository(
            ITelemetryLogger telemetry,
            IKeyVaultClient keyVaultClient,
            IOptions<WCACoreSettings> appSettingsAccessor)
        {
            if (appSettingsAccessor is null) throw new ArgumentNullException(nameof(appSettingsAccessor));

            _telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
            _appSettings = appSettingsAccessor.Value;
            _keyVaultClient = keyVaultClient ?? throw new ArgumentNullException(nameof(keyVaultClient));
        }

        public async Task AddOrUpdateGlobalXCredentials(GlobalXCredentials credentials)
        {
            if (credentials is null) { throw new ArgumentNullException(nameof(credentials)); }

            _telemetry.TrackEvent(nameof(AddOrUpdateGlobalXCredentials), new Dictionary<string, string>() {
                { "User Id", credentials.UserId }
            });

            await _keyVaultClient.SetSecretAsync(
                _appSettings.CredentialAzureKeyVaultUrl,
                EnsureUserIdPrefix(credentials.UserId),
                credentials.Password,
                new Dictionary<string, string>()
                {
                    { _usernameKey, credentials.Username }
                });
        }

        public async Task<GlobalXCredentials> GetCredentialsForUser(string userId)
        {
            var secretBundle = await _keyVaultClient.GetSecretOrNullAsync(
                _appSettings.CredentialAzureKeyVaultUrl,
                EnsureUserIdPrefix(userId));

            if (secretBundle != null && !string.IsNullOrEmpty(secretBundle.Value))
            {
                return new GlobalXCredentials()
                {
                    // Pass through userId. If a secredBundle was found, the userId will match.
                    UserId = userId,
                    Username = secretBundle.Tags[_usernameKey],
                    Password = secretBundle.Value
                };
            }
            else
            {
                return null;
            }
        }

        private string EnsureUserIdPrefix(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Must be supplied", nameof(userId));

            return userId.StartsWith(_credentialNamePrefix, StringComparison.Ordinal)
                ? userId
                : $"{_credentialNamePrefix}{userId}";
        }

        private string RemoveUserIdPrefix(string userIdWithPrefix)
        {
            if (string.IsNullOrEmpty(userIdWithPrefix)) throw new ArgumentException("Must be supplied", nameof(userIdWithPrefix));

            return userIdWithPrefix.StartsWith(_credentialNamePrefix, StringComparison.Ordinal)
                ? userIdWithPrefix.Substring(_credentialNamePrefix.Length)
                : userIdWithPrefix;
        }
    }
}
