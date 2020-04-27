namespace WCA.Core.Features.Actionstep.Connection
{
    public class IntegrationsTabStatusForOrgAndActionType
    {
        public int ActionTypeId { get; set; }
        public string ActionTypeName { get; set; }
        public string OrgKey { get; set; }
        public bool? NeedsUpdate { get; set; }
        public bool Authorized { get; set; }
    }
}
