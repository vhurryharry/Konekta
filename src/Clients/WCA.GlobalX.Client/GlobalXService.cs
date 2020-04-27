using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WCA.GlobalX.Client.Authentication;
using WCA.GlobalX.Client.Documents;
using WCA.GlobalX.Client.Transactions;

namespace WCA.GlobalX.Client
{
    [SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Due to FlurlClient wrapper which wraps HttpClient. If you introduce other disposable fields, be sure to implement IDisposable at that time.")]
    public class GlobalXService : IGlobalXService
    {
        private const string _allScopes = "document-store national-property-information company-business transaction-api voi-api";
        private readonly IClock _clock;
        private readonly FlurlClient _flurlClient;
        private readonly IGlobalXApiTokenRepository _globalXApiTokenRepository;
        private readonly IGlobalXCredentialsRepository _globalXCredentialsRepository;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        // These must not have trailing slash!
        private const string _prodApiUrlBase = "https://gap.globalx.com.au";
        private const string _stagingApiUrlBase = "https://staging-gap.globalx.com.au";
        private const string _prodWebUrlBase = "https://online.globalx.com.au";
        private const string _stagingWebUrlBase = "https://staging-online.globalx.com.au";

        private Duration _requestLockDuration = Duration.FromMinutes(1);
        private Duration _waitForExistingLockTimeout = Duration.FromSeconds(10);
        private TimeSpan _requestLockPollingInterval = TimeSpan.FromMilliseconds(500);

        private string _apiKey;
        private string _clientId;
        private string _clientSecret;
        public GlobalXService(
            ILogger<GlobalXService> logger,
            IClock clock,
            HttpClient httpClient,
            IGlobalXApiTokenRepository globalXApiTokenRepository,
            IGlobalXCredentialsRepository globalXCredentialsRepository,
            IOptionsSnapshot<GlobalXOptions> globalXOptionsSnapshot)
        {
            if (globalXOptionsSnapshot is null) throw new ArgumentNullException(nameof(globalXOptionsSnapshot));

            _logger = logger;
            _httpClient = httpClient;
            _flurlClient = new FlurlClient(_httpClient);
            _clock = clock;
            _globalXApiTokenRepository = globalXApiTokenRepository;
            _globalXCredentialsRepository = globalXCredentialsRepository;

            GlobalXEnvironment = globalXOptionsSnapshot.Value.Environment;

            _clientId = globalXOptionsSnapshot.Value.ClientId;
            if (string.IsNullOrEmpty(_clientId)) throw new ArgumentException($"Option {nameof(globalXOptionsSnapshot.Value.ClientId)} must be set.", nameof(globalXOptionsSnapshot));

            _clientSecret = globalXOptionsSnapshot.Value.ClientSecret;
            if (string.IsNullOrEmpty(_clientSecret)) throw new ArgumentException($"Option {nameof(globalXOptionsSnapshot.Value.ClientSecret)} must be set.", nameof(globalXOptionsSnapshot));

            _apiKey = globalXOptionsSnapshot.Value.ApiKey;
            if (string.IsNullOrEmpty(_apiKey)) throw new ArgumentException($"Option {nameof(globalXOptionsSnapshot.Value.ApiKey)} must be set.", nameof(globalXOptionsSnapshot));
        }

        public Uri BaseApiUrl
        {
            get
            {
                switch (GlobalXEnvironment)
                {
                    case GlobalXEnvironment.Production:
                        return new Uri(_prodApiUrlBase);
                    default:
                        return new Uri(_stagingApiUrlBase);
                }
            }
        }

        public Uri BaseWebUrl
        {
            get
            {
                switch (GlobalXEnvironment)
                {
                    case GlobalXEnvironment.Production:
                        return new Uri(_prodWebUrlBase);
                    default:
                        return new Uri(_stagingWebUrlBase);
                }
            }
        }

        public GlobalXEnvironment GlobalXEnvironment { get; set; } = GlobalXEnvironment.Staging;
        /// <summary>
        /// For Flurl
        /// </summary>
        private string BaseApiUrlString
        {
            get
            {
                switch (GlobalXEnvironment)
                {
                    case GlobalXEnvironment.Production:
                        return _prodApiUrlBase;
                    default:
                        return _stagingApiUrlBase;
                }
            }
        }

        #region Auth
        public async Task<GlobalXApiToken> RefreshAndPersistTokenForUser(string userId)
        {
            /// First request a lock for this user's token before we do anything with it.
            /// This does not require the token to exist.
            /// <see cref="GlobalXApiTokenLockInfo"/> implements <see cref="IDisposable"/> which will
            /// delete the lock when disposed, so it does not need to be deleted explicitly in this code.
            using (var lockInfo = await RequestApiTokenLock(userId))
            {
                var existingToken = await _globalXApiTokenRepository.GetTokenForUser(userId);
                Exception refreshException = null;

                if (existingToken != null && !string.IsNullOrEmpty(existingToken.RefreshToken))
                {
                    try
                    {
                        var refreshedToken = await SendAccessTokenRefreshRequest(existingToken);
                        if (!refreshedToken.AccessTokenHasExpired(_clock))
                        {
                            return refreshedToken;
                        }
                    }
                    catch (Exception ex)
                    {
                        refreshException = ex;
                    }
                }

                var credentials = await _globalXCredentialsRepository.GetCredentialsForUser(userId);
                if (credentials is null)
                {
                    throw new GlobalXApiCredentialsNotFoundException(
                        $"Could not find an API token to refresh, nor credentials to request a new token for user '{userId}'.",
                        refreshException);
                }

                var tokenFromCredentials = await SendAccessTokenFromCredentialsRequest(credentials);
                await _globalXApiTokenRepository.AddOrUpdateGlobalXApiToken(tokenFromCredentials, lockInfo);
                return tokenFromCredentials;
            }
        }

        public async Task SafeAddOrUpdateGlobalXApiToken(GlobalXApiToken globalXApiToken)
        {
            if (globalXApiToken is null) throw new ArgumentNullException(nameof(globalXApiToken));

            using (var lockInfo = await RequestApiTokenLock(globalXApiToken.UserId))
            {
                await _globalXApiTokenRepository.AddOrUpdateGlobalXApiToken(globalXApiToken, lockInfo);
            }
        }

        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "It is the callers responsibility to dispose.")]
        private async Task<GlobalXApiTokenLockInfo> RequestApiTokenLock(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Must be supplied", nameof(userId));

            var timeoutExpiry = _clock.GetCurrentInstant().Plus(_waitForExistingLockTimeout);
            GlobalXApiTokenLockInfo currentLock;

            do
            {
                currentLock = await _globalXApiTokenRepository.GetLock(userId);

                // No existing lock, or previous lock has expired. So we can attempt to create a new lock.
                if (currentLock is null || currentLock?.ExpiresAt <= _clock.GetCurrentInstant())
                {
                    var lockInfo = new GlobalXApiTokenLockInfo(userId, _clock.GetCurrentInstant().Plus(_requestLockDuration), _globalXApiTokenRepository);
                    await _globalXApiTokenRepository.SetLock(lockInfo);

                    // Read the lock back to ensure our lock won and was set correctly.
                    var lockInfoCheck = await _globalXApiTokenRepository.GetLock(userId);

                    // Only return the lock if it was successfully saved.
                    // If these don't match, then another lock got in before we did so we should
                    // let the loop continue and try again after the next polling interval.
                    if (lockInfo?.LockId == lockInfoCheck?.LockId)
                    {
                        return lockInfoCheck;
                    }
                }

                await Task.Delay(_requestLockPollingInterval);

            } while (_clock.GetCurrentInstant() < timeoutExpiry);

            throw new GlobalXApiTokenRequestLockTimeoutException(currentLock, userId);
        }
        #endregion Auth

        #region API Requests
        public async Task<TransactionsResponse> GetTransactions(TransactionsQuery transactionsQuery)
        {
            if (transactionsQuery is null) throw new ArgumentNullException(nameof(transactionsQuery));
            var validator = new TransactionsQuery.Validator();
            validator.Validate(transactionsQuery);

            var flurlRequest = _flurlClient.Request(BaseApiUrlString)
                .AppendPathSegment("api/transactions/v3/transactions")
                .WithHeader("Accept", "application/json")
                .SetQueryParam("matterReference", transactionsQuery.MatterReference)
                .SetQueryParam("periodStart", transactionsQuery.PeriodStart)
                .SetQueryParam("periodEnd", transactionsQuery.PeriodEnd)
                .SetQueryParam("transId", transactionsQuery.TransId)
                .SetQueryParam("userType", transactionsQuery.UserType switch
                {
                    UserType.AllChildren => "allChildren",
                    UserType.JustMe => "justMe",
                    _ => null
                });

            return await GetJsonWithTokenRefreshIfRequiredAsync<TransactionsResponse>(flurlRequest, transactionsQuery.UserId);
        }

        public async IAsyncEnumerable<Document> GetDocuments(DocumentsRequest documentsQuery)
        {
            if (documentsQuery is null) throw new ArgumentNullException(nameof(documentsQuery));
            var validator = new DocumentsRequest.Validator();
            validator.Validate(documentsQuery);

            // The "statuses" parameter requires comma separated int values.
            var statuses = string.Join(',', documentsQuery.Statuses.Select(s => (int)s));
            if (string.IsNullOrEmpty(statuses))
            {
                // If empty, ensure null so that parameter is not set in query string.
                statuses = null;
            }

            // Number of documents to retrieve in a page. API Default: 10, Minimum: 10, Maximum: 200.
            const int pageSize = 200;

            // Set up base query
            var flurlRequest = _flurlClient.Request(BaseApiUrlString)
                    .AppendPathSegment("api/document-store/documents")
                    .WithHeader("Accept", "application/json")
                    .SetQueryParam("orderId", documentsQuery.OrderId)
                    .SetQueryParam("orderIdPrefix", documentsQuery.OrderIdPrefix)
                    .SetQueryParam("matterReference", documentsQuery.MatterReference)
                    .SetQueryParam("orderType", documentsQuery.OrderType)
                    .SetQueryParam("name", documentsQuery.Name)
                    .SetQueryParam("before", documentsQuery.Before)

                    // Set offset as a hack while troubleshooting date filtering behaviour of the GlobalX API.
                    .SetQueryParam("after", documentsQuery.After?.Minus(Duration.FromHours(20)).WithOffset(Offset.FromHours(10)))
                    .SetQueryParam("statuses", statuses)
                    .SetQueryParam("pageSize", pageSize);

            // The start of the page to retrieve the paginated documents. API Default: 1, Minimum: 1.
            var currentPage = 1;
            var morePages = false;

            do
            {
                var thisPageRequest = flurlRequest.SetQueryParam("pageNumber", currentPage++);
                var thisPageResult = await GetJsonWithTokenRefreshIfRequiredAsync<DocumentsResponse>(thisPageRequest, documentsQuery.UserId);

                foreach (var document in thisPageResult.Items)
                {
                    yield return document;
                }

                morePages = thisPageResult.PageNumber * thisPageResult.PageSize < thisPageResult.Total;
            } while (morePages);
        }

        public async Task<Document> GetDocument(Guid documentId, string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Must be supplied.", nameof(userId));

            return await GetJsonWithTokenRefreshIfRequiredAsync<Document>(
                _flurlClient.Request(BaseApiUrlString)
                    .AppendPathSegments("api/document-store/documents", documentId)
                    .WithHeader("Accept", "application/json"),
                userId);
        }

        public async Task<DocumentFileInfo> DownloadMostRecentDocument(Guid documentId, string filePath, string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Must be supplied.", nameof(userId));

            var mostRecentDocumentVersion = await GetJsonWithTokenRefreshIfRequiredAsync<DocumentVersion>(
                _flurlClient.Request(BaseApiUrlString)
                    .AppendPathSegments("api/document-store/documents", documentId, "versions/mostRecent")
                    .WithHeader("Accept", "application/json"),
                userId);

            if (!mostRecentDocumentVersion.DocumentVersionId.HasValue)
            {
                throw new InvalidGlobalXResponseException($"The {nameof(mostRecentDocumentVersion.DocumentVersionId)} was missing.");
            }

            return await DownloadDocument(documentId, mostRecentDocumentVersion.DocumentVersionId.Value, filePath, userId);
        }

        public async Task<DocumentFileInfo> DownloadDocument(Guid documentId, Guid documentVersionId, string filePath, string userId)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Must be supplied.", nameof(documentId));
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Must be supplied.", nameof(userId));

            if (File.Exists(filePath))
            {
                throw new IOException($"Cannot download file. Local file already exists; '{filePath}'");
            }

            // Ensure the destination directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var flurlRequest = _flurlClient.Request(BaseApiUrlString)
                    .AppendPathSegments("api/document-store/documents", documentId, "versions", documentVersionId, "blob");

            using (var response = await GetWithTokenRefreshIfRequiredAsync(flurlRequest, userId))
            {
                using (var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (var fileStream = File.OpenWrite(filePath))
                {
                    await contentStream.CopyToAsync(fileStream).ConfigureAwait(false);
                }

                var fileNameFromHeader = WebUtility.UrlDecode(
                    response.Content.Headers.ContentDisposition?.FileName
                        ?.Trim(new[] { '\'', '"' })
                        ?.Trim());

                var mimeTypeFromHeader = response.Content.Headers.ContentType?.MediaType;

                return new DocumentFileInfo(filePath, fileNameFromHeader, mimeTypeFromHeader);
            }
        }
        #endregion API Requests

        private static async Task<(JObject responseJObject, Instant receivedAt)> ParseReceivedAtAndJObject(HttpResponseMessage httpResponseMessage, IClock clock)
        {
            if (httpResponseMessage is null) throw new ArgumentNullException(nameof(httpResponseMessage));

            var receivedAt = httpResponseMessage.Headers.Date.HasValue
                ? Instant.FromDateTimeUtc(httpResponseMessage.Headers.Date.Value.UtcDateTime)
                : clock.GetCurrentInstant();

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var responseJObject = string.IsNullOrEmpty(responseContent)
                ? null
                : JObject.Parse(responseContent);

            return (responseJObject, receivedAt);
        }

        private async Task<T> GetJsonWithTokenRefreshIfRequiredAsync<T>(IFlurlRequest request, string userId)
        {
            using (var httpResponseMessage = await GetWithTokenRefreshIfRequiredAsync(request, userId))
            {
                var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
        }

        private async Task<HttpResponseMessage> GetWithTokenRefreshIfRequiredAsync(IFlurlRequest request, string userId)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Must be supplied", nameof(userId));

            // Get initial token
            var token = await _globalXApiTokenRepository.GetTokenForUser(userId);
            var tokenHasBeenRefreshed = false;

            if (token is null || token.AccessTokenHasExpired(_clock))
            {
                token = await RefreshAndPersistTokenForUser(userId);
                tokenHasBeenRefreshed = true;
            }

            // First try
            var httpResponseMessage = await request
                .AllowHttpStatus(HttpStatusCode.Unauthorized)
                .WithOAuthBearerToken(token.AccessToken)
                .GetAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return httpResponseMessage;
            }
            // We either received a different error code, or the token has already been refreshed. In either
            // case we want to throw whatever error caused this first attempt to fail.
            else if (httpResponseMessage.StatusCode != HttpStatusCode.Unauthorized || tokenHasBeenRefreshed)
            {

                // This should throw because it is not a success status code
                httpResponseMessage.EnsureSuccessStatusCode();

                /// Just in case something is broken with <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/>
                throw new InvalidGlobalXApiTokenException(token);
            }

            // Second try
            if (!tokenHasBeenRefreshed)
            {
                token = await RefreshAndPersistTokenForUser(userId);
                tokenHasBeenRefreshed = true;

                httpResponseMessage = await request
                    .WithOAuthBearerToken(token.AccessToken)
                    .GetAsync();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return httpResponseMessage;
                }
                // We either received a different error code, or the token has already been refreshed. In either
                // case we want to throw whatever error caused this first attempt to fail.
                else
                {
                    // This should throw because it is not a success status code
                    httpResponseMessage.EnsureSuccessStatusCode();
                }
            }

            /// Just in case something is broken with <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/>.
            /// This should never execute.
            throw new InvalidGlobalXApiTokenException(token);
        }

        private async Task<GlobalXApiToken> SendAccessTokenRefreshRequest(GlobalXApiToken apiToken)
        {
            if (apiToken is null) throw new ArgumentNullException(nameof(apiToken));

            HttpResponseMessage httpResponseMessage = null;

            if (string.IsNullOrEmpty(apiToken.RefreshToken))
            {
                throw new MissingRefreshTokenException();
            }

            try
            {
                httpResponseMessage = await _flurlClient.Request(BaseApiUrlString)
                    .AppendPathSegment("auth/connect/token")
                    .WithHeader("Accept", "application/json")
                    .PostUrlEncodedAsync(new
                    {
                        client_id = _clientId,
                        client_secret = _clientSecret,
                        grant_type = "refresh_token",
                        refresh_token = apiToken.RefreshToken,
                    });

                (var responseJObject, var receivedAt) = await ParseReceivedAtAndJObject(httpResponseMessage, _clock);
                return new GlobalXApiToken(responseJObject, receivedAt, apiToken.UserId);
            }
            catch (FlurlHttpException fex)
            {
                (var responseJObject, var receivedAt) = await ParseReceivedAtAndJObject(fex.Call.Response, _clock);
                throw new RefreshGlobalXApiTokenErrorResponseException(apiToken, responseJObject, fex.Call.Response.StatusCode, receivedAt);
            }
            finally
            {
                if (!(httpResponseMessage is null))
                {
                    httpResponseMessage.Dispose();
                }
            }
        }

        private async Task<GlobalXApiToken> SendAccessTokenFromCredentialsRequest(GlobalXCredentials globalXCredentials)
        {
            if (globalXCredentials is null) throw new ArgumentNullException(nameof(globalXCredentials));

            HttpResponseMessage httpResponseMessage = null;

            try
            {
                httpResponseMessage = await _flurlClient.Request(BaseApiUrlString)
                    .AppendPathSegment("/auth/connect/token")
                    .PostUrlEncodedAsync(new
                    {
                        client_id = _clientId,
                        client_secret = _clientSecret,
                        grant_type = "password",
                        username = globalXCredentials.Username,
                        password = globalXCredentials.Password,
                        scope = _allScopes,
                    });

                (var responseJObject, var receivedAt) = await ParseReceivedAtAndJObject(httpResponseMessage, _clock);
                return new GlobalXApiToken(responseJObject, receivedAt, globalXCredentials.UserId);
            }
            catch (FlurlHttpException fex)
            {
                (var responseJObject, var receivedAt) = await ParseReceivedAtAndJObject(fex.Call.Response, _clock);
                throw new InvalidGlobalXCredentialsException(globalXCredentials.UserId, responseJObject);
            }
            finally
            {
                if (!(httpResponseMessage is null))
                {
                    httpResponseMessage.Dispose();
                }
            }
        }
    }
}
