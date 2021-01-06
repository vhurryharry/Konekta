using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WCA.Core.Features.InfoTrack;
using WCA.Domain.Models.Account;
using static WCA.Core.Features.InfoTrack.SendMappingsToInfoTrack;

namespace WCA.Web.Areas.API.InfoTrack
{
    /// <summary>
    /// This is an entrypoint to for InfoTrack services.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/infotrack/[controller]/[action]")]
    public class ServicesController : Controller
    {
        private readonly UserManager<WCAUser> userManager;
        private readonly IMediator mediator;
        private readonly ILogger logger;

        private static InfoTrackEntryPoint[] ResolvableEntryPoints
        {
            get
            {
                return new InfoTrackEntryPoint[] {
                    new InfoTrackEntryPoint("PropertyEnquiry", "NSW", "NswEnquiries/Search"),
                    new InfoTrackEntryPoint("PropertyEnquiry", "VIC", "VicEnquiries/PropertyEnquiry"),
                    new InfoTrackEntryPoint("PropertyEnquiry", "QLD", "qldenquiries/search"),
                    new InfoTrackEntryPoint("PropertyEnquiry", "ACT", "ActEnquiries/Certificate.aspx"),
                    new InfoTrackEntryPoint("PropertyEnquiry", "WA",  "Manual/wapropertyinterestreport"),
                    new InfoTrackEntryPoint("PropertyEnquiry", "SA",  "Manual/sapropertyinterestreport"),
                    new InfoTrackEntryPoint("PropertyEnquiry", "NT",  "Manual/PrePurchaseReports/PrepurchaseOrdersReport?state=NT"),
                    new InfoTrackEntryPoint("PropertyEnquiry", "TAS", "Manual/PrePurchaseReports/PrepurchaseOrdersReport?state=TAS"),
                    new InfoTrackEntryPoint("TitleSearch", "NSW", "Nsw/Search"),
                    new InfoTrackEntryPoint("TitleSearch", "VIC", "Victoria/Search"),
                    new InfoTrackEntryPoint("TitleSearch", "QLD", "Queensland/search"),
                    new InfoTrackEntryPoint("TitleSearch", "ACT", "ACT/Search"),
                    new InfoTrackEntryPoint("TitleSearch", "WA",  "WA/Search"),
                    new InfoTrackEntryPoint("TitleSearch", "SA",  "SA/Search"),
                    new InfoTrackEntryPoint("TitleSearch", "NT",  "NT/Search/Index"),
                    new InfoTrackEntryPoint("TitleSearch", "TAS", "Tasmania/search"),
                };
            }
        }

        public ServicesController(
            UserManager<WCAUser> userManager,
            IMediator mediator,
            ILoggerFactory loggerFactory)
        {
            this.userManager = userManager;
            this.mediator = mediator;
            logger = loggerFactory.CreateLogger<ServicesController>();
        }

        [HttpGet]
        public static InfoTrackEntryPoint[] GetResolvableInfoTrackEntryPoints()
        {
            return ResolvableEntryPoints;
        }

        [HttpPost]
        public async Task<InfoTrackMappedDataUrl> GetInfoTrackUrlWithMatterInfo([FromBody] SendActionstepMatterInfoModel sendActionstepMatterInfoModel)
        {
            if (sendActionstepMatterInfoModel is null)
            {
                throw new ArgumentNullException(nameof(sendActionstepMatterInfoModel));
            }

            var currentUser = await userManager.GetUserAsync(User);

            var infoTrackMapping = await mediator.Send(new GetMappingDataFromActionstep.GetMappingDataFromActionstepQuery
            {
                ActionstepOrgKey = sendActionstepMatterInfoModel.OrgKey,
                AuthenticatedUser = currentUser,
                MatterId = sendActionstepMatterInfoModel.MatterId
            });

            if (!string.IsNullOrEmpty(sendActionstepMatterInfoModel.ResolvableEntryPoint))
            {
                infoTrackMapping.InfoTrackMappingData.EntryPoint = ResolveEntryPoint(
                    sendActionstepMatterInfoModel.ResolvableEntryPoint,
                    infoTrackMapping?.InfoTrackMappingData?.PropertyDetails?[0]?.PropertyAddress?.State);
            }

            var result = await mediator.Send(infoTrackMapping);

            return result;
        }

        [NonAction]
        private static string ResolveEntryPoint(string resolvableEntryPoint, string state)
        {
            if (string.IsNullOrEmpty(resolvableEntryPoint))
            {
                return null;
            }

            if (state == null)
            {
                // Make sure 'state' isn't null to ensure the .Equals comparison doesn't throw.
                state = string.Empty;
            }
            else
            {
                state = state.Trim();
            }

            // See if there is a match in the list of known resolvable entrypoints
            var entryPointFromLookup = ResolvableEntryPoints
                .SingleOrDefault(e =>
                    resolvableEntryPoint.Equals(e.EntryPointType, StringComparison.InvariantCultureIgnoreCase) &&
                    state.Equals(e.State, StringComparison.InvariantCultureIgnoreCase))
                ?.EntryPointPath;

            if (!string.IsNullOrEmpty(entryPointFromLookup))
            {
                // If we found a match in the lookup list, we'll return it
                return entryPointFromLookup;
            }

            // Finally, if we got here either the resolvableEntryPoint doesn't need resolving
            // or it should be resolved but there was no valid 'state' supplied. Here we'll
            // handle the latter case.
            if (ResolvableEntryPoints.Any(e => resolvableEntryPoint.Equals(
                    e.EntryPointType,
                    StringComparison.InvariantCultureIgnoreCase)))
            {
                // If we're here it means that a 'key' was supplied but
                // we don't know which state for. So we return null to
                // just send the user to the main page.
                return null;
            }

            // Fall back to returning the raw resolvableEntryPoint value.
            // InfoTrack expects there not to be a starting slash, so we'll trim
            // just in case one has been supplied.
            return resolvableEntryPoint?.TrimStart('/');
        }
    }

    public class InfoTrackEntryPoint
    {
        private readonly string _entryPointPath;

        public string EntryPointType { get; }
        public string State { get; }

        /// <summary>
        /// Gets the entry point path to go to InfoTrack. This should NOT start with a slash (/).
        /// </summary>
        /// <value>
        /// The entry point path.
        /// </value>
        public string EntryPointPath => _entryPointPath?.TrimStart('/');

        public InfoTrackEntryPoint(string entryPointType, string state, string entryPointPath)
        {
            EntryPointType = entryPointType;
            State = state;
            _entryPointPath = entryPointPath;
        }
    }

    public class SendActionstepMatterInfoModel
    {
        public int MatterId { get; set; }
        public string OrgKey { get; set; }
        public string ResolvableEntryPoint { get; set; }
    }
}
