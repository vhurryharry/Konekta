using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using System.Threading.Tasks;
using WCA.Core.Features.Actionstep.Conveyancing.SettlementCalculator;
using WCA.Core.Features.Conveyancing.PolicyRequest;
using WCA.Core.Features.FirstTitle.Connection;
using WCA.Domain.Models.Account;
using WCA.FirstTitle.Client;

namespace WCA.Web.Areas.API.FirstTitle
{
    [Route("api/insurance")]
    public class InsuranceController : Controller
    {
        private readonly UserManager<WCAUser> _userManager;
        private readonly IMediator _mediator;
        private readonly IClock _clock;

        public InsuranceController(
            UserManager<WCAUser> userManager,
            IMediator mediator,
            IClock clock)
        {
            _userManager = userManager;
            _mediator = mediator;
            _clock = clock;
        }

        [HttpGet]
        [Route("first-title-request-from-matter")]
        [ProducesResponseType(typeof(FirstTitlePolicyRequestFromActionstepResponse), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 401)]
        public async Task<FirstTitlePolicyRequestFromActionstepResponse> FirstTitlePolicyRequestFromMatter(string actionstepOrg, int matterId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var firstTitleCredentials = await _mediator.Send(new FirstTitleCredentialsQuery()
            {
                AuthenticatedUser = currentUser
            });

            if (firstTitleCredentials == null)
            {
                throw new MissingFirstTitleCredentialsException(currentUser);
            }

            return await _mediator.Send(new FirstTitlePolicyRequestFromActionstepQuery
            {
                AuthenticatedUser = currentUser,
                MatterId = matterId,
                OrgKey = actionstepOrg
            });
        }

        [HttpPost]
        [Route("first-title-request-from-matter")]
        [ProducesResponseType(typeof(SendFirstTitlePolicyRequestResponse), 200)]
        [ProducesResponseType(typeof(FirstTitlePolicyRequestException), 400)]
        public async Task<SendFirstTitlePolicyRequestResponse> SendPolicyRequestToFirstTitle([FromBody] SendFirstTitlePolicyRequestQuery request)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var firstTitleCredentials = await _mediator.Send(new FirstTitleCredentialsQuery()
            {
                AuthenticatedUser = currentUser
            });

            if (firstTitleCredentials == null)
            {
                throw new MissingFirstTitleCredentialsException(currentUser);
            }

            request.AuthenticatedUser = currentUser;
            request.FirstTitleCredentials = firstTitleCredentials;

            var ftResponse =  await _mediator.Send(request);

            var createDisbursementsCommand = new CreateDisbursementsCommand()
            {
                ActionstepOrg = request.ActionstepOrg,
                MatterId = request.MatterId,
                FirstTitlePrice = ftResponse.Price,
                PolicyNumber = ftResponse.PolicyNumber,
                AuthenticatedUser = currentUser
            };

            await _mediator.Send(createDisbursementsCommand);

            if(ftResponse.AttachmentPaths != null && ftResponse.AttachmentPaths.Length > 0)
            {
                for(var i = 0; i < ftResponse.AttachmentPaths.Length; i ++)
                {
                    var fTAttachment = await _mediator.Send(new SavePolicyPDFToActionstepCommand()
                    {
                        AuthenticatedUser = currentUser,
                        ActionstepOrg = request.ActionstepOrg,
                        MatterId = request.MatterId,
                        FileName = ftResponse.AttachmentPaths[i].FileName,
                        FilePath = ftResponse.AttachmentPaths[i].FileUrl
                    });

                    ftResponse.AttachmentPaths[i] = fTAttachment;
                }
            }

            return ftResponse;
        }

        [HttpPost]
        [Route("first-title-check-and-update-credentials")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        public async Task<bool> CheckAndUpdateFirstTitleCredentials([FromBody] FirstTitleCredential firstTitleCredentials)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var isValidCredentials = await _mediator.Send(new CheckFirstTitleCredentialsQuery()
            {
                AuthenticatedUser = currentUser,
                FirstTitleCredentials = firstTitleCredentials
            });

            if(isValidCredentials)
            {
                // Save the credentials if it's valid
                await _mediator.Send(new StoreFirstTitleCredentialsCommand()
                {
                    AuthenticatedUser = currentUser,
                    FirstTitleCredentials = firstTitleCredentials
                });
            }

            return isValidCredentials;
        }
    }
}