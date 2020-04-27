using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WCA.AzureFunctions.EmailToSMS.BurstSms
{
    public class BurstSMSService : ISMSService
    {
        private string _resellerApiKey;
        private string _resellerApiSecret;

        // Internal cache
        private Dictionary<int, ApiCredentialPair> _clientCredentials = new Dictionary<int, ApiCredentialPair>();

        public BurstSMSService(string resellerApiKey, string resellerApiSecret)
        {
            if (string.IsNullOrEmpty(resellerApiKey)) throw new ArgumentException("Parameter be supplied", nameof(resellerApiKey));
            if (string.IsNullOrEmpty(resellerApiSecret)) throw new ArgumentException("Parameter be supplied", nameof(resellerApiSecret));

            _resellerApiKey = resellerApiKey;
            _resellerApiSecret = resellerApiSecret;
        }

        public async Task<SendSmsResponse> SendSms(int clientId, string recipientMobileNumber, string message, string sendAt, string repliesToEmail)
        {
            var clientCredential = await FindCredentialForClient(clientId);

            return await "https://api.transmitsms.com/send-sms.json"
                .SetQueryParams(new
                {
                    message,
                    to = recipientMobileNumber,
                    send_at = sendAt,
                    replies_to_email = repliesToEmail,
                })
                .WithBasicAuth(clientCredential.ApiKey, clientCredential.ApiSecret)
                .AllowHttpStatus("400")
                .GetJsonAsync<SendSmsResponse>();
        }

        private async Task<ApiCredentialPair> FindCredentialForClient(int clientId)
        {
            if (_clientCredentials.ContainsKey(clientId))
            {
                return _clientCredentials[clientId];
            }
            else
            {
                // No client credential in cache, so use reseller credential to get it
                var clientCredentialResponse = await "https://api.transmitsms.com/get-client.json"
                    .SetQueryParam("client_id", clientId)
                    .WithBasicAuth(_resellerApiKey, _resellerApiSecret)
                    .GetJsonAsync<ClientResponse>();

                var clientCredential = new ApiCredentialPair(clientCredentialResponse.ApiKey, clientCredentialResponse.ApiSecret);

                if (!clientCredential.ContainsValues())
                {
                    throw new InvalidSMSClientException(clientId);
                }

                _clientCredentials.Add(clientId, clientCredential);
                return clientCredential;
            }
        }

#pragma warning disable CA1812
        private class ClientResponse
#pragma warning restore CA1812
        {
            [JsonProperty("apikey")]
            public string ApiKey { get; set; }

            [JsonProperty("apisecret")]
            public string ApiSecret{ get; set; }
        }
    }
}
