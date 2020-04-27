using System;

namespace WCA.Domain.Actionstep
{
    public class ConfirmOrder
    {
        public string OrgKey { get; set; }
        public int MatterId { get; set; }
        public string MatterType { get; set; }
        public string TitleOrderId { get; set; }
        public string TitleReference { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
    }
}
