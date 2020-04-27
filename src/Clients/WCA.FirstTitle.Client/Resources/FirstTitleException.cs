
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace WCA.FirstTitle.Client.Resources
{
    public class FirstTitleException : Exception
    {

        public ExceptionResponse ExceptionResponse { get; }
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; private set; }

        public FirstTitleException()
        {

        }
        public FirstTitleException(string message) : base(message)
        {
        }

        public FirstTitleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FirstTitleException(string message, int statusCode, string response, Dictionary<string, IEnumerable<string>> headers, Exception innerException)
            : base(CreateDefaultMessage(message, statusCode, response), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public FirstTitleException(ExceptionResponse exceptionResponse)
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

    [XmlRoot("ExceptionResponse", Namespace = "http://ws.etitle.com.au/schemas")]
    public class ExceptionResponse
    {
        [XmlElement("ExceptionList")]
        public List<ExceptionDetails> ExceptionList { get; } = new List<ExceptionDetails>();
    }

    public class ExceptionDetails
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
