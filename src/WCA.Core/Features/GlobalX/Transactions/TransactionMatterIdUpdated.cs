using MediatR;

namespace WCA.Core.Features.GlobalX.Transactions
{
    public class TransactionMatterIdUpdated : INotification
    {
        public int TransactionId { get; set; }

        public string ActionstepOrgKey { get; set; }
        public string OldMatterId { get; set; }
        public int NewMatterId { get; set; }

        public TransactionMatterIdUpdated(int transactionId, string actionstepOrgKey, string oldMatterId, int newMatterId)
        {
            TransactionId = transactionId;
            ActionstepOrgKey = actionstepOrgKey;
            OldMatterId = oldMatterId;
            NewMatterId = newMatterId;
        }
    }
}
