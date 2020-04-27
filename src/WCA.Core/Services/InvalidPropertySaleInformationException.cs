using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WCA.Core.Services
{
    public class InvalidPropertySaleInformationException : Exception
    {
        public InvalidPropertySaleInformationException()
        {
        }

        public InvalidPropertySaleInformationException(string message) : base(message)
        {
        }

        public InvalidPropertySaleInformationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
