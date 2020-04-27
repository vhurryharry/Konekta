namespace WCA.Core.Features.GlobalX.Transactions
{
    public class TransactionDisbursementRelationship
    {
        public int GlobalXTransactionId { get; }
        public string ActionstepOrgKey { get; }
        public int ActionstepMatterId { get; }
        public int? GSTTaxableDisbursementId { get; }
        public int? GSTFreeDisbursementId { get; }

        public TransactionDisbursementRelationship(
            int globalXTransactionId,
            string actionstepOrgKey,
            int actionstepMatterId,
            int? gstTaxableDisbursementId,
            int? gstFreeDisbursementId)
        {
            GlobalXTransactionId = globalXTransactionId;
            ActionstepOrgKey = actionstepOrgKey;
            ActionstepMatterId = actionstepMatterId;
            GSTTaxableDisbursementId = gstTaxableDisbursementId;
            GSTFreeDisbursementId = gstFreeDisbursementId;
        }
    }
}
