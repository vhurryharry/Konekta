using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Data;
using WCA.Domain.Actionstep;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class StorePexaWorkspaceIdInActionstepMatterCommandHandler : IRequestHandler<StorePexaWorkspaceIdInActionstepMatterCommand, bool>
    {

        private readonly IActionstepService _actionstepService;
        private readonly WCADbContext _wCADbContext;

        public StorePexaWorkspaceIdInActionstepMatterCommandHandler(
            WCADbContext wCADbContext,
            IActionstepService actionstepService)
        {
            _actionstepService = actionstepService;
            _wCADbContext = wCADbContext;
        }

        public async Task<bool> Handle(StorePexaWorkspaceIdInActionstepMatterCommand command, CancellationToken cancellationToken)
        {
            if (command is null) throw new System.ArgumentNullException(nameof(command));

            var tokenSetQuery = new TokenSetQuery(command.AuthenticatedUser?.Id, command.ActionstepOrg);

            var actionResponse = await _actionstepService.Handle<GetActionResponse>(new GetActionRequest
            {
                TokenSetQuery = tokenSetQuery,
                ActionId = command.MatterId
            });
            
            var convdetDataCollectionFields = await _actionstepService.Handle<ListDataCollectionFieldResponse>(new ListDataCollectionFieldsRequest
            {
                DataCollectionNames = { "convdet" },
                TokenSetQuery = tokenSetQuery,
                ActionTypes = { actionResponse.Action.Links.ActionType}
            });

            if (!convdetDataCollectionFields.DataCollectionFields.Exists(f => f.Name == "pexa_workspace_id"))
            {
                var createDataCollectionFieldRequest = new CreateDataCollectionFieldRequest();
                var dataCollectionId = convdetDataCollectionFields.Linked.DataCollections.Single().Id;

                var dataCollectionField = new DataCollectionField
                {
                    Id = dataCollectionId + "--pexa_workspace_id",
                    Name = "pexa_workspace_id",
                    DataType = "String",
                    Label = "Pexa Workspace ID",
                    CustomHtmlAbove = "",
                    CustomHtmlBelow = "",
                    Description = "",
                    Links = new DataCollectionFieldLink
                    {
                        DataCollection = dataCollectionId.ToString(CultureInfo.InvariantCulture)
                    }
                };

                createDataCollectionFieldRequest.DataCollectionFields.Add(dataCollectionField);
                createDataCollectionFieldRequest.TokenSetQuery = tokenSetQuery;

                var listActionstepUsersRequest = new ListActionstepUsersRequest();
                listActionstepUsersRequest.TokenSetQuery = tokenSetQuery;

                // Add a filter to get only the authorized users
                listActionstepUsersRequest.Filter = "hasAuthority_eq=T";

                var listActionstepUsers = await _actionstepService.Handle<ListActionstepUsersResponse>(listActionstepUsersRequest);
                if(listActionstepUsers == null)
                {
                    throw new UnauthorizedAccessException("No ActionstepCredential available to create the field!");
                }
                var currentUser = listActionstepUsers.Users.FirstOrDefault(u => u.EmailAddress == command.AuthenticatedUser?.Email);

                if(currentUser == default(ActionstepUser))
                {
                    var actionstepCredential = _wCADbContext.ActionstepCredentials.FirstOrDefault(ac =>
                        ac.ActionstepOrg.Key == command.ActionstepOrg
                        && listActionstepUsers.Users.Any(au => au.EmailAddress == ac.Owner.Email)
                    );

                    if (actionstepCredential == default(ActionstepCredential))
                    {
                        throw new UnauthorizedAccessException("No ActionstepCredential available to create the field!");
                    }

                    var newTokenSetQuery = new TokenSetQuery(actionstepCredential.Owner.Id, command.ActionstepOrg);
                    createDataCollectionFieldRequest.TokenSetQuery = newTokenSetQuery;
                }

                await _actionstepService.Handle(createDataCollectionFieldRequest);
            }

            var dataCollectionRecordValuesResponse = await _actionstepService.Handle<ListDataCollectionRecordValuesResponse>(new ListDataCollectionRecordValuesRequest
            {
                ActionstepId = command.MatterId,
                TokenSetQuery = tokenSetQuery,
                DataCollectionRecordNames = { "convdet" },
                DataCollectionFieldNames = { "pexa_workspace_id" }
            });

            dataCollectionRecordValuesResponse["convdet", "pexa_workspace_id"] = command.WorkspaceId;

            var updateDataCollectionRecordValuesRequest = new UpdateDataCollectionRecordValuesRequest();
            updateDataCollectionRecordValuesRequest.TokenSetQuery = tokenSetQuery;

            foreach(var recordValue in dataCollectionRecordValuesResponse.DataCollectionRecordValues)
            {
                updateDataCollectionRecordValuesRequest.DataCollectionRecordValues.Add(recordValue);
            }

            await _actionstepService.Handle(updateDataCollectionRecordValuesRequest);

            return true;
        }
    }
}
