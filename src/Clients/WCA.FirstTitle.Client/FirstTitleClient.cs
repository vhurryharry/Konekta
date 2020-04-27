using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WCA.FirstTitle.Client.Resources;

namespace WCA.FirstTitle.Client
{
    /// <summary>
    /// Designed to be used as a singleton in Dependency Injection.
    /// </summary>
    public class FirstTitleClient : IFirstTitleClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FirstTitleClient> _logger;
        private readonly string _stagingUrl = "https://staging.etitle.com.au/TI/V01-02/RequestForInsurance.asmx";
        private readonly string _productionUrl = "https://ws.etitle.com.au/TI/V01-02/RequestForInsurance.asmx";

        public FirstTitleEnvironment FirstTitleEnvironment { get; set; }

        private Uri ServiceUri { get
            {
                switch (FirstTitleEnvironment)
                {
                    case FirstTitleEnvironment.Staging:
                        return new Uri(_stagingUrl);
                    default:
                        return new Uri(_productionUrl);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="httpClient"></param>
        public FirstTitleClient(HttpClient httpClient, IConfiguration configuration, ILogger<FirstTitleClient> logger)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _httpClient = httpClient;
            _logger = logger;
            FirstTitleEnvironment = (FirstTitleEnvironment)Enum.Parse(typeof(FirstTitleEnvironment), configuration["WCACoreSettings:FirstTitleSettings:Environment"].ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Checks credentials against the service. Useful to verify validity of credentials before storing them for later use.
        /// </summary>
        /// <param name="firstTitleCredential">Ther First Title credential to check against the First Title service.</param>
        /// <returns><c>true</c> if the credentials passed the authentication check, otherwise <c>false</c>.</returns>
        /// <exception cref="HttpRequestException">If the check failed due to an infrastructure issue.</exception>
        public async Task<bool> CheckCredentials(FirstTitleCredential credential)
        {
            if(credential is null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            if (string.IsNullOrEmpty(credential.Username) || string.IsNullOrEmpty(credential.Password))
            {
                return false;
            }

            // Some sanity checks. 10,000 is arbitrary. High enough to hopefully never be encountered for
            // a legitimate username/password and low enough to reduce side effects of attacks.
            if (credential.Username.Length > 10000 || credential.Password.Length > 10000)
                return false;

            HttpResponseMessage response;

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, ServiceUri))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetBasicAuthValue(credential));
                response = await _httpClient.SendAsync(requestMessage);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            // This will throw an exception with the relevant details so that
            // the caller knows that the check wasn't able to be completed.
            response.EnsureSuccessStatusCode();

            // If it didn't throw, then we know we had a successful return code
            // so the credentials must be valid.
            return true;
        }

        public async Task<TResponse> Handle<TResponse>(FirstTitleRequestBase fTRequest, CancellationToken cancellationToken)
            where TResponse : class
        {
            if (fTRequest is null)
            {
                throw new ArgumentNullException(nameof(fTRequest));
            }

            var isValidCredentials = await CheckCredentials(fTRequest.FirstTitleCredential);
            if (!isValidCredentials)
            {
                throw new InvalidFirstTitleCredentialsException(fTRequest.FirstTitleCredential);
            }

            var response = await SendApiRequest(fTRequest, cancellationToken);

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

                    var rawStringContent = await response.Content.ReadAsStringAsync();
                    var xdoc = XDocument.Parse(rawStringContent);
                    XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
                    XNamespace m = "http://ws.etitle.com.au/schemas";

                    var responseXml = xdoc.Element(soap + "Envelope").Element(soap + "Body")
                                            .Element(m + "TitleInsuranceResponse");
                    var message = responseXml.Elements().First();

                    message.Name = message.Name.LocalName;

                    var content = message.ToString();
                    content = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + content;

                    using (var reader = new StringReader(content))
                    using (var xmlReader = new XmlTextReader(reader))
                    {
                        var serializer = new XmlSerializer(typeof(TResponse));
                        var rawResponse = serializer.Deserialize(xmlReader);

                        return (TResponse)rawResponse;
                    }
                }
                else if (status_ == "400")
                {
                    if (response.Content == null) throw new FirstTitleException();

                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var xmlReader = XmlReader.Create(contentStream))
                    {
                        ExceptionResponse exceptionResponse;

                        var exceptionResponseSerializerv2 = new XmlSerializer(typeof(ExceptionResponse));
                        exceptionResponse = (ExceptionResponse)exceptionResponseSerializerv2.Deserialize(xmlReader);

                        if (xmlReader != null)
                        {
                            xmlReader.Dispose();
                        }

                        throw new FirstTitleException(exceptionResponse);
                    }
                }
                else if (status_ == "401")
                {
                    var responseData_ = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new FirstTitleException("Unauthenticated", (int)response.StatusCode, responseData_, headers, null);
                }
                else if (status_ == "403")
                {
                    var responseData_ = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new FirstTitleException("Forbidden", (int)response.StatusCode, responseData_, headers, null);
                }
                else if (status_ == "500")
                {
                    var responseData_ = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new FirstTitleException("Internal Server Error", (int)response.StatusCode, responseData_, headers, null);
                }
                else if (status_ != "200" && status_ != "204")
                {
                    var responseData_ = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new FirstTitleException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", (int)response.StatusCode, responseData_, headers, null);
                }

                return null;
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
        }

        private static string GetBasicAuthValue(FirstTitleCredential firstTitleCredential)
        {
            var credentialsByteArray = Encoding.ASCII.GetBytes($"{firstTitleCredential.Username}:{firstTitleCredential.Password}");
            return Convert.ToBase64String(credentialsByteArray);
        }

        private static XmlDocument CreateSoapEnvelope(string content)
        {
            XmlDocument soapEnvelope = new XmlDocument();
            string xmlContent = string.Format(CultureInfo.InvariantCulture, @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                    <soap:Body>
                        {0}
                    </soap:Body>
                </soap:Envelope>", content);
            soapEnvelope.LoadXml(xmlContent);
            return soapEnvelope;
        }

        private async Task<HttpResponseMessage> SendApiRequest(FirstTitleRequestBase fTRequest, CancellationToken cancellationToken)
        {
            if (fTRequest is null)
            {
                throw new ArgumentNullException(nameof(fTRequest));
            }

            if (fTRequest.FirstTitleCredential is null)
            {
                throw new ArgumentException(nameof(fTRequest.FirstTitleCredential));
            }

            var client_ = _httpClient;

            using (var httpRequest = new HttpRequestMessage())
            {
                if (string.IsNullOrEmpty(fTRequest.Content))
                {
                    httpRequest.Content = null;
                }
                else
                {
                    var soapRequest = CreateSoapEnvelope(fTRequest.Content);

                    httpRequest.Content = new StringContent(soapRequest.OuterXml, Encoding.UTF8);
                    httpRequest.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("text/xml");
                }

                httpRequest.Method = fTRequest.HttpMethod;
                httpRequest.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/soap+xml"));
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetBasicAuthValue(fTRequest.FirstTitleCredential));
                httpRequest.Headers.Add("SOAPAction", fTRequest.SOAPAction);

                httpRequest.RequestUri = ServiceUri;

                return await client_.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(true);
            }
        }
    }
}
