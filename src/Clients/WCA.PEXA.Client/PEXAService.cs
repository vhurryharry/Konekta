using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WCA.PEXA.Client.Resources;

namespace WCA.PEXA.Client
{
    public class PEXAService : IPEXAService
    {
        private readonly HttpClient _httpClient;

        // These must not have trailing slash!
        private readonly string _testAuthUrlBase = "https://auth-tst.pexalabs.com.au";
        private readonly string _prodAuthUrlBase = "https://auth.pexa.com.au";
        private readonly string _testApiUrlBase = "https://api-tst.pexalabs.com.au/api/rest";
        private readonly string _prodApiUrlBase = "https://api.pexa.com.au/api/rest";

        public PEXAEnvironment PEXAEnvironment { get; set; } = PEXAEnvironment.Test;

        public Uri AuthUrlBase
        {
            get
            {
                switch (PEXAEnvironment)
                {
                    case PEXAEnvironment.Production:
                        return new Uri(_prodAuthUrlBase);
                    default:
                        return new Uri(_testAuthUrlBase);
                }
            }
        }

        public Uri ApiUrlBase
        {
            get
            {
                switch (PEXAEnvironment)
                {
                    case PEXAEnvironment.Production:
                        return new Uri(_prodApiUrlBase);
                    default:
                        return new Uri(_testApiUrlBase);
                }
            }
        }

        public PEXAService(HttpClient httpClient, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _httpClient = httpClient;
            PEXAEnvironment = (PEXAEnvironment)Enum.Parse(typeof(PEXAEnvironment), configuration["WCACoreSettings:PEXASettings:Environment"].ToString(CultureInfo.InvariantCulture));
        }

        public Uri GetWorkspaceUri(string workspaceId, PexaRole workspaceRole)
        {
            if (string.IsNullOrEmpty(workspaceId))
            {
                throw new ArgumentNullException(nameof(workspaceId));
            }

            var urlSafeId = Uri.EscapeDataString(workspaceId);

            var urlSafeRole = Uri.EscapeDataString(TryGetXmlName(workspaceRole));

            switch (PEXAEnvironment)
            {
                case PEXAEnvironment.Test:
                    return new Uri($"https://api-tst.pexalabs.com.au/pexa_web/dl/workspaces/{urlSafeId}?role={urlSafeRole}");
                case PEXAEnvironment.Production:
                    return new Uri($"https://api.pexa.com.au/pexa_web/dl/workspaces/{urlSafeId}?role={urlSafeRole}");
                default:
                    throw new InvalidDataException("Unknown PEXAEnvironment.");
            }
        }

        public Uri GetInvitationUri(string workspaceId, PexaRole workspaceRole)
        {
            if (string.IsNullOrEmpty(workspaceId))
            {
                throw new ArgumentNullException(nameof(workspaceId));
            }

            var urlSafeId = Uri.EscapeDataString(workspaceId);
            urlSafeId = urlSafeId.Substring(6);

            var urlSafeRole = Uri.EscapeDataString(TryGetXmlName(workspaceRole));

            switch (PEXAEnvironment)
            {
                case PEXAEnvironment.Test:
                    return new Uri($"https://api-tst.pexalabs.com.au/pexa_web/displayParticipantInvitationPage.html?&workspaceReference={urlSafeId}&roleId={urlSafeRole}#");
                case PEXAEnvironment.Production:
                    return new Uri($"https://api.pexa.com.au/pexa_web/displayParticipantInvitationPage.html?&workspaceReference={urlSafeId}&roleId={urlSafeRole}#");
                default:
                    throw new InvalidDataException("Unknown PEXAEnvironment.");
            }
        }

        public static string TryGetXmlName(Enum enumValue)
        {
            if (enumValue is null)
            {
                throw new ArgumentNullException(nameof(enumValue));
            }

            try
            {
                var enumType = enumValue.GetType();
                var valueMethodInfo = enumType.GetMember(enumValue.ToString());

                if (valueMethodInfo.Length > 0)
                {
                    var xmlEnumAttribute = (XmlEnumAttribute)Attribute.GetCustomAttribute(valueMethodInfo[0], typeof(XmlEnumAttribute));

                    // Try to avoid exceptions if the attribute isn't present
                    if (xmlEnumAttribute != null)
                    {
                        return xmlEnumAttribute.Name;
                    }
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types: We don't want this to fail
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            { /* No-op */ }

            // Fall back to enum value
            return enumValue.ToString();
        }

        public async Task<TResponse> Handle<TResponse>(PEXARequestBase request, CancellationToken cancellationToken)
            where TResponse : class
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = await SendApiRequest(request, cancellationToken);

            try
            {
                var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h_ => h_.Key, h_ => h_.Value);
                if (response.Content != null && response.Content.Headers != null)
                {
                    foreach (var item_ in response.Content.Headers)
                        headers[item_.Key] = item_.Value;
                }

                var status_ = ((int)response.StatusCode).ToString(CultureInfo.InvariantCulture);
                if (status_ == "200" || status_ == "206")
                {
                    if (response.Content == null)
                    {
                        return null;
                    }

                    var workspaceCreationResponseSerializer = new XmlSerializer(typeof(TResponse));
                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var xmlReader = XmlReader.Create(contentStream))
                    {
                        return (TResponse)workspaceCreationResponseSerializer.Deserialize(xmlReader);
                    }
                }
                else if (status_ == "400")
                {
                    if (response.Content == null) throw new PEXAException();

                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var xmlReader = XmlReader.Create(contentStream))
                    {
                        ExceptionResponse exceptionResponse;
                        if (request.Version == 1)
                        {
                            var exceptionResponseSerializerv1 = new XmlSerializer(typeof(ExceptionResponsev1));
                            exceptionResponse = (ExceptionResponse)exceptionResponseSerializerv1.Deserialize(xmlReader);
                        }
                        else
                        {
                            var exceptionResponseSerializerv2 = new XmlSerializer(typeof(ExceptionResponsev2));
                            exceptionResponse = (ExceptionResponse)exceptionResponseSerializerv2.Deserialize(xmlReader);
                        }

                        if (xmlReader != null)
                        {
                            xmlReader.Dispose();
                        }

                        throw new PEXAException(exceptionResponse);
                    }
                }
                else if (status_ == "401")
                {
                    var responseData_ = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new PEXAException("Unauthenticated", (int)response.StatusCode, responseData_, headers, null);
                }
                else if (status_ == "403")
                {
                    var responseData_ = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new PEXAException("Forbidden", (int)response.StatusCode, responseData_, headers, null);
                }
                else if (status_ != "200" && status_ != "204")
                {
                    var responseData_ = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new PEXAException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", (int)response.StatusCode, responseData_, headers, null);
                }

                return null;
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
        }

        private async Task<HttpResponseMessage> SendApiRequest(PEXARequestBase pexaRequest, CancellationToken cancellationToken)
        {
            if (pexaRequest is null)
            {
                throw new ArgumentNullException(nameof(pexaRequest));
            }

            if (string.IsNullOrEmpty(pexaRequest.BearerToken))
            {
                throw new ArgumentException(nameof(pexaRequest.BearerToken));
            }

            var urlBuilder = new System.Text.StringBuilder();
            switch (PEXAEnvironment)
            {
                case PEXAEnvironment.Test:
                    urlBuilder.Append(_testApiUrlBase);
                    break;
                case PEXAEnvironment.Production:
                    urlBuilder.Append(_prodApiUrlBase);
                    break;
                default:
                    throw new InvalidDataException("Unknown PEXAEnvironment.");
            }

            urlBuilder.Append(pexaRequest.Path);

            var client_ = _httpClient;

            using (var httpRequest = new HttpRequestMessage())
            {
                httpRequest.Content = pexaRequest.Content;
                httpRequest.Method = pexaRequest.HttpMethod;
                httpRequest.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml"));
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", pexaRequest.BearerToken);

                // TODO: Determine if this needs to be implemented
                // PrepareRequest(client_, request_, urlBuilder_);

                var url_ = urlBuilder.ToString();
                httpRequest.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                // TODO: Determine if this needs to be implemented
                // PrepareRequest(client_, request_, url_);

                return await client_.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(true);
            }
        }
    }
}