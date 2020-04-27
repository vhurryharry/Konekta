using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Actionstep;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using static WCA.Core.Features.Integrations.IntegrationsQuery;

namespace WCA.Core.Features.Integrations
{
    public class IntegrationsQuery : IAuthenticatedQuery<IEnumerable<Integration>>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public string ActionstepOrg { get; set; }
        public int MatterId { get; set; }

        public class Validator : AbstractValidator<IntegrationsQuery>
        {
            public Validator()
            {
                RuleFor(q => q.AuthenticatedUser).NotNull();
                RuleFor(q => q.AuthenticatedUser.Id).NotEmpty();
                RuleFor(q => q.ActionstepOrg).NotEmpty();
            }
        }

        public class IntegrationsQueryHandler : IRequestHandler<IntegrationsQuery, IEnumerable<Integration>>
        {
            private readonly WCADbContext _wCADbContext;
            private readonly Validator _validator;
            private readonly IMemoryCache _memoryCache;

            public IntegrationsQueryHandler(
                WCADbContext wCADbContext,
                Validator validator,
                IMemoryCache memoryCache)
            {
                _wCADbContext = wCADbContext;
                _validator = validator;
                _memoryCache = memoryCache;
            }

            public async Task<IEnumerable<Integration>> Handle(IntegrationsQuery request, CancellationToken cancellationToken)
            {
                if (request is null) throw new System.ArgumentNullException(nameof(request));
                _validator.ValidateAndThrow(request);

                var enabledIntegrationIds = _wCADbContext.IntegrationSettings.AsNoTracking().Where(s =>
                        (s.ActionstepOrgKey == request.ActionstepOrg || s.ActionstepOrgKey == ActionstepDefaults.AllOrgsKey)
                        &&
                        (s.UserId == request.AuthenticatedUser.Id || s.UserId == WCAUser.AllUsersId))
                    .OrderBy(s => s.SortOrder)
                    .AsEnumerable()
                    .GroupBy(s => s.IntegrationId)
                    .Where(s => s.All(gs => !gs.HideIntegration))
                    .Select(s => s.Key)
                    .Distinct()
                    .ToList();

                var enabledIntegrationLinkIds = _wCADbContext.IntegrationLinkSettings.AsNoTracking().Where(s =>
                        (s.ActionstepOrgKey == request.ActionstepOrg || s.ActionstepOrgKey == ActionstepDefaults.AllOrgsKey)
                        &&
                        (s.UserId == request.AuthenticatedUser.Id || s.UserId == WCAUser.AllUsersId))
                    .OrderBy(s => s.SortOrder)
                    .AsEnumerable()
                    .GroupBy(s => s.IntegrationLinkId)
                    .Where(s => s.All(gs => !gs.HideIntegrationLink))
                    .Select(s => s.Key)
                    .Distinct()
                    .ToList();

                var allIntegrations = _wCADbContext.Integrations.AsNoTracking().Include(i => i.Links).ToList();

                var integrationsForResult = new List<Integration>();
                foreach (var integrationSetting in enabledIntegrationIds)
                {
                    var integration = allIntegrations.SingleOrDefault(i => i.Id == integrationSetting);

                    if (!(integration is null))
                    {
                        var linksForThisIntegration = new List<Link>();

                        foreach (var link in integration.Links)
                        {
                            if (enabledIntegrationLinkIds.Contains(link.Id))
                            {
                                linksForThisIntegration.Add(new Link(link.Title, link.Href, link.OpenInNewWindow, link.IsReactLink, link.IsBeta, link.Disabled, link.ToolTip));
                            }
                        }

                        integrationsForResult.Add(
                            new Integration(
                                integration.Title,
                                new Logo(integration.LogoSrc, integration.LogoAlt, integration.LogoHref, integration.LogoWidth),
                                linksForThisIntegration,
                                integration.ComingSoon));
                    }
                }

                return integrationsForResult;
            }
        }

        public class Logo
        {
            public string Src { get; }
            public string Alt { get; }
            public string Href { get; }
            public string Width { get; }

            public Logo(string src, string alt, string href, string width)
            {
                Src = src;
                Alt = alt;
                Href = href;
                Width = width;
            }
        }

        public class Link
        {
            public string Title { get; }
            public string Href { get; }
            public bool OpenInNewWindow { get; }
            public bool IsReactLink { get; }
            public bool IsBeta { get; }
            public bool Disabled { get; }
            public string ToolTip { get; }

            public Link(string title, string href, bool openInNewWindow, bool isReactLink, bool isBeta, bool disabled, string toolTip = null)
            {
                Title = title;
                Href = href;
                OpenInNewWindow = openInNewWindow;
                IsReactLink = isReactLink;
                IsBeta = isBeta;
                Disabled = disabled;
                ToolTip = toolTip;
            }
        }

        public class Integration
        {
            public string Title { get; }
            public bool ComingSoon { get; }
            public Logo Logo { get; }
            public IReadOnlyCollection<Link> Links { get; }

            public Integration(string title, Logo logo, IEnumerable<Link> links, bool comingSoon = false)
            {
                Title = title;
                ComingSoon = comingSoon;
                Logo = logo;
                Links = new List<Link>(links).AsReadOnly();
            }
        }
    }
}
