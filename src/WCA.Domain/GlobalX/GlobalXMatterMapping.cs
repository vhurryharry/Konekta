using WCA.Domain.Abstractions;
using WCA.Domain.Actionstep;

namespace WCA.Domain.GlobalX
{
    /// <summary>
    /// Contains a mapping between a GlobalX Matter ID and an Actionstep Matter ID.
    /// Used when a matter is incorrectly entered in to the GlobalX system.
    /// </summary>
    public class GlobalXMatterMapping : EntityBase
    {
        public ActionstepOrg ActionstepOrg { get; set; }
        public string ActionstepOrgKey { get; set; }
        public string GlobalXMatterId { get; set; }
        public int ActionstepMatterId { get; set; }
    }
}
