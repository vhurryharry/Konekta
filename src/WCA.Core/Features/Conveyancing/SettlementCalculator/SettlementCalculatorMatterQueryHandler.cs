using MediatR;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Core.Features.Conveyancing.SettlementCalculator;
using WCA.Domain.Models.Settlement;

namespace WCA.Core.Features.Actionstep.Conveyancing.SettlementCalculator
{
    public class SettlementCalculatorMatterQueryHandler : IRequestHandler<SettlementCalculatorMatterQuery, ActionstepMatter>
    {
        private readonly IActionstepService _actionstepService;
        private readonly IMediator _mediator;
        public SettlementCalculatorMatterQueryHandler(IActionstepService actionstepService, IMediator mediator)
        {
            _actionstepService = actionstepService;
            _mediator = mediator;
        }

        public async Task<ActionstepMatter> Handle(SettlementCalculatorMatterQuery request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var tokenSetQuery = new TokenSetQuery(request.AuthenticatedUser?.Id, request.OrgKey);

            var actionResponse = await _actionstepService.Handle<GetActionResponse>(new GetActionRequest
            {
                TokenSetQuery = tokenSetQuery,
                ActionId = request.MatterId
            });

            var action = actionResponse.Action;

            var actionParticipantsResponse = await _actionstepService.Handle<ListActionParticipantsResponse>(new ListActionParticipantsRequest
            {
                ActionstepId = request.MatterId,
                TokenSetQuery = tokenSetQuery
            });

            string property = "";
            var participantType = actionParticipantsResponse.Linked.ParticipantTypes.SingleOrDefault(t => t.Name == "Property_Address");
            if (participantType != null)
            {
                var actionParticipant = actionParticipantsResponse.ActionParticipants.SingleOrDefault(p => p.Links.ParticipantType == participantType.Id.ToString(CultureInfo.InvariantCulture));

                var participant = actionParticipantsResponse.Linked.Participants.SingleOrDefault(p => p.Id.ToString(CultureInfo.InvariantCulture) == actionParticipant.Links.Participant);

                property = participant.DisplayName;
            }

            var dataCollectionRecordValueResponse = await _actionstepService.Handle<ListDataCollectionRecordValuesResponse>(new ListDataCollectionRecordValuesRequest
            {
                TokenSetQuery = tokenSetQuery,
                ActionstepId = action.Id,
                DataCollectionRecordNames = { "property", "convdet", "keydates" },
                DataCollectionFieldNames = { "lotno", "purprice", "depamount", "adjustdate", "smtdateonly", "smtloc", "smttime", "ConveyType" }
            });

            string state;
            switch (actionResponse.ActionTypeName)
            {
                case "Conveyancing  - Queensland":
                    state = "QLD";
                    break;

                case "Conveyancing - Victoria":
                    state = "VIC";
                    break;

                case "Conveyancing - NSW":
                    state = "NSW";
                    break;

                case "Conveyancing - SA":
                    state = "SA";
                    break;

                default:
                    state = "General";
                    break;
            }

            string adjustDate = dataCollectionRecordValueResponse["keydates", "adjustdate"];
            if (string.IsNullOrEmpty(dataCollectionRecordValueResponse["keydates", "adjustdate"]))
                adjustDate = DateTime.MinValue.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);

            string settlementDate = dataCollectionRecordValueResponse["keydates", "smtdateonly"];
            if (string.IsNullOrEmpty(dataCollectionRecordValueResponse["keydates", "smtdateonly"]))
                settlementDate = DateTime.MinValue.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);

            MatterDetails matterDetails = new MatterDetails(
                    request.MatterId,
                    action.Name,
                    property,
                    DateTime.Parse(adjustDate, CultureInfo.InvariantCulture),
                    DateTime.Parse(settlementDate, CultureInfo.InvariantCulture),
                    string.IsNullOrEmpty(dataCollectionRecordValueResponse["convdet", "smtloc"]) ? "" : dataCollectionRecordValueResponse["convdet", "smtloc"],
                    string.IsNullOrEmpty(dataCollectionRecordValueResponse["convdet", "smttime"]) ? "" : dataCollectionRecordValueResponse["convdet", "smttime"],
                    state,
                    string.IsNullOrEmpty(dataCollectionRecordValueResponse["convdet", "ConveyType"]) ? "Vendor" : dataCollectionRecordValueResponse["convdet", "conveytype"]
                );

            ActionstepMatter actionstepMatter = new ActionstepMatter(matterDetails,
                string.IsNullOrEmpty(dataCollectionRecordValueResponse["convdet", "purprice"]) ? 0 : float.Parse(dataCollectionRecordValueResponse["convdet", "purprice"], CultureInfo.InvariantCulture),
                string.IsNullOrEmpty(dataCollectionRecordValueResponse["convdet", "depamount"]) ? 0 : float.Parse(dataCollectionRecordValueResponse["convdet", "depamount"], CultureInfo.InvariantCulture)
            );

            return actionstepMatter;
        }
    }
}
