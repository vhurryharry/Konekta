using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Features.Pexa;
using WCA.PEXA.Client;
using WCA.PEXA.Client.Resources;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class GetPexaWorkspaceSummaryQueryHandler : IRequestHandler<GetPexaWorkspaceSummaryQuery, WorkspaceSummaryResponseType>
    {
        private readonly IExtendedPexaService _pexaService;
        private readonly GetPexaWorkspaceSummaryQuery.ValidatorCollection _validator;

        public GetPexaWorkspaceSummaryQueryHandler(
            IExtendedPexaService pexaService,
            GetPexaWorkspaceSummaryQuery.ValidatorCollection validator)
        {
            _pexaService = pexaService;
            _validator = validator;
        }

        public async Task<WorkspaceSummaryResponseType> Handle(GetPexaWorkspaceSummaryQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await _validator.ValidateAndThrowAsync(request);

            var workspaceSummary = await _pexaService.Handle<WorkspaceSummaryResponseType>(
                    new RetrieveWorkspaceSummaryRequestQuery(request.RetrieveWorkspaceSummaryParameters, request.AccessToken), request.AuthenticatedUser, cancellationToken);

            return workspaceSummary;
        }
    }
}
