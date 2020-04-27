using System;
using WCA.Domain.Abstractions;
using WCA.Domain.Actionstep;
using WCA.Domain.Models.Account;

namespace WCA.Domain.Integrations
{
    public class IntegrationLinkSetting : EntityBase, IEntityWithGuid
    {
        public Guid Id { get; set; }

        public ActionstepOrg ActionstepOrg { get; set; }
        public string ActionstepOrgKey { get; set; }

        public WCAUser User { get; set; }
        public string UserId { get; set; }

        public int SortOrder { get; set; }

        public bool HideIntegrationLink { get; set; } = false;

        public IntegrationLink IntegrationLink { get; set; }
        public Guid IntegrationLinkId { get; set; }
    }
}
