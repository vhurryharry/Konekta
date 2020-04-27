using MediatR;
using WCA.GlobalX.Client.Transactions;

namespace WCA.Core.Features.GlobalX.Transactions
{
    public class TransactionAdded : INotification
    {
        public string ActionstepOrgKey { get; set; }
        public Transaction Transaction { get; set; }

        public TransactionAdded(string actionstepOrgKey, Transaction transaction)
        {
            ActionstepOrgKey = actionstepOrgKey;
            Transaction = transaction;
        }
    }
}
