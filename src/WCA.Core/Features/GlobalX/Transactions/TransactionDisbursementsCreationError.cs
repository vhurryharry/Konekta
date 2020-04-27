using MediatR;

namespace WCA.Core.Features.GlobalX.Transactions
{
    public class TransactionDisbursementsCreationError : INotification
    {
        public enum TransactionErrorType
        {
            MatterIdNotFoundInActionstep,
            MatterIdBelowMinimum,
            MatterIdUnableToParseAsInt,
            UnknownError,
        }

        public int TransactionId { get; set; }
        public TransactionErrorType ErrorType { get; set; }
        public string ErrorMessage { get; set; }

        public TransactionDisbursementsCreationError(
            int transactionId,
            TransactionErrorType errorType,
            string errorMessage)
        {
            TransactionId = transactionId;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
        }
    }
}
