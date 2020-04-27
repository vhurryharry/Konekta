using System;
using WCA.Domain.Models.Settlement;

namespace WCA.Web.Areas.API.SettlementCalculator
{
    public class SettlementMatterViewModel
    {
        public string ActionstepOrg { get; set; }
        public int MatterId { get; set; }
        public int Version { get; set; }
        public SettlementInfo SettlementData { get; set; }
        public ActionstepMatter ActionstepData { get; set; }

        public SettlementMatterViewModel(string actionstepOrg,
            int matterId,
            int version,
            SettlementInfo settlementData,
            ActionstepMatter actionstepData = null)
        {
            ActionstepOrg = actionstepOrg;
            MatterId = matterId;
            Version = version;
            SettlementData = settlementData;
            ActionstepData = actionstepData;
        }

        public SettlementMatter ToDomainModel()
        {
            return new SettlementMatter(ActionstepOrg, MatterId, SettlementData);
        }

        public static SettlementMatterViewModel FromDomainModel(SettlementMatter settlementMatter)
        {
            if (settlementMatter is null)
            {
                throw new ArgumentNullException(nameof(settlementMatter));
            }

            return new SettlementMatterViewModel(
                settlementMatter.ActionstepOrgKey,
                settlementMatter.ActionstepMatterId,
                settlementMatter.Version,
                settlementMatter.SettlementData,
                settlementMatter.ActionstepData
                );
        }
    }
}
