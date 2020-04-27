using NodaTime;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace WCA.Core.Services.DurableFunctions
{
    [SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Not Applicable, as this refers to Durable Functions events.")]
    public interface IDurableFunctionsService
    {
        Task<OrchestratorInstance<TInput, TOutput, TCustomStatus>> GetInstance<TInput, TOutput, TCustomStatus>(string id);
        Task<OrchestratorInstance> GetInstance(string id);
        Task<IEnumerable<OrchestratorInstance>> GetInstances();
        Task<IEnumerable<OrchestratorInstance>> GetInstances(int? top, params RuntimeStatus[] runtimeStatuses);
        Task<IEnumerable<OrchestratorInstance>> GetInstances(params RuntimeStatus[] runtimeStatuses);
        Task<IEnumerable<OrchestratorInstance>> GetInstances(Instant? createdTimeFrom, Instant? createdTimeTo, int? top, params RuntimeStatus[] runtimeStatuses);
        Task RaiseDurableEvent(string instanceId, string EventName);
        Task RaiseDurableEvent(string instanceId, string EventName, object payload);
        Task Terminate(string instanceId, string reason = null);
        Task Rewind(string instanceId, string reason = null);
    }
}