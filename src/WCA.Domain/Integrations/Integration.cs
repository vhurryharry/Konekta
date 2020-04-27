using System;
using System.Collections.Generic;
using WCA.Domain.Abstractions;

namespace WCA.Domain.Integrations
{
    /// <summary>
    /// Uses Guid identifier to allow for seeding and consistent base set of integrations.
    /// </summary>
    public class Integration : EntityBase, IEntityWithGuid
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string LogoSrc { get; set; }
        public string LogoAlt { get; set; }
        public string LogoHref { get; set; }
        public string LogoWidth { get; set; }

        /// <summary>
        /// Whether to show "Coming soon" text instead of "No integrations currently available" in the UI.
        /// </summary>
        public bool ComingSoon { get; set; } = false;

        public List<IntegrationLink> Links { get; } = new List<IntegrationLink>();
    }
}
