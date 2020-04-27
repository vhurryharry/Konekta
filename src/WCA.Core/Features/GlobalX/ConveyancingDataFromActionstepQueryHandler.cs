using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Features.Conveyancing.Services;
using WCA.GlobalX.Client;

namespace WCA.Core.Features.GlobalX
{
    public class ConveyancingDataFromActionstepQueryHandler : IRequestHandler<ConveyancingDataFromActionstepQuery, RequestPropertyInformationFromActionstepResponse>
    {
        private readonly IActionstepService _actionstepService;
        private readonly IGlobalXService _globalXService;
        private readonly IMapper _mapper;
        private readonly IActionstepToWCAMapper _actionstepToWCAMapper;

        public ConveyancingDataFromActionstepQueryHandler(
            IActionstepService actionstepService,
            IGlobalXService globalXService,
            IMapper mapper,
            IActionstepToWCAMapper actionstepToWCAMapper)
        {
            _actionstepService = actionstepService;
            _globalXService = globalXService;
            _mapper = mapper;
            _actionstepToWCAMapper = actionstepToWCAMapper;
        }

        public async Task<RequestPropertyInformationFromActionstepResponse> Handle(ConveyancingDataFromActionstepQuery request, CancellationToken cancellationToken)
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

                var state = State.Unknown;
                switch (wCAConveyancingMatter.ActionType)
                {
                    case "Conveyancing - NSW":
                        state = State.NSW;
                        break;
                    case "Conveyancing  - Queensland":
                        state = State.QLD;
                        break;
                    case "Conveyancing - Victoria":
                        state = State.Vic;
                        break;
                    default:
                        state = State.Unknown;
                        break;
                }

                var matter = new Matter
                {
                    MatterReference = request.MatterId.ToString(CultureInfo.InvariantCulture),
                    MatterEntity = new RealProperty
                    {
                        TitleReference = wCAConveyancingMatter?.PropertyDetails?.TitleReference,
                        LotPlan = new LotPlan() {
                            Lot = wCAConveyancingMatter?.PropertyDetails?.LotNo
                        },
                    }
                };

                var version = new WCA.GlobalX.Client.Version
                {
                    ModelVersion = "http://globalx.com.au/common/model/matter/2013/07/31",
                    ApplicationName = "Konekta",
		            ApplicationVersion = "1.0"
                };

                var settings = new JsonSerializerSettings {
                    SerializationBinder = new GXSerializationBinder(),
                    TypeNameHandling = TypeNameHandling.All
                };

                var entryPoint = string.IsNullOrEmpty(request.EntryPoint) ? "web" : request.EntryPoint;
                var response = new RequestPropertyInformationFromActionstepResponse
                {
                    Matter = JsonConvert.SerializeObject(matter, settings),
                    Version = JsonConvert.SerializeObject(version, settings),
                    GXUri = new Uri(_globalXService.BaseWebUrl, $"{entryPoint}?embed={request.Embed}&state={state}")
                };

                return response;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.All(e => e is InvalidTokenSetException || e is InvalidCredentialsForActionstepApiCallException))
                {
                    throw ex.InnerExceptions.FirstOrDefault();
                }

                throw;
            }
        }
    }
}
