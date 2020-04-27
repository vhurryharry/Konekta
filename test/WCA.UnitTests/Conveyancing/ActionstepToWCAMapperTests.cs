using System;
using System.Collections.Generic;
using System.Text;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Core.Features.Conveyancing.Services;
using Xunit;

namespace WCA.UnitTests.Conveyancing
{
    public class ActionstepToWCAMapperTests
    {
        [Fact]
        void ShouldMapFromValidActionstepTypes()
        {
            // Given
            var actionstepToWCAMapper = new ActionstepToWCAMapper();
            var actionResponse = new GetActionResponse
            {
                Action = new ActionstepAction
                {
                    Id = 123,
                    Name = "Test matter name"
                }
            };

            var participantsResponse = new ListActionParticipantsResponse();
            participantsResponse.ActionParticipants.AddRange(new List<ActionParticipant>
            {
                new ActionParticipant
                {
                    Id = "aabb",
                },
                new ActionParticipant
                {
                    Id = "ccdd",
                }
            });

            var dataCollectionsResponse = new ListDataCollectionRecordValuesResponse
            {
                DataCollectionRecordValues = new List<DataCollectionRecordValue>
                {
                    new DataCollectionRecordValue
                    {
                        Id = "dc-id1",
                        StringValue = "dc-val1"
                    }
                }
            };
            

            // When
            var wcaMatter = actionstepToWCAMapper.MapFromActionstepTypes(actionResponse, participantsResponse, dataCollectionsResponse);

            // Then
            Assert.Equal(actionResponse.Action.Id, wcaMatter.Id);
        }
    }
}
