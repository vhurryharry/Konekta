using System.Collections.Generic;
using WCA.Web.FeatureFlags;

namespace WCA.Web.Areas.API.Actionstep
{
    public class ActionstepMatterInfo
    {
        public string OrgKey { get; set; }
        public string OrgName { get; set; }
        public string MatterName { get; set; }
        public int MatterId { get; set; }
        public IEnumerable<FeatureFlag> FeatureFlags { get; set; }
    }
}