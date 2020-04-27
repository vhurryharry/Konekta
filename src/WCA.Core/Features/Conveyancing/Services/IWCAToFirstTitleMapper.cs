using WCA.Core.Features.Conveyancing.PolicyRequest;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.Conveyancing.Services
{
    public interface IWCAToFirstTitleMapper
    {
        TitleInsuranceRequest MapToFirstTitleInsuranceRequest(FTActionstepMatter source, RequestPolicyOptions options);
    }
}
