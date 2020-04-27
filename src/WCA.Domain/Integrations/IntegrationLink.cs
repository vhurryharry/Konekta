using System;
using WCA.Domain.Abstractions;

namespace WCA.Domain.Integrations
{
    /// <summary>
    /// Uses Guid identifier to allow for seeding and consistent base set of links.
    /// </summary>
    public class IntegrationLink : EntityBase, IEntityWithGuid
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Href { get; set; }
        public bool OpenInNewWindow { get; set; }
        public bool IsReactLink { get; set; }
        public bool IsBeta { get; set; }
        public bool Disabled { get; set; }
        public string ToolTip { get; set; }

        public Integration Integration { get; set; }
        public Guid IntegrationId { get; set; }
    }
}