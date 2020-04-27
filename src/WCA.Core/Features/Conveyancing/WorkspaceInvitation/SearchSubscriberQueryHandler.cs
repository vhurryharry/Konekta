using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Features.Pexa;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Conveyancing.WorkspaceInvitation
{
    public class SearchSubscriberQueryHandler : IRequestHandler<SearchSubscriberQuery, SubscriberSearchResponseType>
    {
        private readonly SearchSubscriberQuery.ValidatorCollection _validator;
        private readonly IExtendedPexaService _pexaService;

        public SearchSubscriberQueryHandler(
            SearchSubscriberQuery.ValidatorCollection validator,
            IExtendedPexaService pexaService)
        {
            _validator = validator;
            _pexaService = pexaService;
        }

        public async Task<SubscriberSearchResponseType> Handle(SearchSubscriberQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await _validator.ValidateAndThrowAsync(request);

            var subscriberSearchResponse = await _pexaService.Handle<SubscriberSearchResponseType>(
                    new SearchSubscriberRequestQuery(request.SubscriberInformation, request.AccessToken), request.AuthenticatedUser, cancellationToken);

            return subscriberSearchResponse;
        }
    }
}
