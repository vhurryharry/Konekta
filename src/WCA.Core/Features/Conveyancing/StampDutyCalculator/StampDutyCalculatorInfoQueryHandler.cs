using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Features.Actionstep.Queries;
using WCA.Core.Features.Actionstep.Responses;

namespace WCA.Core.Features.Conveyancing.StampDutyCalculator
{
    public class StampDutyCalculatorInfoQueryHandler : IRequestHandler<StampDutyCalculatorInfoQuery, StampDutyCalculatorInfo>
    {
        private readonly StampDutyCalculatorInfoQuery.Validator _validator;
        private readonly IActionstepService _actionstepService;

        public StampDutyCalculatorInfoQueryHandler(
            StampDutyCalculatorInfoQuery.Validator validator,
            IActionstepService actionstepService)
        {
            _validator = validator;
            _actionstepService = actionstepService;
        }

        public async Task<StampDutyCalculatorInfo> Handle(StampDutyCalculatorInfoQuery message, CancellationToken token)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));

            ValidationResult result = _validator.Validate(message);
            if (!result.IsValid)
            {
                throw new ValidationException("Invalid input.", result.Errors);
            }

            var request = new ListDataCollectionRecordValuesRequest()
            {
                TokenSetQuery = new TokenSetQuery(message.AuthenticatedUser?.Id, message.OrgKey),
                ActionstepId = message.MatterId
            };

            request.DataCollectionRecordNames.Add("convdet");
            request.DataCollectionFieldNames.AddRange(new[] { "purprice", "ConveySubType" });
            var actionstepResponse = await _actionstepService.Handle<ListDataCollectionRecordValuesResponse>(request);

            if (actionstepResponse == null)
            {
                return new StampDutyCalculatorInfo(0, string.Empty);
            }

#pragma warning disable CA1806 // Do not ignore method results
            decimal.TryParse(actionstepResponse["convdet", "purprice"], out decimal purchasePrice);
#pragma warning restore CA1806 // Do not ignore method results

            return new StampDutyCalculatorInfo(purchasePrice, actionstepResponse["convdet", "ConveySubType"]);
        }
    }
}