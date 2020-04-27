using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.GlobalX.Client.Authentication;
using WCA.GlobalX.Client.Documents;
using WCA.GlobalX.Client.Transactions;

namespace WCA.GlobalX.Client
{
    public interface IGlobalXService
    {
        Uri BaseApiUrl { get; }
        Uri BaseWebUrl { get; }
        GlobalXEnvironment GlobalXEnvironment { get; set; }

        #region Auth
        /// <summary>
        /// Will attempt to refresh the token for the specified user via the configured
        /// <see cref="IGlobalXApiTokenRepository"/>.
        /// 
        /// First a refresh using the refresh token will be attempted. If that fails,
        /// an attempt will be made to retrieve a new token using credentials stored
        /// in <see cref="IGlobalXCredentialsRepository"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GlobalXApiToken> RefreshAndPersistTokenForUser(string id);

        /// <summary>
        /// Safely adds or updates a token by first requesting a lock.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SafeAddOrUpdateGlobalXApiToken(GlobalXApiToken globalXApiToken);
        #endregion Auth

        #region API Requests
        Task<TransactionsResponse> GetTransactions(TransactionsQuery transactionsQuery);

        /// <summary>
        /// Retrives list of documents, metadata only, without document version information.
        /// </summary>
        /// <param name="documentsQuery"></param>
        /// <returns></returns>
        IAsyncEnumerable<Document> GetDocuments(DocumentsRequest documentsQuery);

        /// <summary>
        /// Get the metadata for a specific document, including version information.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task<Document> GetDocument(Guid documentId, string userId);

        /// <summary>
        /// Downloads the latest version of the document ID to the specified file path.
        /// </summary>
        /// <param name="documentId">The ID of the document to retrieve.</param>
        /// <param name="filePath">
        ///     The full path and filename where to save the file contents to.
        ///     If the file already exists an <see cref="System.IO.IOException"/> will be thrown.
        /// </param>
        /// <param name="userId">The userId whose permissions to use.</param>
        /// <returns></returns>
        Task<DocumentFileInfo> DownloadMostRecentDocument(Guid documentId, string filePath, string userId);

        /// <summary>
        /// Downloads the latest version of the document ID to the specified file path.
        /// </summary>
        /// <param name="documentId">The ID of the document to retrieve.</param>
        /// <param name="filePath">
        ///     The full path and filename where to save the file contents to.
        ///     If the file already exists an <see cref="System.IO.IOException"/> will be thrown.
        /// </param>
        /// <param name="documentVersionId">The specific document version ID to retrieve (if not the latest).</param>
        /// <param name="userId">The userId whose permissions to use.</param>
        /// <returns></returns>
        Task<DocumentFileInfo> DownloadDocument(Guid documentId, Guid documentVersionId, string filePath, string userId);
        #endregion API Requests
    }
}
