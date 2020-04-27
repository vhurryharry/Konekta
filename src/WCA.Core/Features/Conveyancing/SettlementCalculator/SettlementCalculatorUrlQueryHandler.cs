using MediatR;
using Microsoft.Extensions.Options;
using NodaTime.Text;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;

namespace WCA.Core.Features.Conveyancing.SettlementCalculator
{
    public class SettlementCalculatorUrlQueryHandler : IRequestHandler<SettlementCalculatorUrlQuery, string>
    {
        private readonly IActionstepService _actionstepService;
        private readonly ConveyancingSettings _settings;

        internal static CultureInfo formatProvider = CultureInfo.GetCultureInfo("en-AU");

        public SettlementCalculatorUrlQueryHandler(
            IActionstepService actionstepService,
            IOptions<WCACoreSettings> appSettings)
        {
            if (appSettings is null) throw new ArgumentNullException(nameof(appSettings));

            _actionstepService = actionstepService;
            _settings = appSettings.Value.ConveyancingSettings;
        }

        public async Task<string> Handle(SettlementCalculatorUrlQuery request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            string resultUrl = string.Empty;
            var tokenSetQuery = new TokenSetQuery(request.AuthenticatedUser?.Id, request.OrgKey);

            var actionResponse = await _actionstepService.Handle<GetActionResponse>(new GetActionRequest
            {
                TokenSetQuery = tokenSetQuery,
                ActionId = request.MatterId
            });

            var action = actionResponse.Action;

            if (actionResponse.ActionTypeName.Equals("Conveyancing - Victoria", StringComparison.OrdinalIgnoreCase))
            {
                resultUrl += _settings.SettlementCalculatorBaseUrlVIC;
            }
            // The Konekta add-on uses "Conveyancing - NSW"
            // bytherules use "NSW Conveyancing"
            // Not using property address state as it's a free text field, to avoid issues with typos etc.
            else if (actionResponse.ActionTypeName.Equals("Conveyancing - NSW", StringComparison.OrdinalIgnoreCase) ||
                     actionResponse.ActionTypeName.Equals("NSW Conveyancing", StringComparison.OrdinalIgnoreCase))
            {
                resultUrl += _settings.SettlementCalculatorBaseUrlNSW;
            }
            else
            {
                // The QLD Action Type display name is "Conveyancing  - Queensland" (yes, with the double space).
                // The Conveyancing Action Type for BTR's org "dv4642" is just "Conveyancing".
                // However, we'll just fallback to QLD by default which will catch both of these and any others.
                resultUrl += _settings.SettlementCalculatorBaseUrlQLD;
            }

            var dataCollectionRecordValueResponse = await _actionstepService.Handle<ListDataCollectionRecordValuesResponse>(new ListDataCollectionRecordValuesRequest
            {
                TokenSetQuery = tokenSetQuery,
                ActionstepId = action.Id,
                DataCollectionRecordNames = { "property", "convdet", "keydates" },
                DataCollectionFieldNames = { "lotno", "purprice", "depamount", "adjustdate", "smtdateonly", "smtven", "smttime" }
            });

            var actionParticipantsResponse = await _actionstepService.Handle<ListActionParticipantsResponse>(new ListActionParticipantsRequest
            {
                TokenSetQuery = tokenSetQuery,
                ActionstepId = request.MatterId
            });

            var settlementVenueRawValue = dataCollectionRecordValueResponse["convdet", "smtven"];
            var settlementVenueString = string.Empty;
            if (int.TryParse(settlementVenueRawValue, out int settlementVenueParticipantId))
            {
                var settlementVenueParticipantResponse = await _actionstepService.Handle<GetParticipantResponse>(new GetParticipantRequest
                {
                    TokenSetQuery = tokenSetQuery,
                    Id = settlementVenueParticipantId
                });

                settlementVenueString = settlementVenueParticipantResponse?.Participant?.DisplayName;
            }

            var orgParticipantResponse = await _actionstepService.Handle<GetParticipantResponse>(new GetParticipantRequest
            {
                TokenSetQuery = tokenSetQuery,
                Id = 4
            });

            var conveyancer = actionParticipantsResponse["Conveyancer"].FirstOrDefault();
            var propertyAddress = actionParticipantsResponse["Property_Address"].FirstOrDefault();

            if (!resultUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                resultUrl += "/";
            }

            resultUrl += $"{action.Id}_{request.OrgKey}";
            resultUrl += $"?id={action.Id}_{request.OrgKey}";
            resultUrl += ParamValueOrAsterisks("&matter", action.Name);
            resultUrl += ParamValueOrAsterisks("&address", propertyAddress?.DisplayName);
            resultUrl += ParseAndFormatCurrencyParam("&purprice", dataCollectionRecordValueResponse["convdet", "purprice"]);
            resultUrl += ParseAndFormatCurrencyParam("&depamount", dataCollectionRecordValueResponse["convdet", "depamount"]);
            resultUrl += ParseAndFormatDateParam("&adjustdate", dataCollectionRecordValueResponse["keydates", "adjustdate"]);
            resultUrl += ParseAndFormatDateParam("&settledate", dataCollectionRecordValueResponse["keydates", "smtdateonly"]);
            resultUrl += ParamValueOrAsterisks("&lc", conveyancer?.Email);
            resultUrl += ParamValueOrAsterisks("&div", orgParticipantResponse?.Participant?.CompanyName);
            resultUrl += ParamValueOrAsterisks("&smtloc", settlementVenueString);
            resultUrl += ParamValueOrAsterisks("&smttime", dataCollectionRecordValueResponse["convdet", "smttime"]);

            return resultUrl;
        }

        internal static string ParamValueOrAsterisks(string paramName, string rawValue)
        {
            if (string.IsNullOrEmpty(rawValue))
            {
                return $"{paramName}=****";
            }
            else
            {
                return $"{paramName}={Uri.EscapeDataString(rawValue)}";
            }
        }

        internal static string ParseAndFormatCurrencyParam(string paramName, string actionstepCurrencyAmount)
        {
            var value = string.Empty;

            if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(actionstepCurrencyAmount))
            {
                if (decimal.TryParse(actionstepCurrencyAmount, out decimal actionstepCurrencyAmountDecimal))
                {
                    return $"{paramName}={Uri.EscapeUriString(actionstepCurrencyAmountDecimal.ToString("C", formatProvider))}";
                }
            }

            return value;
        }

        internal static string ParseAndFormatDateParam(string paramName, string actionstepDate)
        {
            var newDate = string.Empty;

            if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(actionstepDate))
            {
                var parseResult = LocalDatePattern.Iso.Parse(actionstepDate);
                if (parseResult.Success)
                {
                    return $"{paramName}={Uri.EscapeDataString(parseResult.Value.ToString("dd MMMM uuuu", formatProvider))}";
                }
            }

            return newDate;
        }
    }
}
