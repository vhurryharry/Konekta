using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCA.Core.Features.Actionstep;
using WCA.Core.Features.InfoTrack;
using WCA.Core.Services;
using WCA.Domain.Models.Account;
using OrderHistoryResult = WCA.Core.Features.InfoTrack.OrderHistoryResult;

namespace WCA.Web.Areas.API.InfoTrack
{
    [Route("api/infotrack/[controller]/[action]")]
    public class OrdersController : Controller
    {
        private readonly UserManager<WCAUser> _userManager;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ITelemetryLogger _telemetryLogger;

        public OrdersController(
            UserManager<WCAUser> userManager,
            IMediator mediator,
            ILogger<OrdersController> logger,
            ITelemetryLogger telemetryLogger)
        {
            _userManager = userManager;
            _mediator = mediator;
            _logger = logger;
            _telemetryLogger = telemetryLogger;
        }

        [HttpPost]
        public async Task<InfoTrackOrderResult[]> GetOrderHistory([FromBody]GetInfoTrackOrderHistory.GetInfoTrackOrderHistoryQuery query)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            query.AuthenticatedUser = await _userManager.GetUserAsync(User);
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<List<OrderHistoryResult>> GetOrderHistoryPreview([FromBody]GetInfoTrackOrderHistoryPreview.GetInfoTrackOrderHistoryPreviewQuery query)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            query.AuthenticatedUser = currentUser;
            var result = await _mediator.Send(query);

            var groupResult = result.GroupBy(order => order.ActionstepMatterId,
                order => order,
                (key, orders) => new OrderHistoryResult
                {
                    MatterId = key,
                    TotalFee = orders.Sum(o => o.InfoTrackTotalFee),
                    TotalFeeGst = orders.Sum(o => o.InfoTrackTotalFeeGST),
                    TotalFeeTotal = orders.Sum(o => o.InfoTrackTotalFeeTotal),
                    Orders = orders.ToList()
                }).OrderBy(m => m.MatterId).ToList();

            // Group the results by OrgKey then retrieved all MatterId's related to the key 
            // Retreive the Matter action names for each unique organisation key
            var groupedByOrgKey = result.GroupBy(order => order.ActionstepOrgKey,
                order => order,
                (key, orders) => new { Key = key, MatterIds = orders.Select(o => o.ActionstepMatterId).Distinct() }).ToList();

            foreach (var groupedByOrg in groupedByOrgKey)
            {
                var command = new GetActionstepActions.GetActionstepActionsCommand
                {
                    AuthenticatedUser = currentUser,
                    ActionstepOrgKey = groupedByOrg.Key,
                    MatterIds = groupedByOrg.MatterIds
                };

                try
                {
                    var actionNames = (await _mediator.Send(command)).Actions;
                    groupResult.ForEach(r => r.Name = actionNames.SingleOrDefault(n => n.Id == r.MatterId)?.Name);
                }
#pragma warning disable CA1031 // Do not catch general exception types: Exception is logged
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    // Don't fail the whole request if we can't get Action/Matter names.
                    _telemetryLogger.TrackException(ex, new Dictionary<string, string>() {
                        { "User", currentUser.Id },
                        { "ActionstepOrg", groupedByOrg.Key }
                    });
                }
            }

            return groupResult;

        }

        [HttpPost]
        public async Task Reconcile(int infoTrackOrderId)
        {
            await _mediator.Send(new ReconcileInfoTrackOrder.ReconcileInfoTrackOrderCommand()
            {
                AuthenticatedUser = await _userManager.GetUserAsync(User),
                InfoTrackOrderId = infoTrackOrderId
            });
        }

        [HttpPost]
        public async Task UnReconcile(int infoTrackOrderId)
        {
            await _mediator.Send(new UnReconcileInfoTrackOrder.UnReconcileInfoTrackOrderCommand()
            {
                AuthenticatedUser = await _userManager.GetUserAsync(User),
                InfoTrackOrderId = infoTrackOrderId
            });
        }
    }
}
