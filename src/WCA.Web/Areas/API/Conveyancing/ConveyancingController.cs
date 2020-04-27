using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using System.Threading.Tasks;
using WCA.Core.Features.Actionstep;
using WCA.Core.Features.Conveyancing.SettlementCalculator;
using WCA.Core.Features.Conveyancing.StampDutyCalculator;
using WCA.Core.Features.Conveyancing.WorkspaceCreation;
using WCA.Core.Features.Conveyancing.WorkspaceInvitation;
using WCA.Core.Features.Pexa.Authentication;
using WCA.Domain.Models.Account;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Web.Areas.API.Conveyancing
{
    [Route("api/conveyancing")]
    public class ConveyancingController : Controller
    {
        private readonly UserManager<WCAUser> _userManager;
        private readonly IMediator _mediator;
        private readonly IClock _clock;

        public ConveyancingController(
            UserManager<WCAUser> userManager,
            IMediator mediator,
            IClock clock)
        {
            _userManager = userManager;
            _mediator = mediator;
            _clock = clock;
        }

        [HttpGet]
        [Route("pexa-workspace-creation-request-from-matter")]
        [ProducesResponseType(typeof(PEXAWorkspaceCreationRequestWithActionstepResponse), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 401)]
        public async Task<PEXAWorkspaceCreationRequestWithActionstepResponse> PEXAWorkspaceCreationRequestFromMatter(string actionsteporg, int matterid)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var pexaToken = await _mediator.Send(new PexaApiTokenQuery()
            {
                AuthenticatedUser = currentUser
            });

            if (pexaToken == null || pexaToken.CheckIfAccessTokenHasExpired(_clock))
            {
                throw new MissingOrInvalidPexaApiTokenException(currentUser);
            }

            var pexaWorkspaceRequestWithActionstepData = await _mediator.Send(new PEXAWorkspaceCreationRequestFromActionstepQuery
            {
                AuthenticatedUser = currentUser,
                MatterId = matterid,
                ActionstepOrg = actionsteporg
            });

            var userProfile = await _mediator.Send(new GetPexaUserProfileQuery() 
            { 
                AuthenticatedUser = currentUser, 
                AccessToken = pexaToken.AccessToken 
            });

            pexaWorkspaceRequestWithActionstepData.ExistingPexaWorkspace = await _mediator.Send(new GetPexaWorkspaceForActionstepMatterQuery
            {
                AuthenticatedUser = currentUser,
                MatterId = matterid,
                ActionstepOrg = actionsteporg
            });

            pexaWorkspaceRequestWithActionstepData.WorkgroupList = userProfile.UserProfile.WorkgroupList;

            return pexaWorkspaceRequestWithActionstepData;
        }

        [HttpPost]
        [Route("pexa-workspace-creation-request-from-matter")]
        [ProducesResponseType(typeof(CreatePexaWorkspaceResponse), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 401)]
        public async Task<CreatePexaWorkspaceResponse> PEXAWorkspaceCreationRequestFromMatter([FromBody] CreatePexaWorkspaceCommand command)
        {
            if (command is null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            command.AuthenticatedUser = currentUser;

            var pexaApiToken = await _mediator.Send(new PexaApiTokenQuery() { AuthenticatedUser = currentUser});
            command.AccessToken = pexaApiToken.AccessToken;

            var createPexaWorkspaceResponse = await _mediator.Send(command);

            var storePexaWorkspaceIdInActionstepCommand = new StorePexaWorkspaceIdInActionstepMatterCommand()
            {
                AuthenticatedUser = currentUser,
                WorkspaceId = createPexaWorkspaceResponse.WorkspaceId,
                ActionstepOrg = command.OrgKey,
                MatterId = command.MatterId
            };

            await _mediator.Send(storePexaWorkspaceIdInActionstepCommand);

            var storePexaWorkspaceInfoIntoDBCommand = new StorePexaWorkspaceInfoIntoDBCommand()
            {
                AuthenticatedUser = currentUser,
                ActionstepOrg = command.OrgKey,
                MatterId = command.MatterId,
                WorkspaceId = createPexaWorkspaceResponse.WorkspaceId,
                WorkspaceUri = createPexaWorkspaceResponse.WorkspaceUri
            };

            await _mediator.Send(storePexaWorkspaceInfoIntoDBCommand);

            return createPexaWorkspaceResponse;
        }

        [HttpPost]
        [Route("validate-land-title-reference")]
        public async Task<LandTitleReferenceVerificationResponseType> ValidateLandTitleReference([FromBody] LandTitleReferenceAndJurisdiction landTitleReferenceAndJurisdiction)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var pexaApiToken = await _mediator.Send(new PexaApiTokenQuery() { AuthenticatedUser = currentUser });

            if (pexaApiToken == null || pexaApiToken.CheckIfAccessTokenHasExpired(_clock))
            {
                throw new MissingOrInvalidPexaApiTokenException(currentUser);
            }

            ValidateLandTitleReferenceQuery query = new ValidateLandTitleReferenceQuery();

            query.LandTitleReferenceAndJurisdiction = landTitleReferenceAndJurisdiction;
            query.AuthenticatedUser = currentUser;
            query.AccessToken = pexaApiToken.AccessToken;

            return await _mediator.Send(query);
        }

        [HttpPost]
        [Route("get-pexa-workspace-summary")]
        [ProducesResponseType(typeof(WorkspaceSummaryResponseType), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 401)]
        public async Task<WorkspaceSummaryResponseType> GetPexaWorkspaceSummary([FromBody] RetrieveWorkspaceSummaryParameters workspaceSummaryParameters)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var pexaApiToken = await _mediator.Send(new PexaApiTokenQuery() { AuthenticatedUser = currentUser });

            if (pexaApiToken == null || pexaApiToken.CheckIfAccessTokenHasExpired(_clock))
            {
                throw new MissingOrInvalidPexaApiTokenException(currentUser);
            }

            GetPexaWorkspaceSummaryQuery query = new GetPexaWorkspaceSummaryQuery();
            query.RetrieveWorkspaceSummaryParameters = workspaceSummaryParameters;
            query.AuthenticatedUser = currentUser;
            query.AccessToken = pexaApiToken.AccessToken;

            return await _mediator.Send(query);
        }

        [HttpGet]
        [Route("search-subscriber")]
        public async Task<SubscriberSearchResponseType> SearchSubscriber(string subscriberName, string subscriberId)
        {
            SearchSubscriberQuery query = new SearchSubscriberQuery();

            query.SubscriberInformation = new SubscriberInformation {
                SearchSubscriberId = subscriberId,
                SubscriberName = subscriberName
            };

            var currentUser = await _userManager.GetUserAsync(User);
            query.AuthenticatedUser = currentUser;

            var pexaApiToken = await _mediator.Send(new PexaApiTokenQuery() { AuthenticatedUser = currentUser });
            query.AccessToken = pexaApiToken.AccessToken;

            return await _mediator.Send(query);
        }

        [HttpPost]
        [Route("invite-subscribers")]
        [ProducesResponseType(typeof(CreateWorkspaceInvitationResponseType[]), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 401)]
        public async Task<CreateWorkspaceInvitationResponseType[]> InviteSubscribers([FromBody] CreateWorkspaceInvitationRequestType[] invitationRequests)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var pexaApiToken = await _mediator.Send(new PexaApiTokenQuery() { AuthenticatedUser = currentUser });

            if (pexaApiToken == null || pexaApiToken.CheckIfAccessTokenHasExpired(_clock))
            {
                throw new MissingOrInvalidPexaApiTokenException(currentUser);
            }

            var userProfile = await _mediator.Send(new GetPexaUserProfileQuery() { AuthenticatedUser = currentUser, AccessToken = pexaApiToken.AccessToken });
            foreach(var invitationRequest in invitationRequests)
            {
                invitationRequest.SubscriberId = userProfile.UserProfile.SubscriberId;
            }

            CreatePexaWorkspaceInvitationCommand command = new CreatePexaWorkspaceInvitationCommand
            {
                AccessToken = pexaApiToken.AccessToken,
                AuthenticatedUser = currentUser,
                PexaWorkspaceInvitationRequests = invitationRequests
            };

            return await _mediator.Send(command);
        }

        [HttpPost]
        [Route("get-available-settlement-times")]
        [ProducesResponseType(typeof(RetrieveSettlementAvailabilityResponseType), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 401)]
        public async Task<RetrieveSettlementAvailabilityResponseType> GetAvailableSettlementTimes([FromBody] RetrieveSettlementAvailabilityParams retrieveSettlementAvailabilityParams)
        {
            if(retrieveSettlementAvailabilityParams == null)
            {
                throw new System.ArgumentNullException(nameof(retrieveSettlementAvailabilityParams));
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var pexaApiToken = await _mediator.Send(new PexaApiTokenQuery() { AuthenticatedUser = currentUser });

            if (pexaApiToken == null || pexaApiToken.CheckIfAccessTokenHasExpired(_clock))
            {
                throw new MissingOrInvalidPexaApiTokenException(currentUser);
            }

            var userProfile = await _mediator.Send(new GetPexaUserProfileQuery() { AuthenticatedUser = currentUser, AccessToken = pexaApiToken.AccessToken });
            retrieveSettlementAvailabilityParams.SubscriberId = userProfile.UserProfile.SubscriberId;

            GetAvailableSettlementTimesQuery query = new GetAvailableSettlementTimesQuery
            {
                AccessToken = pexaApiToken.AccessToken,
                AuthenticatedUser = currentUser,
                RetrieveSettlementAvailabilityParams = retrieveSettlementAvailabilityParams
            };

            return await _mediator.Send(query);
        }

        [HttpGet]
        [Route("old-settlement-calculator/redirect-with-matter-data/{actionstepOrg}/{matterId}")]
        public async Task<RedirectResult> BuildSettlementCalculatorUrl(string actionstepOrg, int matterId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return new RedirectResult(await _mediator.Send(new SettlementCalculatorUrlQuery
            {
                AuthenticatedUser = currentUser,
                OrgKey = actionstepOrg,
                MatterId = matterId
            }));
        }

        [HttpGet]
        [Route("stamp-duty-calculator-data/{actionstepOrg}/{matterId}")]
        [ProducesResponseType(typeof(StampDutyCalculatorInfo), 200)]
        [ProducesResponseType(typeof(MatterNotFoundException), 401)]
        public async Task<StampDutyCalculatorInfo> GetStampDutyCalculatorInfo(string actionstepOrg, int matterId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new StampDutyCalculatorInfoQuery(actionstepOrg, matterId, currentUser));
        }
    }
}