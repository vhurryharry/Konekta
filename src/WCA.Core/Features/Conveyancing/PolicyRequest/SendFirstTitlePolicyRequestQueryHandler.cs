using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Features.Conveyancing.Services;
using WCA.FirstTitle.Client;
using WCA.FirstTitle.Client.Resources;

namespace WCA.Core.Features.Conveyancing.PolicyRequest
{
    class SendFirstTitlePolicyRequestQueryHandler : IRequestHandler<SendFirstTitlePolicyRequestQuery, SendFirstTitlePolicyRequestResponse>
    {
        private readonly IFirstTitleToWCAMapper _firstTitleToWCAMapper;
        private readonly IFirstTitleClient _firstTitleClient;

        private readonly IWCAToFirstTitleMapper _wCAToFirstTitleMapper;

        public SendFirstTitlePolicyRequestQueryHandler(
            IWCAToFirstTitleMapper wCAToFirstTitleMapper,
            IFirstTitleToWCAMapper firstTitleToWCAMapper,
            IFirstTitleClient firstTitleClient)
        {
            _firstTitleToWCAMapper = firstTitleToWCAMapper;
            _wCAToFirstTitleMapper = wCAToFirstTitleMapper;
            _firstTitleClient = firstTitleClient;
        }

        public async Task<SendFirstTitlePolicyRequestResponse> Handle(SendFirstTitlePolicyRequestQuery request, CancellationToken cancellationToken)
        {
            var titleInsuranceRequest = _wCAToFirstTitleMapper.MapToFirstTitleInsuranceRequest(request.ActionstepMatter, request.RequestPolicyOptions);

            var titleInsuranceRequestCommand = new TitleInsuranceRequestCommand(titleInsuranceRequest, request.FirstTitleCredentials);

            // send request
            var titleInsuranceResponse = await _firstTitleClient.Handle<TitleInsuranceResponse>(titleInsuranceRequestCommand, cancellationToken);

            // map response to format expected by SPA frontend
            return await _firstTitleToWCAMapper.MapFromFirstTitleResponse(titleInsuranceResponse);
        }
    }
}
