using FluentValidation;
using System;
using WCA.Domain.Abstractions;
using WCA.Domain.Actionstep;
using WCA.Domain.Models.Account;

namespace WCA.Domain.GlobalX
{
    /// <summary>
    /// Stores Org level configuration information for GlobalX 
    /// </summary>
    public class GlobalXOrgSettings : EntityBase
    {
        public string ActionstepOrgKey { get; set; }
        public ActionstepOrg ActionstepOrg { get; set; }

        /// <summary>
        /// The user who is a GlobalX Administrator and root user for this org.
        /// 
        /// This account will be used to retrieve transactions and documents from the GlobalX API.
        /// </summary>
        public WCAUser GlobalXAdmin { get; set; }
        public string GlobalXAdminId { get; set; }

        /// <summary>
        /// The user who is a GlobalX Administrator and root user for this org.
        /// </summary>
        public WCAUser ActionstepSyncUser { get; set; }
        public string ActionstepSyncUserId { get; set; }

        /// <summary>
        /// Whether transaction (disbursement) sync is enabled for this org. Defaults to true.
        /// </summary>
        public bool TransactionSyncEnabled { get; set; } = true;

        /// <summary>
        /// The most recently processed transaction id. To enable incremental transaction processing.
        /// </summary>
        public int LatestTransactionId { get; set; }

        /// <summary>
        /// The Actionstep TaxCode to use when creating disbursements with GST.
        /// </summary>
        public int? TaxCodeIdWithGST { get; set; }

        /// <summary>
        /// The Actionstep TaxCode to use when creating disbursements with no GST.
        /// </summary>
        public int? TaxCodeIdNoGST { get; set; }

        /// <summary>
        /// Whether document sync is enabled for this org. Defaults to true.
        /// </summary>
        public bool DocumentSyncEnabled { get; set; } = true;

        public DateTime LastDocumentSyncUtc { get; set; }

        /// <summary>
        /// When performing document or transaction sync, if the matter ID is equal to
        /// or less than this number, the sync will be skipped.
        /// 
        /// Use this in migration scenarios when it is known that only matters above a
        /// certain number will be in Actionstep (e.g. 100,000), and therefore all
        /// matters below this number are in the old system and should not be synced.
        /// </summary>
        public int MinimumMatterIdToSync { get; set; } = 0;

        public class Validator : AbstractValidator<GlobalXOrgSettings>
        {
            public Validator()
            {
                RuleFor(s => s.ActionstepOrgKey).NotEmpty();
                RuleFor(s => s.GlobalXAdminId).NotEmpty();
                RuleFor(s => s.ActionstepSyncUserId).NotEmpty();
                RuleFor(s => s.TaxCodeIdWithGST).NotNull().GreaterThan(0).When(s => s.TransactionSyncEnabled);
                RuleFor(s => s.TaxCodeIdNoGST).NotNull().GreaterThan(0).When(s => s.TransactionSyncEnabled);
            }
        }
    }
}
