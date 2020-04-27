using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace WCA.AzureFunctions.EmailToSMS.BurstSms
{
    [Serializable]
    public class InvalidSMSClientException : Exception
    {
        public InvalidSMSClientException()
        {
        }

        public InvalidSMSClientException(int clientId)
            : base($"Client ID: {clientId.ToString(CultureInfo.InvariantCulture)}")
        {
        }

        public InvalidSMSClientException(string message) : base(message)
        {
        }

        public InvalidSMSClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSMSClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}