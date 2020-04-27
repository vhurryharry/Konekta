using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace WCA.Core.Services.DurableFunctions
{
    public class DurableFunctionsService : IDurableFunctionsService
    {
        private readonly string _functionsBaseUrl;
        private readonly string _functionsHubName;
        private readonly string _functionsHostKey;

        public DurableFunctionsService(IOptions<WCACoreSettings> options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            _functionsBaseUrl = options.Value.AzureFunctionsUrl;
            _functionsHubName = options.Value.AzureFunctionsHubName;
            _functionsHostKey = options.Value.AzureFunctionsHostKey;
        }

        public Task<IEnumerable<OrchestratorInstance>> GetInstances() => GetInstances(null, null, null, null);
        public Task<IEnumerable<OrchestratorInstance>> GetInstances(params RuntimeStatus[] runtimeStatuses) => GetInstances(null, null, null, runtimeStatuses);
        public Task<IEnumerable<OrchestratorInstance>> GetInstances(int? top, params RuntimeStatus[] runtimeStatuses) => GetInstances(null, null, top, runtimeStatuses);

        public async Task<IEnumerable<OrchestratorInstance>> GetInstances(
            Instant? createdTimeFrom,
            Instant? createdTimeTo,
            int? top,
            RuntimeStatus[] runtimeStatuses)
        {
            return await _functionsBaseUrl
                .AppendPathSegment("/runtime/webhooks/durableTask/instances")
                .SetQueryParam("createdTimeFrom", createdTimeFrom?.ToString(InstantPattern.General.PatternText, CultureInfo.InvariantCulture))
                .SetQueryParam("createdTimeTo", createdTimeTo?.ToString(InstantPattern.General.PatternText, CultureInfo.InvariantCulture))
                .SetQueryParam("runtimeStatus", runtimeStatuses is null ? null : string.Join(',', runtimeStatuses))
                .SetQueryParam("top", top)
                .SetQueryParam("taskHub", _functionsHubName)
                .WithHeader("x-functions-key", _functionsHostKey)
                .WithHeader("Accept", "application/json")
                .GetJsonAsync<IEnumerable<OrchestratorInstance>>();
        }

        public Task<OrchestratorInstance> GetInstance(string id) => GetInstance<OrchestratorInstance>(id);

        public Task<OrchestratorInstance<TInput, TOutput, TCustomStatus>> GetInstance<TInput, TOutput, TCustomStatus>(string id)
            => GetInstance<OrchestratorInstance<TInput, TOutput, TCustomStatus>>(id);

        private async Task<T> GetInstance<T>(string id)
        {
            return await _functionsBaseUrl
                .AppendPathSegments("/runtime/webhooks/durabletask/instances", id)
                .SetQueryParam("showHistory", true)
                .SetQueryParam("taskHub", _functionsHubName)
                .WithHeader("x-functions-key", _functionsHostKey)
                .WithHeader("Accept", "application/json")
                .GetJsonAsync<T>();
        }

        public Task RaiseDurableEvent(string instanceId, string EventName) => RaiseDurableEvent(instanceId, EventName, null);
        public async Task RaiseDurableEvent(string instanceId, string eventName, object payload)
        {
            await _functionsBaseUrl
                .AppendPathSegments("/runtime/webhooks/durabletask/instances", instanceId, "raiseEvent", eventName)
                .SetQueryParam("taskHub", _functionsHubName)
                .WithHeader("x-functions-key", _functionsHostKey)
                .WithHeader("Accept", "application/json")
                .WithHeader("Content-Type", "application/json")
                .PostJsonAsync(payload);
        }

        public async Task Terminate(string instanceId, string reason = null)
        {
            await _functionsBaseUrl
                .AppendPathSegments("/runtime/webhooks/durabletask/instances", instanceId, "terminate")
                .SetQueryParam("taskHub", _functionsHubName)
                .SetQueryParam("reason", reason)
                .WithHeader("x-functions-key", _functionsHostKey)
                .WithHeader("Accept", "application/json")
                .SendAsync(HttpMethod.Post);
        }

        public async Task Rewind(string instanceId, string reason = null)
        {
            await _functionsBaseUrl
                .AppendPathSegments("/runtime/webhooks/durabletask/instances", instanceId, "rewind")
                .SetQueryParam("taskHub", _functionsHubName)
                .SetQueryParam("reason", reason)
                .WithHeader("x-functions-key", _functionsHostKey)
                .WithHeader("Accept", "application/json")
                .SendAsync(HttpMethod.Post);
        }
    }
}