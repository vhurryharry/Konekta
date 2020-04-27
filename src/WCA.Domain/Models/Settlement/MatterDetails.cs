using System;

namespace WCA.Domain.Models.Settlement
{
    public class MatterDetails
    {
        public int MatterRef { get; set; }
        public string Matter { get; set; }
        public string Property { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public DateTime SettlementDate { get; set; }
        public string SettlementPlace { get; set; }
        public string SettlementTime { get; set; }
        public string State { get; set; }
        public string ConveyType { get; set; }

        public MatterDetails()
        {
            MatterRef = 0;
            Matter = "";
            Property = "";
            AdjustmentDate = DateTime.Today;
            SettlementDate = DateTime.Today;
            SettlementPlace = "";
            SettlementTime = "";
            State = "";
            ConveyType = "";
        }

        public MatterDetails(MatterDetails matterDetails)
        {
            if (matterDetails != null)
            {
                MatterRef = matterDetails.MatterRef;
                Matter = matterDetails.Matter;
                Property = matterDetails.Property;
                AdjustmentDate = matterDetails.AdjustmentDate;
                SettlementDate = matterDetails.SettlementDate;
                SettlementPlace = matterDetails.SettlementPlace;
                SettlementTime = matterDetails.SettlementTime;
                State = matterDetails.State;
                ConveyType = matterDetails.ConveyType;
            }
        }

        public MatterDetails(
            int matterRef,
            string matter,
            string property,
            DateTime adjustmentDate,
            DateTime settlementDate,
            string settlementPlace,
            string settlementTime,
            string state,
            string conveyType)
        {
            MatterRef = matterRef;
            Matter = matter;
            Property = property;
            AdjustmentDate = adjustmentDate;
            SettlementDate = settlementDate;
            SettlementPlace = settlementPlace;
            SettlementTime = settlementTime;
            State = state;
            ConveyType = conveyType;
        }
    }
}
