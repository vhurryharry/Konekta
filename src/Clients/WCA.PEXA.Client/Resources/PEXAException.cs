using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace WCA.PEXA.Client.Resources
{
    public class PEXAException : Exception
    {
        public ExceptionResponse ExceptionResponse { get; }
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; private set; }

        public PEXAException()
        {

        }
        public PEXAException(string message) : base(message)
        {
        }

        public PEXAException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PEXAException(string message, int statusCode, string response, Dictionary<string, IEnumerable<string>> headers, Exception innerException)
            : base(CreateDefaultMessage(message, statusCode, response), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public PEXAException(ExceptionResponse exceptionResponse)
        {
            StatusCode = 400;
            ExceptionResponse = exceptionResponse;
        }

        private static string CreateDefaultMessage(string message, int statusCode, string response)
        {
            return $"{message}" + Environment.NewLine +
                $"Status: {statusCode.ToString(CultureInfo.InvariantCulture)}" + Environment.NewLine +
                $"Response: {response.Substring(0, response.Length >= 512 ? 512 : response.Length)}";
        }
    }

    public class ExceptionResponse
    {
        [XmlElement("ExceptionList")]
        public List<ExceptionDetails> ExceptionList { get; } = new List<ExceptionDetails>();
    }

    [XmlRoot("ExceptionResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
    public class ExceptionResponsev2: ExceptionResponse { }

    [XmlRoot("ExceptionResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public class ExceptionResponsev1: ExceptionResponse { }

    public class ExceptionDetails
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
