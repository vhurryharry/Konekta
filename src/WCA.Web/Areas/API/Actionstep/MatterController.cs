using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCA.Core.Features.Actionstep.Queries;
using WCA.Domain.Actionstep;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Web.Extensions;
using WCA.Web.FeatureFlags;

namespace WCA.Web.Areas.API.Actionstep
{
    [Route("api/actionstep")]
    public class MatterController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<WCAUser> _userManager;
        private readonly IEventStore _eventStore;
        private IFeatureManager _featureManager;
        private readonly IMapper _mapper;

        public MatterController(
            IMediator mediator,
            UserManager<WCAUser> userManager,
            IEventStore eventStore,
            IFeatureManager featureManager,
            IMapper mapper)
        {
            _mediator = mediator;
            _userManager = userManager;
            _eventStore = eventStore;
            _featureManager = featureManager;
            _mapper = mapper;
        }

        [Route("matter/{orgKey}/{matterId}")]
        [ProducesResponseType(typeof(ActionstepMatterInfo), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 401)]
        public async Task<ActionstepMatterInfo> MatterInfo(string orgKey, int matterId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var actionstepMatterInfo = await _mediator.Send(new ActionstepMatterInfoQuery(orgKey, matterId, currentUser));
            var matterInfo = _mapper.Map<ActionstepMatterInfo>(actionstepMatterInfo);

            HttpContext.SetCurrentOrgKey(orgKey);
            matterInfo.FeatureFlags = Enum.GetValues(typeof(FeatureFlag))
                .OfType<FeatureFlag>()
                .Where(f => _featureManager.IsEnabled(f.ToString()));

            return matterInfo;
        }

        [Authorize(Roles = "GlobalAdministrator")]
        [Route("matter/{orgKey}/{matterId}/events")]
        [ProducesResponseType(typeof(IEnumerable<IEvent>), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 401)]
        public IEnumerable<IEvent> MatterIntegrationEvents(string orgKey, int matterId)
        {
            var aggregateId = ActionstepMatter.ConstructId(orgKey, matterId);
            return _eventStore.GetEventsForAggregate(aggregateId);
        }
    }
}