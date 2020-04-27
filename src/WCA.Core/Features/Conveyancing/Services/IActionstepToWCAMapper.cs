using WCA.Actionstep.Client.Resources.Responses;
using WCA.Domain.Conveyancing;

namespace WCA.Core.Features.Conveyancing.Services
{
    public interface IActionstepToWCAMapper
    {
        ConveyancingMatter MapFromActionstepTypes(
            GetActionResponse actionResponse,
            ListActionParticipantsResponse participantsResponse,
            ListDataCollectionRecordValuesResponse dataCollectionsResponse);
    }
}
