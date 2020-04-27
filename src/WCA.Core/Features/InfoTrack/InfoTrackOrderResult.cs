using System;
using System.Collections.Generic;

namespace WCA.Core.Features.InfoTrack
{
    public class InfoTrackOrderResult
    {
        public string ActionstepOrgTitle { get; set; }
        public string ActionstepOrgKey { get; set; }
        public int ActionstepMatterId { get; set; }
        public string OrderedByWCAUserId { get; set; }
        public string OrderedByWCAUserName { get; set; }
        public string OrderedByWCAUserEmail { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public string UpdatedByEmail { get; set; }
        public string ActionstepDisbursementStatus { get; set; }
        public DateTime ActionstepDisbursementStatusUpdatedUtc { get; set; }
        public string ActionstepDocumentUploadStatus { get; set; }
        public DateTime ActionstepDocumentUploadStatusUpdatedUtc { get; set; }

        public bool InfoTrackAvailableOnline { get; set; }
        public string InfoTrackBillingTypeName { get; set; }
        public string InfoTrackClientReference { get; set; }
        public DateTime InfoTrackDateOrderedUtc { get; set; }
        public DateTime? InfoTrackDateCompletedUtc { get; set; }
        public string InfoTrackDescription { get; set; }
        public int InfoTrackOrderId { get; set; }
        public int InfoTrackParentOrderId { get; set; }
        public string InfoTrackOrderedBy { get; set; }
        public string InfoTrackReference { get; set; }
        public string InfoTrackRetailerReference { get; set; }
        public decimal InfoTrackRetailerFee { get; set; }
        public decimal InfoTrackRetailerFeeGST { get; set; }
        public decimal InfoTrackRetailerFeeTotal { get; set; }
        public decimal InfoTrackSupplierFee { get; set; }
        public decimal InfoTrackSupplierFeeGST { get; set; }
        public decimal InfoTrackSupplierFeeTotal { get; set; }
        public decimal InfoTrackTotalFee { get; set; }
        public decimal InfoTrackTotalFeeGST { get; set; }
        public decimal InfoTrackTotalFeeTotal { get; set; }
        public string InfoTrackServiceName { get; set; }
        public string InfoTrackStatus { get; set; }
        public string InfoTrackStatusMessage { get; set; }
        public string InfoTrackDownloadUrl { get; set; }
        public string InfoTrackOnlineUrl { get; set; }
        public bool InfoTrackIsBillable { get; set; }
        public string InfoTrackFileHash { get; set; }
        public string InfoTrackEmail { get; set; }
        public bool Reconciled { get; set; }
    }

    public class OrderHistoryResult
    {
        public int MatterId { get; set; }
        public string OrgKey { get; set; }
        public string Name { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalFeeGst { get; set; }
        public decimal TotalFeeTotal { get; set; }
        public List<InfoTrackOrderResult> Orders { get; set; }
    }

}
