using System;
using WCA.Domain.Abstractions;
using WCA.Domain.Actionstep;

namespace WCA.Domain.GlobalX
{
    public enum TransactionProcessingStatus
    {
        NotYetProcessed,
        ProcessedSuccessfully,

        MatterIdNotFoundInActionstep,
        MatterIdBelowMinimum,
        MatterIdUnableToParseAsInt,

        UnknownError,
    }

    public class GlobalXTransactionState : EntityBase
    {
        public int TransactionId { get; set; }
        public string OrderId { get; set; }

        public ActionstepOrg ActionstepOrg { get; set; }
        public string ActionstepOrgKey { get; set; }

        /// <summary>
        /// Stored as a string because it is a string in GlobalX. This allows us to store state
        /// in the event an invalid matter comes back from GlobalX.
        /// </summary>
        public string MatterId { get; set; }
        
        public TransactionProcessingStatus ProcessingStatus { get; private set; } = TransactionProcessingStatus.NotYetProcessed;
        public DateTime ProcessingStatusUpdatedUtc { get; private set; }
        public string LastError { get; private set; }

        public int? GSTTaxableDisbursementId { get; set; }
        public int? GSTFreeDisbursementId { get; set; }

        public DateTime TransactionDateTimeUtc { get; set; }
        public string SearchReference { get; set; }
        public decimal WholesalePrice { get; set; }
        public decimal WholesaleGst { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal RetailGst { get; set; }
        public int CreditForTransactionId { get; set; }
        public int ItemNumber { get; set; }
        public bool MatterBasedInvoiced { get; set; }
        public string GlobalXUserId { get; set; }
        public string GlobalXCustomerRef { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string ProductSubGroup { get; set; }

        public void UpdateStatus(TransactionProcessingStatus transactionProcessingStatus, string lastErrorMessage = null)
        {
            ProcessingStatus = transactionProcessingStatus;
            ProcessingStatusUpdatedUtc = DateTime.UtcNow;
            LastError = lastErrorMessage;
        }
    }
}
