namespace WCA.Core.Features.Actionstep.Responses
{
    public class ActionstepMatterInfo
    {
        public string OrgKey { get; }
        public string OrgName { get; }
        public string MatterName { get; }
        public int MatterId { get; }
        

        public ActionstepMatterInfo(
            string orgKey,
            string matterName,
            int matterId,
            string orgName)
        {
            OrgKey = orgKey;
            OrgName = orgName;
            MatterName = matterName;
            MatterId = matterId;
        }
    }
}
