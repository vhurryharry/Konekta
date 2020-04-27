using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;

namespace WCA.Actionstep.Client
{
    public class ActionstepService : IActionstepService
    {
        internal const string ActionstepAcceptMediaType = "application/vnd.api+json";
        private const int RefreshAccessTokenMaxRetryCount = 4;
        private const string JwtDiscoveryCacheKey = "JwtDiscoveryCacheKey";
        private const int JwtDiscoveryCacheTimeoutInHours = 24;

        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly ITokenSetRepository _tokenSetRepository;
        private readonly IClock _clock;
        private readonly IMemoryCache _memoryCache;
        private readonly Duration _accessTokenExpiryBuffer = Duration.FromMinutes(2);
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly Func<int, TimeSpan> _calculateNextDelay = retryAttempt => TimeSpan.FromMilliseconds(retryAttempt * 750);

        // These must not have trailing slash!
        private const string _stagingDomain = "actionstepstaging.com";
        private const string _productionDomain = "actionstep.com";

        private readonly string _authUrlBaseTemplate = "https://api.{0}/api";
        private readonly string _launchPadUrlTemplate = "https://api.{0}/mym/asfw";
        private readonly string _webFormPostUrlTemplate = "https://go.{0}/frontend/application/webform/post";

        public ActionstepEnvironment ActionstepEnvironment { get; } = ActionstepEnvironment.Staging;

        public Uri AuthEndpoint => CreateEnvironmentUriFromTemplate(_authUrlBaseTemplate);

        public Uri TokenUri => new Uri(AuthEndpoint, Path.Combine(AuthEndpoint.AbsolutePath, "oauth/token"));
        public Uri AuthorizeUri => new Uri(AuthEndpoint, Path.Combine(AuthEndpoint.AbsolutePath, "oauth/authorize"));

        /// <summary>
        /// This isn't documented on the Actionstep API Wiki. Not sure if this works
        /// See https://actionstep.atlassian.net/wiki/spaces/API/pages/12025899/Authorization
        /// </summary>
        public Uri EndSessionUri => new Uri(AuthEndpoint, Path.Combine(AuthEndpoint.AbsolutePath, "oauth/logout"));

        /// <summary>
        /// Contains a JSON file with the following properties. Each key value contains an RS256 key string starting with "-----BEGIN PUBLIC KEY-----":
        /// <see cref="JwtPublicKeyIds"/>
        /// </summary>
        public Uri JwtPublicKeysUri => new Uri("https://cdn.actionstep.com/jwt-discovery-public.json", UriKind.Absolute);

        public Uri LaunchPadUri => CreateEnvironmentUriFromTemplate(_launchPadUrlTemplate);
        public Uri WebFormPostUri => CreateEnvironmentUriFromTemplate(_webFormPostUrlTemplate);

        private Uri CreateEnvironmentUriFromTemplate(string template) =>
            new Uri(
                string.Format(
                    CultureInfo.InvariantCulture,
                    template,
                    (ActionstepEnvironment == ActionstepEnvironment.Production) ? _productionDomain : _stagingDomain),
                UriKind.Absolute);

        public IEnumerable<SecurityKey> GetPublicKeys()
        {
            return _memoryCache.GetOrCreate<IEnumerable<SecurityKey>>($"{JwtDiscoveryCacheKey}-{ActionstepEnvironment}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(JwtDiscoveryCacheTimeoutInHours);

                // We can vary based on environment because the environment forms part of the cache key
                var keyIds = ActionstepEnvironment == ActionstepEnvironment.Production
                    ? new[] { JwtPublicKeyIds.ProdPublicKey, JwtPublicKeyIds.ProdPublicKeyOld }
                    : new[] { JwtPublicKeyIds.LabsPublicKey, JwtPublicKeyIds.LabsPublicKeyOld };

                var keysJObject = GetJwtPublicKeyData();
                var securitykeys = new List<SecurityKey>();

                foreach (var keyId in keyIds)
                {
                    var thisKey = keysJObject.Value<string>(keyId);
                    if (!string.IsNullOrEmpty(thisKey))
                    {
                        securitykeys.Add(new RsaSecurityKey(RSAParametersFromPem(thisKey)));
                    }
                }

                return securitykeys.ToArray();
            });
        }

        private static RSAParameters RSAParametersFromPem(string pemString)
        {
            if (string.IsNullOrEmpty(pemString)) throw new ArgumentException("Must be supplied.", nameof(pemString));

            using (var pemStringReader = new StringReader(pemString))
            {
                var publicKeyParam = (RsaKeyParameters)new PemReader(pemStringReader).ReadObject();
                var rsaParameters = new RSAParameters();
                rsaParameters.Modulus = publicKeyParam.Modulus.ToByteArrayUnsigned();
                rsaParameters.Exponent = publicKeyParam.Exponent.ToByteArrayUnsigned();
                return rsaParameters;
            }
        }

        /// <summary>
        /// Should be enough time for timeouts and retries to have finished. This is really more of a fall-back.
        /// If the refresh fails after internal retries, then it probably won't be able to be refreshed anyway.
        /// </summary>
        private Duration _tokenRefreshLockDuration = Duration.FromMinutes(30);

        public ActionstepService(
            ILogger<ActionstepService> logger,
            HttpClient httpClient,
            ActionstepServiceConfigurationOptions actionstepServiceConfigurationOptions,
            ITokenSetRepository tokenHandler,
            IClock clock,
            IMemoryCache memoryCache)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
            _logger.LogDebug((int)LogEventId.MethodEntry, nameof(ActionstepService));

            if (actionstepServiceConfigurationOptions is null) throw new ArgumentNullException(nameof(actionstepServiceConfigurationOptions));

            if (string.IsNullOrEmpty(actionstepServiceConfigurationOptions.ClientId))
            {
                throw new ArgumentException(
                    $"The property '{nameof(ActionstepServiceConfigurationOptions.ClientId)}' is required.",
                    nameof(actionstepServiceConfigurationOptions));
            }

            if (string.IsNullOrEmpty(actionstepServiceConfigurationOptions.ClientSecret))
            {
                throw new ArgumentException(
                    $"The property '{nameof(ActionstepServiceConfigurationOptions.ClientSecret)}' is required.",
                    nameof(actionstepServiceConfigurationOptions));
            }

            ActionstepEnvironment = actionstepServiceConfigurationOptions.ActionstepEnvironment;

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _clientId = actionstepServiceConfigurationOptions.ClientId;
            _clientSecret = actionstepServiceConfigurationOptions.ClientSecret;

            _tokenSetRepository = tokenHandler;
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            _jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            _jsonSerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

            _logger.LogDebug((int)LogEventId.MethodExit, nameof(ActionstepService));
        }

        /// <exception cref="InvalidJwtDiscoveryResponseException" />
        public JObject GetJwtPublicKeyData()
        {
            try
            {
                var stringResponse = _httpClient.GetStringAsync(JwtPublicKeysUri)
                    .ConfigureAwait(false)
                    .GetAwaiter().GetResult();

                if (string.IsNullOrEmpty(stringResponse))
                {
                    throw new InvalidJwtDiscoveryResponseException("The JWT Discovery public key response was empty.");
                }

                return JObject.Parse(stringResponse);
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidJwtDiscoveryResponseException("There was a problem retrieving the JWT Discovery public key data.", ex);
            }
            catch (JsonReaderException ex)
            {
                throw new InvalidJwtDiscoveryResponseException("There was a problem parsing the JWT Discovery public key response.", ex);
            }
        }

        public async Task Handle(IActionstepRequest request)
        {
            _logger.LogDebug((int)LogEventId.MethodEntry, nameof(Handle));

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace($"Handle called." +
                $" Http Method: {request?.HttpMethod?.ToString()}" +
                $", Relative Resource Path; {request?.RelativeResourcePath}" +
                $", Org Key: {request?.TokenSetQuery?.OrgKey}" +
                $", User ID: {request?.TokenSetQuery?.UserId}" +
                $", Json Payload: {request?.JsonPayload}");
            }

            await Handle<dynamic>(request);

            _logger.LogDebug((int)LogEventId.MethodExit, nameof(Handle));
        }

        /// <summary>
        /// TODO: Make IActionstepRequest generic to specify its result
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="BadActionstepApiResponseException">If the Actionstep API returns a non successful result (i.e. not between 200-299).</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TResponse> Handle<TResponse>(IActionstepRequest request)
        {
            _logger.LogDebug((int)LogEventId.MethodExit, $"{nameof(Handle)}<TResponse>");

            if (request is null) throw new ArgumentNullException(nameof(request));

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace($"Handle called." +
                    $" Http Method: {request?.HttpMethod?.ToString()}" +
                    $", Relative Resource Path; {request?.RelativeResourcePath}" +
                    $", Org Key: {request?.TokenSetQuery?.OrgKey}" +
                    $", User ID: {request?.TokenSetQuery?.UserId}" +
                    $", Json Payload: {request?.JsonPayload}");
            }

            var response = await SendApiRequest(request);

            if (response.IsSuccessStatusCode)
            {
                if (response.Content is null)
                {
                    _logger.LogDebug($"Received successful response, with no content, returning default value of type {typeof(TResponse).FullName}.");

                    return default;
                }
                else
                {
                    _logger.LogDebug($"Received successful response with content, attempting to deserialise value to type {typeof(TResponse).FullName}.");

                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(responseBody, _jsonSerializerSettings);
                }
            }
            else
            {
                _logger.LogDebug("Received non succesful response '{StatusCode}'.", response.StatusCode);
                var responseBody = string.Empty;

                try
                {
                    if (!(response.Content is null))
                    {
                        _logger.LogDebug("Attempting to read ActionstepError response content.");

                        responseBody = await response.Content.ReadAsStringAsync();
                    }

                    // We do this to throw the underlying Http exception.
                    response.EnsureSuccessStatusCode();

                    /// Required because the compiler can't guarantee that <see cref="HttpResponseMessage.EnsureSuccessStatusCode" />
                    /// will throw. Although we're very confident that it will because we're in the else block after checking
                    /// <see cref="HttpResponseMessage.IsSuccessStatusCode" />.
                    return default;
                }
                catch (Exception ex)
                {
                    throw new BadActionstepApiResponseException(ex, responseBody);
                }
            }
        }

        private async Task<HttpResponseMessage> SendApiRequest(IActionstepRequest actionstepRequest)
        {
            _logger.LogDebug((int)LogEventId.MethodEntry, nameof(SendApiRequest));

            if (actionstepRequest is null) throw new ArgumentNullException(nameof(actionstepRequest));
            if (actionstepRequest.TokenSetQuery == null) throw new ArgumentException(
                "No TokenSet (credentials) ID was supplied. Cannot send Actionstep API request as all requests must be authenticated.");

            var tokenSet = await _tokenSetRepository.GetTokenSet(actionstepRequest.TokenSetQuery);

            if (tokenSet is null)
            {
                throw new InvalidTokenSetException($"TokenSet not found for user '{actionstepRequest.TokenSetQuery.UserId}' and org '{actionstepRequest.TokenSetQuery.OrgKey}'.");
            }

            /// NOTE: Disabled to avoid accidental revokation. 
            ///       Tokens are refreshed within access token expiry by a timer job.
            //const bool initialRefreshForceRefresh = false;

            //_logger.LogDebug(
            //    "About to call {RefreshAccessTokenIfExpired} before sending request." +
            //    " TokenSet ID: '{TokenSetId}', forceRefresh: {ForceRefresh}",
            //    nameof(RefreshAccessTokenIfExpired), tokenSet.Id, initialRefreshForceRefresh);

            // tokenSet = await RefreshAccessTokenIfExpired(tokenSet, forceRefresh: initialRefreshForceRefresh);

            using (var actionstepHttpRequestMessage = new ActionstepHttpRequestMessage(actionstepRequest, this, tokenSet))
            {
                ByteArrayContent byteArrayContent = null;

                try
                {
                    /// We don't need to set the <see cref="HttpRequestMessage.RequestUri"/> because it will be set
                    /// in the <see cref="AuthDelegatingHandler"/>. It is set there because it could change if the
                    /// TokenSet is updated, and is therefore an Auth concern.

                    actionstepHttpRequestMessage.Method = actionstepRequest.HttpMethod;
                    actionstepHttpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ActionstepAcceptMediaType));

                    if (actionstepRequest is UploadFileRequest fileRequest)
                    {
                        byteArrayContent = new ByteArrayContent(fileRequest.FileContent);
                        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");//("Content-Type", "application/octet-stream");
                        byteArrayContent.Headers.Add($"Content-Disposition", $"form-data; name=\"file\"; filename=\"{fileRequest.FileName}\"");
                        var multipartFormData = new MultipartFormDataContent($"----multipart-boundary-{Guid.NewGuid()}----");
                        multipartFormData.Add(byteArrayContent, Path.GetFileNameWithoutExtension(fileRequest.FileName), fileRequest.FileName);
                        actionstepHttpRequestMessage.Content = multipartFormData;
                    }
                    else
                    {
                        if (actionstepRequest.JsonPayload != null)
                        {
                            actionstepHttpRequestMessage.Content = new StringContent(
                                JsonConvert.SerializeObject(actionstepRequest.JsonPayload, _jsonSerializerSettings));

                            actionstepHttpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(ActionstepAcceptMediaType);
                        }
                    }

                    /// The <see cref="AuthDelegatingHandler"/> will handle forcefully refreshing the token and
                    /// retrying if a 401 is received, so we don't need to retry here. Plus, we cannot re-use the
                    /// <see cref="HttpRequestMessage"/> here as the following exception will be thrown:
                    ///     System.InvalidOperationException : The request message was already sent. Cannot send the same request message multiple times.
                    return await _httpClient.SendAsync(actionstepHttpRequestMessage);
                }
                finally
                {
                    if (byteArrayContent != null)
                    {
                        byteArrayContent.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tokenSetQuery"></param>
        /// <param name="filePath"></param>
        /// <param name="fileNameWithExtension"></param>
        /// <param name="fileNameWithoutExtension"></param>
        /// <returns>A <see cref="UploadFileResponse"/> from the upload. If the file is uploaded in multiple chunks, then only the latest/last <see cref="UploadFileResponse"/> is returned.</returns>
        public async Task<UploadFileResponse> UploadFile(TokenSetQuery tokenSetQuery, string fileNameWithExtension, string tempContentFilePath)
        {
            if (!File.Exists(tempContentFilePath))
            {
                throw new FileNotFoundException(
                    $"Couldn't upload file '{fileNameWithExtension ?? "null"}' to Actionstep. The temporary file to be uploaded was not found. Expected it at '{tempContentFilePath ?? "null"}'.",
                    tempContentFilePath);
            }

            using (var fileStream = new FileStream(tempContentFilePath, FileMode.Open))
            {
                return await UploadFile(tokenSetQuery, fileNameWithExtension, fileStream);
            }
        }

        public async Task<UploadFileResponse> UploadFile(TokenSetQuery tokenSetQuery, string fileNameWithExtension, Stream stream)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead) throw new InvalidOperationException(
                "Cannot upload file to Actionstep, the stream containing the file contents cannot be read.");

            if (stream.Position != 0 && !stream.CanSeek) throw new InvalidOperationException(
                "Cannot upload file to Actionstep, the position of the stream containing the file contents is not zero, and seeking is not possible.");

            decimal totalSize = stream.Length;
            const int maxChunkSize = 5 * 1024 * 1024; // 5MB
            int partCount = (int)Math.Ceiling(totalSize / maxChunkSize);
            var previousChunk = new UploadFileResponse();

            for (int partNumber = 1; partNumber <= partCount; partNumber++)
            {
                int bufferSize = maxChunkSize;
                if (partNumber == partCount)
                {
                    bufferSize = (int)totalSize - ((partNumber - 1) * maxChunkSize);
                }

                byte[] chunkContent = new byte[bufferSize];

                await stream.ReadAsync(chunkContent, maxChunkSize * (partNumber - 1), bufferSize);
                var uploadFileChunk = new UploadFileRequest()
                {
                    TokenSetQuery = tokenSetQuery,
                    PartCount = partCount,
                    FileName = fileNameWithExtension,
                    PartNumber = partNumber,
                    FileId = partNumber == 1 ? null : previousChunk.File.Id,
                    FileContent = chunkContent
                };

                previousChunk = await Handle<UploadFileResponse>(uploadFileChunk);
            }

            return previousChunk;
        }

        public async Task<TokenSet> RefreshAccessTokenIfExpired(TokenSet tokenSet, bool forceRefresh = false)
        {
            if (tokenSet is null) throw new ArgumentNullException(nameof(tokenSet));
            var tokenRefreshCorrelationId = Guid.NewGuid();

            if (!forceRefresh && tokenSet.AccessTokenAppearsValid(_clock, _accessTokenExpiryBuffer))
            {
                return tokenSet;
            }

            _logger.LogDebug(
                "Refresh Correlation Id {TokenRefreshCorrelationId}: About to start RetryableOperation to refresh TokenSet ID: {TokenSetId}." +
                " Maximum retry count: {RefreshAccessTokenMaxRetryCount}",
                tokenRefreshCorrelationId, tokenSet.Id, RefreshAccessTokenMaxRetryCount);

            return await Helper.RetryableOperation(
                async () => await RefreshAndPersistAccessTokenWithLock(tokenSet, tokenRefreshCorrelationId, forceRefresh),

                /// If we encounter <see cref="TokenSetAlreadyLockedForRefreshException" /> it means another process
                /// is currently trying to refresh this token. We should retry because by the time we retry the other
                /// refresh operation should have finished.
                ///
                /// If we encounter <see cref="FailedToLockTokenForRefreshException" /> it means that another process
                /// likely locked this token in between our call and our request to lock it. We should retry because
                /// by the time we retry hopefully the other refresh hsould have finished.
                ex => Task.FromResult(
                    ex is TokenSetAlreadyLockedForRefreshException
                    || ex is TokenSetConcurrencyException
                    || ex is FailedToLockTokenForRefreshException),

                RefreshAccessTokenMaxRetryCount,

                _calculateNextDelay);
        }

        private async Task<TokenSet> RefreshAndPersistAccessTokenWithLock(TokenSet tokenSet, Guid refreshCorrelationId, bool forceRefresh = false)
        {
            if (tokenSet is null) throw new ArgumentNullException(nameof(tokenSet));

            if (!forceRefresh && tokenSet.AccessTokenAppearsValid(_clock, _accessTokenExpiryBuffer))
            {
                _logger.LogDebug((int)LogEventId.WillNotRefreshTokenSet,
                    "Refresh Correlation Id {TokenRefreshCorrelationId}: Will not refresh TokenSet. Force Refresh is {ForceRefresh} and access token has not yet expired. Token Set ID: {TokenSetId}",
                    refreshCorrelationId, forceRefresh, tokenSet.Id);
                return tokenSet;
            }

            // Perform pessimistic lock check
            var tokenBeingRefreshed = await _tokenSetRepository.GetTokenSetById(tokenSet.Id);

            if (tokenBeingRefreshed is null)
            {
                // We'll use the existing tokenSet. It'll get added when we attempt to lock it.
                _logger.LogDebug(
                    (int)LogEventId.TokenSetNotFoundForUpdate,
                    "Refresh Correlation Id {TokenRefreshCorrelationId}: Refresh of token was requested, but we couldn't find the token in the repository. " +
                    "We'll process the TokenSet as retrieved, and it will be added to the repository " +
                    "after it is refreshed. TokenSet ID: '{TokenSetId}', TokenSet User ID: '{TokenSetUserId}', TokenSet OrgKey: '{TokenSetOrgKey}'",
                    refreshCorrelationId, tokenSet.Id, tokenSet.UserId, tokenSet.OrgKey);
                tokenBeingRefreshed = tokenSet;
            }
            else if (tokenBeingRefreshed.RevokedAt.HasValue)
            {
                _logger.LogDebug(
                    (int)LogEventId.TokenSetRevoked,
                    "Refresh Correlation Id {TokenRefreshCorrelationId}: Refresh of token was requested, but the TokenSet was previously marked as revoked at '{TokenSetRevokedAt}'. " +
                    "TokenSet ID: '{TokenSetId}', TokenSet User ID: '{TokenSetUserId}', TokenSet OrgKey: '{TokenSetOrgKey}'",
                    refreshCorrelationId, tokenSet.RevokedAt.Value, tokenSet.Id, tokenSet.UserId, tokenSet.OrgKey);
                throw new InvalidTokenSetException($"The TokenSet was revoked at {tokenBeingRefreshed.RevokedAt.Value}.", tokenBeingRefreshed);
            }
            else
            {
                // If we're here it means that we were able to retrieve the token from the repository. Now we'll
                // re-check the retrieved token just in case it was refreshed by another process since we were called.
                if (!forceRefresh && tokenBeingRefreshed.AccessTokenAppearsValid(_clock, _accessTokenExpiryBuffer))
                {
                    _logger.LogDebug((int)LogEventId.WillNotRefreshTokenSet,
                        "Refresh Correlation Id {TokenRefreshCorrelationId}: Will not refresh TokenSet. Force Refresh is {ForceRefresh} and access token has not yet expired. Token Set ID: {TokenSetId}",
                        refreshCorrelationId, forceRefresh, tokenSet.Id);
                    return tokenBeingRefreshed;
                }

                if (tokenBeingRefreshed.RefreshLockInfo != null)
                {
                    if (tokenBeingRefreshed.RefreshLockInfo.LockExpiresAt > _clock.GetCurrentInstant())
                    {
                        var tokenSetAlreadyLockedForRefreshException = new TokenSetAlreadyLockedForRefreshException(tokenBeingRefreshed);
                        _logger.LogDebug((int)LogEventId.CannotRefreshTokenSetDueToExistingLock, tokenSetAlreadyLockedForRefreshException,
                            "Refresh Correlation Id {TokenRefreshCorrelationId}: Could not refresh TokenSet due to existing lock.",
                            refreshCorrelationId);
                        throw tokenSetAlreadyLockedForRefreshException;
                    }
                }
            }

            // Request lock
            _logger.LogDebug((int)LogEventId.AttemptToLockTokenSet, "Refresh Correlation Id {TokenRefreshCorrelationId}: About to request lock for TokenSet ID: {TokenSetId}", refreshCorrelationId, tokenSet.Id);
            var lockInfo = tokenBeingRefreshed.LockForRefresh(_clock.GetCurrentInstant().Plus(_tokenRefreshLockDuration));
            await _tokenSetRepository.AddOrUpdateTokenSet(tokenBeingRefreshed);
            _logger.LogDebug((int)LogEventId.TokenSetLocked, "Refresh Correlation Id {TokenRefreshCorrelationId}: Lock achieved for TokenSet ID: {TokenSetId}, Lock ID: {LockInfoLockId}, Lock Expires At: {LockInfoLockExpiresAt}",
                refreshCorrelationId, tokenSet.Id, lockInfo.LockId, lockInfo.LockExpiresAt);

            // Read the token again to make sure that our save was successful
            tokenBeingRefreshed = await _tokenSetRepository.GetTokenSetById(tokenSet.Id);

            // The retrieved token should have our lock id. This indicates a successful lock for our process.
            if (lockInfo.LockId != tokenBeingRefreshed.RefreshLockInfo?.LockId)
            {
                _logger.LogDebug("Refresh Correlation Id {TokenRefreshCorrelationId}: Couldn't lock token '{TokenSetId}' for refresh. This is likely because another process is also trying to refresh the same token and got in before us.",
                    refreshCorrelationId, tokenSet.Id);
                throw new FailedToLockTokenForRefreshException(
                    $"Refresh Correlation Id {refreshCorrelationId}: Couldn't lock token '{tokenSet.Id}' for refresh. This is likely because another process is also trying to refresh the same token and got in before us.",
                    tokenSet.Id);
            }

            try
            {
                _logger.LogDebug("Refresh Correlation Id {TokenRefreshCorrelationId}: Sending Refresh Request to Actionstep for TokenSet ID: {TokenSetId}", refreshCorrelationId, tokenSet.Id);
                tokenBeingRefreshed = await SendRefreshRequest(tokenBeingRefreshed);
            }
            catch (RefreshTokenErrorResponseException refreshTokenErrorResponseException)
            {
                // If the token couldn't be refreshed, remove the lock and mark it as revoked
                tokenBeingRefreshed.MarkRevoked(refreshTokenErrorResponseException.ReceivedAt);
                tokenBeingRefreshed.ResetRefreshLock();
                await _tokenSetRepository.AddOrUpdateTokenSet(tokenBeingRefreshed);
                throw new InvalidTokenSetException($"Refresh Correlation Id {refreshCorrelationId}: There was an error refreshing the TokenSet. TokenSet has been revoked.", refreshTokenErrorResponseException);
            }

            /// Store the refreshed token back in the repository. This new token won't have
            /// its <see cref="TokenSet.RefreshLockInfo" /> set, thereby unlocking the token
            /// at the same time.
            _logger.LogDebug("Refresh Correlation Id {TokenRefreshCorrelationId}: Token refreshed, about to call {AddOrUpdateTokenSet} to persist Token Set data. Token Set ID: {TokenSetId}",
                refreshCorrelationId, nameof(ITokenSetRepository.AddOrUpdateTokenSet), tokenSet.Id);
            await _tokenSetRepository.AddOrUpdateTokenSet(tokenBeingRefreshed);
            _logger.LogDebug("Refresh Correlation Id {TokenRefreshCorrelationId}: Call to {AddOrUpdateTokenSet} complete. Token Set ID: {TokenSetId}",
                refreshCorrelationId, nameof(ITokenSetRepository.AddOrUpdateTokenSet), tokenSet.Id);

            return tokenBeingRefreshed;
        }

        private async Task<TokenSet> SendRefreshRequest(TokenSet tokenSet)
        {
            HttpResponseMessage response;
            string responseContent = null;

            using (var request = new HttpRequestMessage(HttpMethod.Post, TokenUri))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}")));
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ActionstepAcceptMediaType));
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", tokenSet.RefreshToken },
                });

                response = await _httpClient.SendAsync(request);

                responseContent = await response.Content.ReadAsStringAsync();

                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace(
                        "Received response from Actionstep token endpoint." + Environment.NewLine +
                        $" Request Headers: {response.RequestMessage.Headers.ToString()}" + Environment.NewLine +
                        $" Request Content: {await response.RequestMessage.Content.ReadAsStringAsync()}" + Environment.NewLine +
                        $" Response Status Code: {response.StatusCode}" + Environment.NewLine +
                        $" Response Headers: {response.Headers.ToString()}" + Environment.NewLine +
                        $" Response Content: {await response.Content.ReadAsStringAsync()}");
                }
            }

            var receivedAt = response.Headers.Date.HasValue
                        ? Instant.FromDateTimeUtc(response.Headers.Date.Value.UtcDateTime)
                        : _clock.GetCurrentInstant();

            var responseJObject = string.IsNullOrEmpty(responseContent)
                ? null
                : JObject.Parse(responseContent);

            if (response.IsSuccessStatusCode)
            {
                return new TokenSet(responseJObject, receivedAt, tokenSet.UserId, tokenSet.Id);
            }
            else
            {
                throw new RefreshTokenErrorResponseException(tokenSet, responseJObject, response.StatusCode, receivedAt);
            }
        }
    }
}
