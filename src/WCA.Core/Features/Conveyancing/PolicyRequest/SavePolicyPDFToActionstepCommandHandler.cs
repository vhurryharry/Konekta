using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;

namespace WCA.Core.Features.Conveyancing.PolicyRequest
{
    public class SavePolicyPDFToActionstepCommandHandler : IRequestHandler<SavePolicyPDFToActionstepCommand, FTAttachment>
    {
        private readonly IActionstepService _actionstepService;

        public SavePolicyPDFToActionstepCommandHandler(IActionstepService actionstepService)
        {
            _actionstepService = actionstepService;
        }

        public async Task<FTAttachment> Handle(SavePolicyPDFToActionstepCommand request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Get Actionstep matter info
            var tokenSetQuery = new TokenSetQuery(request.AuthenticatedUser?.Id, request.ActionstepOrg);
            
            var actionResponse = await _actionstepService.Handle<GetActionResponse>(new GetActionRequest(tokenSetQuery, request.MatterId));

            UploadFileResponse file = await _actionstepService.UploadFile(tokenSetQuery, request.FileName, request.FilePath);

            #region Check Documents Folder
            ActionFolder actionFolder = new ActionFolder(actionResponse.Action.Id);
            GetActionFolderRequest folderRequest = new GetActionFolderRequest(tokenSetQuery, actionFolder);
            ListActionFolderResponse folderResponse = await _actionstepService.Handle<ListActionFolderResponse>(folderRequest);
            var parentFolderId = folderResponse.ActionFolders.Where(af => af.Name == "Documents").Select(af => af.Id).FirstOrDefault();
            #endregion

            ActionDocument document = new ActionDocument(actionResponse.Action.Id, request.FileName, file, parentFolderId);
            SaveActionDocumentRequest saveRequest = new SaveActionDocumentRequest(tokenSetQuery, document);

            var saveResponse = await _actionstepService.Handle<SaveActionDocumentResponse>(saveRequest);
            var fileUrl = new Uri(saveResponse.ActionDocument.SharepointUrl);

            string documentUrl = $"https://{fileUrl.Host}/mym/asfw/workflow/documents/views/action_id/{actionResponse.Action.Id}#mode=browse&view=list&folder={parentFolderId}&drive=DL";

            return new FTAttachment()
            {
                FileName = request.FileName,
                FileUrl = documentUrl
            };
        }
    }
}
