using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;

namespace WCA.Actionstep.Client
{
    public interface IActionstepService
    {
        Uri AuthEndpoint { get; }
        Uri TokenUri { get; }
        Uri AuthorizeUri { get; }
        Uri EndSessionUri { get; }
        Uri JwtPublicKeysUri { get; }
        Uri LaunchPadUri { get; }
        Uri WebFormPostUri { get; }

        ActionstepEnvironment ActionstepEnvironment { get; }

        Task Handle(IActionstepRequest actionstepRequest);
        Task<TResponse> Handle<TResponse>(IActionstepRequest actionstepRequest);
        Task<TokenSet> RefreshAccessTokenIfExpired(TokenSet tokenSet, bool forceRefresh = false);
        Task<UploadFileResponse> UploadFile(TokenSetQuery tokenSetQuery, string fileNameWithExtension, string tempContentFilePath);
        Task<UploadFileResponse> UploadFile(TokenSetQuery tokenSetQuery, string fileNameWithExtension, Stream stream);

        /// <exception cref="InvalidJwtDiscoveryResponseException" />
        JObject GetJwtPublicKeyData();
        IEnumerable<SecurityKey> GetPublicKeys();
    }
}
