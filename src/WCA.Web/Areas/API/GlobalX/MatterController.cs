using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WCA.Core.Features.GlobalX;
using WCA.Domain.Models.Account;

namespace WCA.Web.Areas.API.GlobalX
{
    [Route("api/globalx")]
    [ApiController]
    public class MatterController : Controller
    {
        private readonly UserManager<WCAUser> _userManager;
        private readonly IMediator _mediator;

        public MatterController(
            UserManager<WCAUser> userManager,
            IMediator mediator) 
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("conveyancing-data-from-actionstep")]
        [ProducesResponseType(typeof(RequestPropertyInformationFromActionstepResponse), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 401)]
        public async Task<RequestPropertyInformationFromActionstepResponse> ConveyancingDataFromActionstep(string actionsteporg, int matterid, string entryPoint, bool embed)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new ConveyancingDataFromActionstepQuery
            {
                AuthenticatedUser = currentUser,
                EntryPoint = entryPoint,
                Embed = embed,
                MatterId = matterid,
                OrgKey = actionsteporg
            });
        }
    }
}