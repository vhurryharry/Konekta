﻿using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core;
using WCA.Core.Features.Pexa.Authentication;
using WCA.Domain.Models.Account;
using Xunit;

namespace WCA.UnitTests.PEXA
{
    public class PexaApiTokenQueryTests
    {
        public PexaApiTokenQueryTests()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                return jsonSerializerSettings;
            };
        }

        [Fact]
        public async void PexaApiTokenQueryNoTokenReturnsNull()
        {
            var query = new PexaApiTokenQuery()
            {
                AuthenticatedUser = new WCAUser()
            };

            using (var pexaTestKeyVaultClient = new PexaTestKeyVaultClient(null, null))
            using (var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions())))
            {
                var handlerUnderTest = new PexaApiTokenQueryHandler(
                    pexaTestKeyVaultClient,
                    new PexaApiTokenQuery.Validator(),
                    memoryCache,
                    Options.Create(new WCACoreSettings() { CredentialAzureKeyVaultUrl = "https://dummy" }));

                var result = await handlerUnderTest.Handle(query, new CancellationToken());

                Assert.Null(result);
            }
        }

        [Fact]
        public async void PexaApiTokenQueryWithTokenInVault()
        {
            var query = new PexaApiTokenQuery()
            {
                AuthenticatedUser = new WCAUser()
            };

            var credentialId = $"PEXA-API-token-for-user-{query.AuthenticatedUser.Id}";
            var testToken = "{\"access_token\": \"Test Access Token\"}";
            var testPexaApiToken = PexaApiToken.Success(JObject.Parse(testToken), Instant.FromUtc(2019, 7, 14, 2, 5));

            using (var pexaTestKeyVaultClient = new PexaTestKeyVaultClient(credentialId, testPexaApiToken))
            using (var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions())))
            {
                var handlerUnderTest = new PexaApiTokenQueryHandler(
                pexaTestKeyVaultClient,
                new PexaApiTokenQuery.Validator(),
                memoryCache,
                Options.Create(new WCACoreSettings() { CredentialAzureKeyVaultUrl = "https://dummy" }));

                var result = await handlerUnderTest.Handle(query, new CancellationToken());

                Assert.Equal("Test Access Token", result.AccessToken);
            }
        }

        [Fact]
        public async void PexaApiTokenQueryRetrieveFromCache()
        {
            var query = new PexaApiTokenQuery()
            {
                AuthenticatedUser = new WCAUser(),
                BypassAndUpdateCache = false
            };

            var credentialId = $"PEXA-API-token-for-user-{query.AuthenticatedUser.Id}";

            var testTokenInCache = "{\"access_token\": \"Test Access Token in Cache\"}";
            var testPexaApiTokenInCache = PexaApiToken.Success(JObject.Parse(testTokenInCache), Instant.FromUtc(2019, 7, 14, 2, 5));

            var testTokenInVault = "{\"access_token\": \"Test Access Token in Vault\"}";
            var testPexaApiTokenInVault = PexaApiToken.Success(JObject.Parse(testTokenInVault), Instant.FromUtc(2019, 7, 14, 2, 5));

            using (var pexaTestKeyVaultClient = new PexaTestKeyVaultClient(credentialId, testPexaApiTokenInVault))
            using (var testMemoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions())))
            {
                testMemoryCache.Set(credentialId, testPexaApiTokenInCache);
                var handlerUnderTest = new PexaApiTokenQueryHandler(
                pexaTestKeyVaultClient,
                new PexaApiTokenQuery.Validator(),
                testMemoryCache,
                Options.Create(new WCACoreSettings() { CredentialAzureKeyVaultUrl = "https://dummy" }));

                var result = await handlerUnderTest.Handle(query, new CancellationToken());

                Assert.Equal("Test Access Token in Cache", result.AccessToken);
            }
        }

        [Fact]
        public async void PexaApiTokenQueryBypassCache()
        {
            var query = new PexaApiTokenQuery()
            {
                AuthenticatedUser = new WCAUser(),
                BypassAndUpdateCache = true
            };

            var credentialId = $"PEXA-API-token-for-user-{query.AuthenticatedUser.Id}";

            var testTokenInCache = "{\"access_token\": \"Test Access Token in Cache\"}";
            var testPexaApiTokenInCache = PexaApiToken.Success(JObject.Parse(testTokenInCache), Instant.FromUtc(2019, 7, 14, 2, 5));

            var testTokenInVault = "{\"access_token\": \"Test Access Token in Vault\"}";
            var testPexaApiTokenInVault = PexaApiToken.Success(JObject.Parse(testTokenInVault), Instant.FromUtc(2019, 7, 14, 2, 5));


            using (var pexaTestKeyVaultClient = new PexaTestKeyVaultClient(credentialId, testPexaApiTokenInVault))
            using (var testMemoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions())))
            {
                testMemoryCache.Set(credentialId, testPexaApiTokenInCache);
                var handlerUnderTest = new PexaApiTokenQueryHandler(
                    pexaTestKeyVaultClient,
                    new PexaApiTokenQuery.Validator(),
                    testMemoryCache,
                    Options.Create(new WCACoreSettings() { CredentialAzureKeyVaultUrl = "https://dummy" }));

                var result = await handlerUnderTest.Handle(query, new CancellationToken());

                Assert.Equal("Test Access Token in Vault", result.AccessToken);
            }
        }

        [Fact]
        public async void PexaApiTokenNotFoundReturnsNull()
        {
            var query = new PexaApiTokenQuery()
            {
                AuthenticatedUser = new WCAUser()
            };

            using (var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound))
            {
                var keyVaultErrorException = new KeyVaultErrorException()
                {
                    Response = new HttpResponseMessageWrapper(httpResponseMessage, null)
                };

                using (var pexaTestKeyVaultClient = new PexaTestKeyVaultClient(keyVaultErrorException))
                using (var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions())))
                {

                    var handlerUnderTest = new PexaApiTokenQueryHandler(
                        pexaTestKeyVaultClient,
                        new PexaApiTokenQuery.Validator(),
                        memoryCache,
                        Options.Create(new WCACoreSettings() { CredentialAzureKeyVaultUrl = "https://dummy" }));

                    var result = await handlerUnderTest.Handle(query, new CancellationToken());

                    Assert.Null(result);
                }
            }
        }

        [Fact]
        public async void PexaApiTokenForbiddenReturnsNull()
        {
            var query = new PexaApiTokenQuery()
            {
                AuthenticatedUser = new WCAUser()
            };

            using (var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden))
            {
                var keyVaultErrorException = new KeyVaultErrorException()
                {
                    Response = new HttpResponseMessageWrapper(httpResponseMessage, null)
                };

                using (var pexaTestKeyVaultClient = new PexaTestKeyVaultClient(keyVaultErrorException))
                using (var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions())))
                {
                    var handlerUnderTest = new PexaApiTokenQueryHandler(
                        pexaTestKeyVaultClient,
                        new PexaApiTokenQuery.Validator(),
                        memoryCache,
                        Options.Create(new WCACoreSettings() { CredentialAzureKeyVaultUrl = "https://dummy" }));

                    var result = await handlerUnderTest.Handle(query, new CancellationToken());

                    Assert.Null(result);
                }
            }
        }

        [Fact]
        public async void PexaApiTokenOtherKeyVaultExceptionThrows()
        {
            var query = new PexaApiTokenQuery()
            {
                AuthenticatedUser = new WCAUser()
            };

            var keyVaultErrorException = new KeyVaultErrorException();

            using (var pexaTestKeyVaultClient = new PexaTestKeyVaultClient(keyVaultErrorException))
            using (var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions())))
            {
                var handlerUnderTest = new PexaApiTokenQueryHandler(
                    pexaTestKeyVaultClient,
                    new PexaApiTokenQuery.Validator(),
                    memoryCache,
                    Options.Create(new WCACoreSettings() { CredentialAzureKeyVaultUrl = "https://dummy" }));

                await Assert.ThrowsAsync<KeyVaultErrorException>(async () =>
                {
                    var result = await handlerUnderTest.Handle(query, new CancellationToken());
                });
            }
        }

        private class PexaTestKeyVaultClient : IKeyVaultClient
        {
            private Dictionary<string, string> _keyValuePairs = new Dictionary<string, string>();
            private KeyVaultErrorException _keyVaultErrorException;

            public PexaTestKeyVaultClient(KeyVaultErrorException keyVaultErrorException)
            {
                _keyVaultErrorException = keyVaultErrorException;
            }

            public PexaTestKeyVaultClient(string pexaApiSecretName, PexaApiToken pexaApiTokenToReturn)
            {
                if (!string.IsNullOrEmpty(pexaApiSecretName) && pexaApiTokenToReturn != null)
                {
                    _keyValuePairs.Add(pexaApiSecretName, JsonConvert.SerializeObject(pexaApiTokenToReturn));
                }
            }

            public JsonSerializerSettings SerializationSettings => throw new NotImplementedException();

            public JsonSerializerSettings DeserializationSettings => throw new NotImplementedException();

            public ServiceClientCredentials Credentials => throw new NotImplementedException();

            public string ApiVersion => throw new NotImplementedException();

            public string AcceptLanguage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int? LongRunningOperationRetryTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public bool? GenerateClientRequestId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public Task<AzureOperationResponse<BackupCertificateResult>> BackupCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<BackupKeyResult>> BackupKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<BackupSecretResult>> BackupSecretWithHttpMessagesAsync(string vaultBaseUrl, string secretName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<BackupStorageResult>> BackupStorageAccountWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateOperation>> CreateCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, CertificatePolicy certificatePolicy = null, CertificateAttributes certificateAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyBundle>> CreateKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, string kty, int? keySize = null, IList<string> keyOps = null, KeyAttributes keyAttributes = null, IDictionary<string, string> tags = null, string curve = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyOperationResult>> DecryptWithHttpMessagesAsync(string vaultBaseUrl, string keyName, string keyVersion, string algorithm, byte[] value, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<Contacts>> DeleteCertificateContactsWithHttpMessagesAsync(string vaultBaseUrl, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IssuerBundle>> DeleteCertificateIssuerWithHttpMessagesAsync(string vaultBaseUrl, string issuerName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateOperation>> DeleteCertificateOperationWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedCertificateBundle>> DeleteCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedKeyBundle>> DeleteKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedSasDefinitionBundle>> DeleteSasDefinitionWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, string sasDefinitionName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedSecretBundle>> DeleteSecretWithHttpMessagesAsync(string vaultBaseUrl, string secretName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedStorageBundle>> DeleteStorageAccountWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                // noop
            }

            public Task<AzureOperationResponse<KeyOperationResult>> EncryptWithHttpMessagesAsync(string vaultBaseUrl, string keyName, string keyVersion, string algorithm, byte[] value, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<Contacts>> GetCertificateContactsWithHttpMessagesAsync(string vaultBaseUrl, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<CertificateIssuerItem>>> GetCertificateIssuersNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<CertificateIssuerItem>>> GetCertificateIssuersWithHttpMessagesAsync(string vaultBaseUrl, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IssuerBundle>> GetCertificateIssuerWithHttpMessagesAsync(string vaultBaseUrl, string issuerName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateOperation>> GetCertificateOperationWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificatePolicy>> GetCertificatePolicyWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<CertificateItem>>> GetCertificatesNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<CertificateItem>>> GetCertificatesWithHttpMessagesAsync(string vaultBaseUrl, int? maxresults = null, bool? includePending = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<CertificateItem>>> GetCertificateVersionsNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<CertificateItem>>> GetCertificateVersionsWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateBundle>> GetCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, string certificateVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedCertificateItem>>> GetDeletedCertificatesNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedCertificateItem>>> GetDeletedCertificatesWithHttpMessagesAsync(string vaultBaseUrl, int? maxresults = null, bool? includePending = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedCertificateBundle>> GetDeletedCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedKeyItem>>> GetDeletedKeysNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedKeyItem>>> GetDeletedKeysWithHttpMessagesAsync(string vaultBaseUrl, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedKeyBundle>> GetDeletedKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedSasDefinitionItem>>> GetDeletedSasDefinitionsNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedSasDefinitionItem>>> GetDeletedSasDefinitionsWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedSasDefinitionBundle>> GetDeletedSasDefinitionWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, string sasDefinitionName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedSecretItem>>> GetDeletedSecretsNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedSecretItem>>> GetDeletedSecretsWithHttpMessagesAsync(string vaultBaseUrl, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedSecretBundle>> GetDeletedSecretWithHttpMessagesAsync(string vaultBaseUrl, string secretName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedStorageAccountItem>>> GetDeletedStorageAccountsNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<DeletedStorageAccountItem>>> GetDeletedStorageAccountsWithHttpMessagesAsync(string vaultBaseUrl, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<DeletedStorageBundle>> GetDeletedStorageAccountWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<KeyItem>>> GetKeysNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<KeyItem>>> GetKeysWithHttpMessagesAsync(string vaultBaseUrl, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<KeyItem>>> GetKeyVersionsNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<KeyItem>>> GetKeyVersionsWithHttpMessagesAsync(string vaultBaseUrl, string keyName, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyBundle>> GetKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, string keyVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<string>> GetPendingCertificateSigningRequestWithHttpMessagesAsync(string vault, string certificateName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<SasDefinitionItem>>> GetSasDefinitionsNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<SasDefinitionItem>>> GetSasDefinitionsWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<SasDefinitionBundle>> GetSasDefinitionWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, string sasDefinitionName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<SecretItem>>> GetSecretsNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<SecretItem>>> GetSecretsWithHttpMessagesAsync(string vaultBaseUrl, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<SecretItem>>> GetSecretVersionsNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<SecretItem>>> GetSecretVersionsWithHttpMessagesAsync(string vaultBaseUrl, string secretName, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<SecretBundle>> GetSecretWithHttpMessagesAsync(string vaultBaseUrl, string secretName, string secretVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                if (_keyVaultErrorException != null)
                {
                    throw _keyVaultErrorException;
                }

                if (_keyValuePairs.ContainsKey(secretName))
                {
                    return Task.FromResult(new AzureOperationResponse<SecretBundle>()
                    {
                        Body = new SecretBundle(_keyValuePairs[secretName])
                    });
                }
                else
                {
                    return Task.FromResult(new AzureOperationResponse<SecretBundle>());
                }
            }

            public Task<AzureOperationResponse<IPage<StorageAccountItem>>> GetStorageAccountsNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IPage<StorageAccountItem>>> GetStorageAccountsWithHttpMessagesAsync(string vaultBaseUrl, int? maxresults = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<StorageBundle>> GetStorageAccountWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateBundle>> ImportCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, string base64EncodedCertificate, string password = null, CertificatePolicy certificatePolicy = null, CertificateAttributes certificateAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyBundle>> ImportKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, JsonWebKey key, bool? hsm = null, KeyAttributes keyAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateBundle>> MergeCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, IList<byte[]> x509Certificates, CertificateAttributes certificateAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse> PurgeDeletedCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse> PurgeDeletedKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse> PurgeDeletedSecretWithHttpMessagesAsync(string vaultBaseUrl, string secretName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse> PurgeDeletedStorageAccountWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateBundle>> RecoverDeletedCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyBundle>> RecoverDeletedKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<SasDefinitionBundle>> RecoverDeletedSasDefinitionWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, string sasDefinitionName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<SecretBundle>> RecoverDeletedSecretWithHttpMessagesAsync(string vaultBaseUrl, string secretName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<StorageBundle>> RecoverDeletedStorageAccountWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<StorageBundle>> RegenerateStorageAccountKeyWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, string keyName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateBundle>> RestoreCertificateWithHttpMessagesAsync(string vaultBaseUrl, byte[] certificateBundleBackup, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyBundle>> RestoreKeyWithHttpMessagesAsync(string vaultBaseUrl, byte[] keyBundleBackup, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<SecretBundle>> RestoreSecretWithHttpMessagesAsync(string vaultBaseUrl, byte[] secretBundleBackup, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<StorageBundle>> RestoreStorageAccountWithHttpMessagesAsync(string vaultBaseUrl, byte[] storageBundleBackup, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<Contacts>> SetCertificateContactsWithHttpMessagesAsync(string vaultBaseUrl, Contacts contacts, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IssuerBundle>> SetCertificateIssuerWithHttpMessagesAsync(string vaultBaseUrl, string issuerName, string provider, IssuerCredentials credentials = null, OrganizationDetails organizationDetails = null, IssuerAttributes attributes = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<SasDefinitionBundle>> SetSasDefinitionWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, string sasDefinitionName, string templateUri, string sasType, string validityPeriod, SasDefinitionAttributes sasDefinitionAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<SecretBundle>> SetSecretWithHttpMessagesAsync(string vaultBaseUrl, string secretName, string value, IDictionary<string, string> tags = null, string contentType = null, SecretAttributes secretAttributes = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<StorageBundle>> SetStorageAccountWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, string resourceId, string activeKeyName, bool autoRegenerateKey, string regenerationPeriod = null, StorageAccountAttributes storageAccountAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyOperationResult>> SignWithHttpMessagesAsync(string vaultBaseUrl, string keyName, string keyVersion, string algorithm, byte[] value, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyOperationResult>> UnwrapKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, string keyVersion, string algorithm, byte[] value, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<IssuerBundle>> UpdateCertificateIssuerWithHttpMessagesAsync(string vaultBaseUrl, string issuerName, string provider = null, IssuerCredentials credentials = null, OrganizationDetails organizationDetails = null, IssuerAttributes attributes = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateOperation>> UpdateCertificateOperationWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, bool cancellationRequested, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificatePolicy>> UpdateCertificatePolicyWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, CertificatePolicy certificatePolicy, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<CertificateBundle>> UpdateCertificateWithHttpMessagesAsync(string vaultBaseUrl, string certificateName, string certificateVersion, CertificatePolicy certificatePolicy = null, CertificateAttributes certificateAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyBundle>> UpdateKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, string keyVersion, IList<string> keyOps = null, KeyAttributes keyAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<SasDefinitionBundle>> UpdateSasDefinitionWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, string sasDefinitionName, string templateUri = null, string sasType = null, string validityPeriod = null, SasDefinitionAttributes sasDefinitionAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<SecretBundle>> UpdateSecretWithHttpMessagesAsync(string vaultBaseUrl, string secretName, string secretVersion, string contentType = null, SecretAttributes secretAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<StorageBundle>> UpdateStorageAccountWithHttpMessagesAsync(string vaultBaseUrl, string storageAccountName, string activeKeyName = null, bool? autoRegenerateKey = null, string regenerationPeriod = null, StorageAccountAttributes storageAccountAttributes = null, IDictionary<string, string> tags = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyVerifyResult>> VerifyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, string keyVersion, string algorithm, byte[] digest, byte[] signature, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<AzureOperationResponse<KeyOperationResult>> WrapKeyWithHttpMessagesAsync(string vaultBaseUrl, string keyName, string keyVersion, string algorithm, byte[] value, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }
    }
}
