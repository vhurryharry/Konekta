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
    public class GetPexaWorkgroupsQueryHandler : IRequestHandler<GetPexaWorkgroupsQuery, WorkgroupListRetrievalResponseType>
    {
        private readonly GetPexaWorkgroupsQuery.ValidatorCollection _validator;
        private readonly IExtendedPexaService _pexaService;

        public GetPexaWorkgroupsQueryHandler(
            GetPexaWorkgroupsQuery.ValidatorCollection validator,
            IExtendedPexaService pexaService)
        {
            _validator = validator;
            _pexaService = pexaService;
        }

        public async Task<WorkgroupListRetrievalResponseType> Handle(GetPexaWorkgroupsQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await _validator.ValidateAndThrowAsync(request);

            var subscriberSearchResponse = await _pexaService.Handle<WorkgroupListRetrievalResponseType>(
                    new RetrieveWorkgroupsQuery(request.AccessToken), request.AuthenticatedUser, cancellationToken);

            return subscriberSearchResponse;
        }
    }
}
