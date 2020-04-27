using MediatR;

namespace WCA.Core.Features.GlobalX.Transactions
{
    public class TransactionDisbursementsCreated : INotification
    {
        public int TransactionId { get; set; }
        public TransactionDisbursementRelationship TransactionDisbursementRelationship { get; set; }

        public TransactionDisbursementsCreated(
            int transactionId,
            TransactionDisbursementRelationship transactionDisbursementRelationship)
        {
            TransactionId = transactionId;
            TransactionDisbursementRelationship = transactionDisbursementRelationship;
        }
    }
}
