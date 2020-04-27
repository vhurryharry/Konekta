using Newtonsoft.Json;
using NodaTime;
using System.Diagnostics.CodeAnalysis;

namespace WCA.Core.Services.DurableFunctions
{
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO")]
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "DTO")]
    public class OrchestratorInstance<TInput, TOutput, TCustomStatus>
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("runtimeStatus")]
        public RuntimeStatus RuntimeStatus { get; set; }

        [JsonProperty("input")]
        public TInput Input { get; set; }

        [JsonProperty("customStatus")]
        public TCustomStatus CustomStatus { get; set; }

        [JsonProperty("output")]
        public TOutput Output { get; set; }

        [JsonProperty("createdTime")]
        public Instant CreatedTime { get; set; }

        [JsonProperty("lastUpdatedTime")]
        public Instant LastUpdatedTime { get; set; }

        [JsonProperty("historyEvents")]
        public HistoryEvent[] HistoryEvents { get; set; }
    }

    public class OrchestratorInstance : OrchestratorInstance<dynamic, dynamic, dynamic>
    { }

    public class HistoryEvent
    {
        public string EventType { get; set; }
        public Instant Timestamp { get; set; }
        public string FunctionName { get; set; }
        public Instant? ScheduledTime { get; set; }
        public string Reason { get; set; }
        public string Details { get; set; }
        public Instant? FireAt { get; set; }
    }

}