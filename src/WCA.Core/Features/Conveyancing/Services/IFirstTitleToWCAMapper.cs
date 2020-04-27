using System.Threading.Tasks;
using WCA.Core.Features.Conveyancing.PolicyRequest;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.Conveyancing.Services
{
    public interface IFirstTitleToWCAMapper
    {
        Task<SendFirstTitlePolicyRequestResponse> MapFromFirstTitleResponse(TitleInsuranceResponse titleInsuranceResponse);
    }
}