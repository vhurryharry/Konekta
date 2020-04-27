using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace WCA.AzureFunctions
{
    [Serializable]
    public class InstanceIdMismatchException : Exception
    {
        public string OrchestratorName { get; private set; }
        public string InstanceId { get; private set; }
        public string ExpectedInstanceId { get; private set; }

        public InstanceIdMismatchException()
        {
        }

        public InstanceIdMismatchException(string message) : base(message)
        {
        }

        public InstanceIdMismatchException(string orchestratorName, string instanceId, string expectedInstanceId)
            : base(GetDefaultMessage(orchestratorName, instanceId, expectedInstanceId))
        {
            OrchestratorName = orchestratorName;
            InstanceId = instanceId;
            ExpectedInstanceId = expectedInstanceId;
        }

        public InstanceIdMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected InstanceIdMismatchException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            InstanceId = info.GetString(nameof(InstanceId));
            ExpectedInstanceId = info.GetString(nameof(ExpectedInstanceId));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(InstanceId), InstanceId);
            info.AddValue(nameof(ExpectedInstanceId), ExpectedInstanceId);
        }

        private static string GetDefaultMessage(string orchestratorName, string instanceId, string expectedInstanceId) =>
            $"Orchestrator '{orchestratorName}' has instance ID '{instanceId}', but we expected it to have the ID '{expectedInstanceId}'.";
    }
}