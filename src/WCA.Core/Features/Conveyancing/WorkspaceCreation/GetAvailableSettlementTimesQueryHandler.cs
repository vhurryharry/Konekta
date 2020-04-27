
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Features.Pexa;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class GetAvailableSettlementTimesQueryHandler : IRequestHandler<GetAvailableSettlementTimesQuery, RetrieveSettlementAvailabilityResponseType>
    {
        private readonly IExtendedPexaService _pexaService;
        private readonly GetAvailableSettlementTimesQuery.ValidatorCollection _validator;

        public GetAvailableSettlementTimesQueryHandler(
            IExtendedPexaService pexaService,
            GetAvailableSettlementTimesQuery.ValidatorCollection validator)
        {
            _pexaService = pexaService;
            _validator = validator;
        }

        public async Task<RetrieveSettlementAvailabilityResponseType> Handle(GetAvailableSettlementTimesQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await _validator.ValidateAndThrowAsync(request);

            var settlementAvailability = await _pexaService.Handle<RetrieveSettlementAvailabilityResponseType>(
                    new RetrieveSettlementAvailabilityQuery(request.RetrieveSettlementAvailabilityParams, request.AccessToken), request.AuthenticatedUser, cancellationToken);

            return settlementAvailability;
        }
    }
}
