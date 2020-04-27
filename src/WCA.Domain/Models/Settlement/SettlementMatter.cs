using System.ComponentModel.DataAnnotations.Schema;
using WCA.Domain.Abstractions;

namespace WCA.Domain.Models.Settlement
{
    /// <summary>
    /// Settlement Matter Data with version information
    /// </summary>
    public class SettlementMatter : EntityBase, IEntityWithId
    {
        public int Id { get; set; }
        public string ActionstepOrgKey { get; set; }
        public int ActionstepMatterId { get; set; }
        public int Version { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public SettlementInfo SettlementData { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public ActionstepMatter ActionstepData { get; set; }

        public SettlementMatter(
            string actionstepOrgKey,
            int actionstepMatterId,
            SettlementInfo settlementData,
            ActionstepMatter actionstepData = null)
        {
            ActionstepOrgKey = actionstepOrgKey;
            ActionstepMatterId = actionstepMatterId;
            Version = 1;
            SettlementData = settlementData;
            ActionstepData = actionstepData;
        }

        public SettlementMatter(
            string actionstepOrgKey = "",
            int actionstepMatterId = 0
            )
        {
            ActionstepOrgKey = actionstepOrgKey;
            ActionstepMatterId = actionstepMatterId;
            Version = 0;
            SettlementData = new SettlementInfo();
            ActionstepData = new ActionstepMatter();
        }
    }
}
