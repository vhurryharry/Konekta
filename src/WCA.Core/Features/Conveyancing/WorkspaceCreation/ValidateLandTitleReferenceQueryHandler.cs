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
    public class ValidateLandTitleReferenceQueryHandler : IRequestHandler<ValidateLandTitleReferenceQuery, LandTitleReferenceVerificationResponseType>
    {
        private readonly IExtendedPexaService _pexaService;
        private readonly ValidateLandTitleReferenceQuery.ValidatorCollection _validator;

        public ValidateLandTitleReferenceQueryHandler(
            IExtendedPexaService pexaService,
            ValidateLandTitleReferenceQuery.ValidatorCollection validator)
        {
            _pexaService = pexaService;
            _validator = validator;
        }

        public async Task<LandTitleReferenceVerificationResponseType> Handle(ValidateLandTitleReferenceQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await _validator.ValidateAndThrowAsync(request);

            var landTitleReferenceVerificationResponse = await _pexaService.Handle<LandTitleReferenceVerificationResponseType>(
                    new CheckLandTitleStatusQuery(request.LandTitleReferenceAndJurisdiction, request.AccessToken), request.AuthenticatedUser, cancellationToken);

            return landTitleReferenceVerificationResponse;
        }
    }
}
