using System;
using WCA.Domain.Abstractions;
using WCA.Domain.Actionstep;
using WCA.Domain.Models.Account;

namespace WCA.Domain.Integrations
{
    public class IntegrationSetting : EntityBase, IEntityWithGuid
    {
        public Guid Id { get; set; }

        public ActionstepOrg ActionstepOrg { get; set; }
        public string ActionstepOrgKey { get; set; }

        public WCAUser User { get; set; }
        public string UserId { get; set; }

        public int SortOrder { get; set; }

        public bool HideIntegration { get; set; } = false;

        public Integration Integration { get; set; }
        public Guid IntegrationId { get; set; }
    }
}
