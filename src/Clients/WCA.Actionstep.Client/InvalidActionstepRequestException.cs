using System;
using System.Runtime.Serialization;

namespace WCA.Actionstep.Client
{
    [Serializable]
    public class InvalidActionstepRequestException : Exception
    {
        public ActionstepHttpRequestMessage ActionstepRequest { get; set; }

        public InvalidActionstepRequestException()
        {
        }

        public InvalidActionstepRequestException(string message) : base(message)
        {
        }

        public InvalidActionstepRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidActionstepRequestException(string message, ActionstepHttpRequestMessage actionstepHttpRequestMessage) : this(EnrichMessage(message, actionstepHttpRequestMessage))
        {
            ActionstepRequest = actionstepHttpRequestMessage;
        }

        private static string EnrichMessage(string message, ActionstepHttpRequestMessage actionstepHttpRequestMessage) =>
            $"{message}" +
                $" (" +
                $"TokenSet Id: '{actionstepHttpRequestMessage?.TokenSet?.Id}'" +
                $"TokenSet UserId: '{actionstepHttpRequestMessage?.TokenSet?.UserId}'" +
                $"TokenSet OrgKey: '{actionstepHttpRequestMessage?.TokenSet?.OrgKey}'" +
                $", " +
                $"Original Uri: {actionstepHttpRequestMessage?.RequestUri}" +
                $")";

        protected InvalidActionstepRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}