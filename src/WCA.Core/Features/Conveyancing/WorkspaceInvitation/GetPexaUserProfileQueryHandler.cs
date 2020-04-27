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
    public class GetPexaUserProfileQueryHandler: IRequestHandler<GetPexaUserProfileQuery, UserProfileRetrievalResponseType>
    {
        private readonly GetPexaUserProfileQuery.ValidatorCollection _validator;
        private readonly IExtendedPexaService _pexaService;

        public GetPexaUserProfileQueryHandler(
            GetPexaUserProfileQuery.ValidatorCollection validator,
            IExtendedPexaService pexaService)
        {
            _validator = validator;
            _pexaService = pexaService;
        }

        public async Task<UserProfileRetrievalResponseType> Handle(GetPexaUserProfileQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await _validator.ValidateAndThrowAsync(request);

            var subscriberSearchResponse = await _pexaService.Handle<UserProfileRetrievalResponseType>(
                    new RetrieveUserProfileQuery(request.AccessToken), request.AuthenticatedUser, cancellationToken);

            return subscriberSearchResponse;
        }
    }
}
