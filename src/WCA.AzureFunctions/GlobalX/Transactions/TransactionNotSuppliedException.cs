using System;
using System.Runtime.Serialization;

namespace WCA.AzureFunctions.GlobalX.Transactions
{
    [Serializable]
    public class TransactionNotSuppliedException : Exception
    {
        public TransactionNotSuppliedException()
        {
        }

        public TransactionNotSuppliedException(string message) : base(message)
        {
        }

        public TransactionNotSuppliedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TransactionNotSuppliedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}