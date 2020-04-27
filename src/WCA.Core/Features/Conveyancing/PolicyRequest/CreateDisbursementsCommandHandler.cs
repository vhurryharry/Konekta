using MediatR;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;

namespace WCA.Core.Features.Conveyancing.PolicyRequest
{
    public class CreateDisbursementsCommandHandler : IRequestHandler<CreateDisbursementsCommand, ListDisbursementsResponse>
    {
        private readonly IActionstepService _actionstepService;

        public CreateDisbursementsCommandHandler(IActionstepService actionstepService)
        {
            _actionstepService = actionstepService;
        }

        public async Task<ListDisbursementsResponse> Handle(CreateDisbursementsCommand request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Get Actionstep matter info
            var tokenSetQuery = new TokenSetQuery(request.AuthenticatedUser?.Id, request.ActionstepOrg);

            var createDisbursementsRequest = new CreateDisbursementsRequest()
            {
                TokenSetQuery = tokenSetQuery
            };

            createDisbursementsRequest.Disbursements.Add(new Disbursement()
            {
                Date = LocalDate.FromDateTime(DateTime.Now),
                Description = "Premium",
                ImportExternalReference = request.PolicyNumber,
                Quantity = 1,
                UnitPrice = request.FirstTitlePrice.Premium,
                UnitPriceIncludesTax = false,
                Links =
                    {
                        Action = 23,
                        TaxCode = 7 // S 10.0
                    }
            });

            createDisbursementsRequest.Disbursements.Add(new Disbursement()
            {
                Date = LocalDate.FromDateTime(DateTime.Now),
                Description = "StampDuty",
                ImportExternalReference = request.PolicyNumber,
                Quantity = 1,
                UnitPrice = request.FirstTitlePrice.StampDuty,
                UnitPriceIncludesTax = false,
                Links =
                    {
                        Action = 23,
                        TaxCode = 8 // BAS Excluded
                    }
            });

            var response = await _actionstepService.Handle<ListDisbursementsResponse>(createDisbursementsRequest);

            return response;
        }
    }
}
