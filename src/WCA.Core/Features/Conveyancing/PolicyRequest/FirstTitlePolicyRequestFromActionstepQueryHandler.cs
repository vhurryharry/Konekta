using MediatR;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Features.Conveyancing.Services;
using WCA.Domain.Conveyancing;
using WCA.FirstTitle.Client;
using WCA.FirstTitle.Client.Resources;

namespace WCA.Core.Features.Conveyancing.PolicyRequest
{
    class FirstTitlePolicyRequestFromActionstepQueryHandler : IRequestHandler<FirstTitlePolicyRequestFromActionstepQuery, FirstTitlePolicyRequestFromActionstepResponse>
    {
        private readonly IActionstepService _actionstepService;
        private readonly IActionstepToWCAMapper _actionstepToWCAMapper;

        public FirstTitlePolicyRequestFromActionstepQueryHandler(
            IActionstepService actionstepService, 
            IActionstepToWCAMapper actionstepToWCAMapper
        )
        {
            _actionstepService = actionstepService;
            _actionstepToWCAMapper = actionstepToWCAMapper;
        }

        public Task<FirstTitlePolicyRequestFromActionstepResponse> Handle(FirstTitlePolicyRequestFromActionstepQuery request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Get Actionstep matter info
            var tokenSetQuery = new TokenSetQuery(request.AuthenticatedUser?.Id, request.OrgKey);

            try
            {
                // New token refresh handling means these can run in parallel.
                var actionResponseTask = _actionstepService.Handle<GetActionResponse>(new GetActionRequest
                {
                    ActionId = request.MatterId,
                    TokenSetQuery = tokenSetQuery
                });

                var actionParticipantsResponseTask = _actionstepService.Handle<ListActionParticipantsResponse>(new ListActionParticipantsRequest
                {
                    ActionstepId = request.MatterId,
                    TokenSetQuery = tokenSetQuery
                });

                var dataCollectionRecordValuesResponseTask = _actionstepService.Handle<ListDataCollectionRecordValuesResponse>(new ListDataCollectionRecordValuesRequest
                {
                    ActionstepId = request.MatterId,
                    TokenSetQuery = tokenSetQuery,
                    DataCollectionRecordNames = { "property", "convdet", "keydates" },
                    DataCollectionFieldNames = { "titleref", "lotno", "planno", "plantype", "smtdateonly", "smttime", "purprice", "ConveyType" }
                });

                Task.WaitAll(actionResponseTask, actionParticipantsResponseTask, dataCollectionRecordValuesResponseTask);

                // Transform Actionstep matter info into generic WCA Conveyancing Matter type
                // using specific configuration for this client (to account for Actionstep
                // action type configuration).
                var wCAConveyancingMatter = _actionstepToWCAMapper.MapFromActionstepTypes(
                    actionResponseTask.Result,
                    actionParticipantsResponseTask.Result,
                    dataCollectionRecordValuesResponseTask.Result);

                var asSourceProperty = wCAConveyancingMatter.PropertyAddresses.FirstOrDefault();
                var sourceProperty = asSourceProperty == default(Domain.Conveyancing.Party) ? new FTParty() : FTParty.FromASParty(asSourceProperty);

                var actionstepData = new FTActionstepMatter()
                {
                    Title = new FTTitle() {
                        TitleInfoType = TitleInfoType.Reference,
                        TitleReference = wCAConveyancingMatter.PropertyDetails.TitleReference
                    },
                    SourceProperty = sourceProperty,
                    PurchasePrice = wCAConveyancingMatter.PurchasePrice,
                    SettlementDate = wCAConveyancingMatter.SettlementDate.ToDateTimeUnspecified()
                };

                foreach (var buyer in wCAConveyancingMatter.Buyers)
                {
                    actionstepData.Buyers.Add(FTParty.FromASParty(buyer));
                }

                FirstTitlePolicyRequestFromActionstepResponse firstTitlePolicyRequestFromActionstepResponse = new FirstTitlePolicyRequestFromActionstepResponse
                {
                    ActionstepData = actionstepData,
                    RequestPolicyOptions = new RequestPolicyOptions()
                };

                return Task.FromResult(firstTitlePolicyRequestFromActionstepResponse);
            }
            catch (UnauthorizedAccessException uax)
            {
                throw new InvalidCredentialsForActionstepApiCallException(string.Empty, uax)
                {
                    ActionstepOrgKey = request.OrgKey,
                    User = request.AuthenticatedUser,
                };
            }
        }
    }
}
