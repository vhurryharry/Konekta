using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace WCA.Actionstep.Client
{
    [Serializable]
    public class BadActionstepApiResponseException : Exception
    {
        public string RawResponse { get; private set; }

        public BadActionstepApiResponseException()
        {
        }

        public BadActionstepApiResponseException(string message) : base(message)
        {
        }

        public BadActionstepApiResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BadActionstepApiResponseException(Exception ex, string rawResponse)
            : this($"The response from the Actionstep API indicated an error. See the Inner Exception or '{nameof(BadActionstepApiResponseException.RawResponse)}' object for details.", ex)
        {
            RawResponse = rawResponse;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected BadActionstepApiResponseException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            RawResponse = info.GetString(nameof(RawResponse));
        }


        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(RawResponse), RawResponse);
        }
    }
}