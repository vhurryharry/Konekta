using FluentValidation;
using MediatR;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Domain.CQRS;
using WCA.GlobalX.Client;
using WCA.GlobalX.Client.Documents;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class CopyDocumentVersionToActionstepCommand : ICommand<DocumentRelationship>
    {
        /// <summary>
        /// The user whose credentials to use when retrieving the document blob from GlobalX.
        /// </summary>
        public string GlobalXUserId { get; set; }

        /// <summary>
        /// The user whose credentials to use when uploading to Actionstep.
        /// </summary>
        public string ActionstepUserId { get; set; }
        public Document Document { get; set; }
        public DocumentVersion DocumentVersion { get; set; }
        public string ActionstepOrgKey { get; set; }
        public int ActionstepMatterId { get; set; }
        public int MinimumMatterIdToSync { get; set; }

        public CopyDocumentVersionToActionstepCommand()
        { }

        public class Validator : AbstractValidator<CopyDocumentVersionToActionstepCommand>
        {
            public Validator()
            {
                RuleFor(d => d.GlobalXUserId).NotEmpty();

                RuleFor(d => d.Document).NotNull();

                RuleFor(d => d.DocumentVersion).NotNull();
                RuleFor(d => d.DocumentVersion.DocumentId).NotNull();
                RuleFor(d => d.DocumentVersion.DocumentVersionId).NotNull();
                RuleFor(d => d.DocumentVersion.DocumentName).NotEmpty();
                RuleFor(d => d.DocumentVersion.MimeType).NotEmpty();

                RuleFor(d => d.ActionstepOrgKey).NotEmpty();
            }
        }

        public class CopyDocumentVersionToActionstepCommandHandler : IRequestHandler<CopyDocumentVersionToActionstepCommand, DocumentRelationship>
        {
            private const string ActionstepFolderSecondPreference = "Documents";
            private const string ActionstepFolderFirstPreference = "_SEARCHES";
            private readonly IGlobalXService _globalXService;
            private readonly IActionstepService _actionstepService;
            private readonly Validator _validator;

            public CopyDocumentVersionToActionstepCommandHandler(
                IGlobalXService globalXService,
                IActionstepService actionstepService,
                Validator validator)
            {
                _globalXService = globalXService;
                _actionstepService = actionstepService;
                _validator = validator;
            }

            public async Task<DocumentRelationship> Handle(CopyDocumentVersionToActionstepCommand request, CancellationToken cancellationToken)
            {
                if (request is null) throw new System.ArgumentNullException(nameof(request));
                _validator.ValidateAndThrow(request);

                var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                DocumentFileInfo globalXDocumentFileInfo;

                var tokenSetQuery = new TokenSetQuery(request.ActionstepUserId, request.ActionstepOrgKey);

                // Check to make sure this matter exists first. This will throw if it's not found.
                var matterInfo = await _actionstepService.Handle<GetActionResponse>(new GetActionRequest(tokenSetQuery, request.ActionstepMatterId));
                if (matterInfo is null)
                {
                    throw new InvalidActionstepMatterException($"Matter '{request.ActionstepMatterId}' was not found in Actionstep.");
                }

                try
                {
                    try
                    {
                        globalXDocumentFileInfo = await _globalXService.DownloadDocument(
                            request.DocumentVersion.DocumentId.Value,
                            request.DocumentVersion.DocumentVersionId.Value,
                            tempFilePath,
                            request.GlobalXUserId);
                    }
                    catch (Exception ex)
                    {
                        throw new FailedToDownloadGlobalXDocumentException(ex);
                    }

                    try
                    {
                        var fileName = !string.IsNullOrEmpty(globalXDocumentFileInfo?.FileName)
                            ? globalXDocumentFileInfo.FileName
                            : request.DocumentVersion.DocumentName;

                        var fileUploadResponse = await _actionstepService.UploadFile(tokenSetQuery, fileName, tempFilePath);

                        if (fileUploadResponse is null)
                        {
                            throw new Exception("Unknown error uploading document to Actionstep.");
                        }

                        // Get all folders for matter and check to see if the specified folder exists. If it does, we need its ID.
                        var actionFolder = new ActionFolder(request.ActionstepMatterId);
                        var getFolderRequest = new GetActionFolderRequest(tokenSetQuery, actionFolder);
                        var folderResponse = await _actionstepService.Handle<ListActionFolderResponse>(getFolderRequest);

                        /// Will be null if the folder name wasn't found. In which case the document will be saved at the root of the matter.
                        var parentFolderId = folderResponse?.ActionFolders?.FirstOrDefault(af => af.Name == ActionstepFolderFirstPreference)?.Id ??
                            folderResponse?.ActionFolders?.FirstOrDefault(af => af.Name == ActionstepFolderSecondPreference)?.Id;

                        /// <see cref="ActionstepDocument"/> represents the object in "Matter Documents", as opposed to the file content from above (which is just in a big bucket).
                        var document = new ActionDocument(request.ActionstepMatterId, fileName, fileUploadResponse, parentFolderId);
                        var saveActionDocumentRequest = new SaveActionDocumentRequest(tokenSetQuery, document);

                        var saveActionDocumentResponse = await _actionstepService.Handle<SaveActionDocumentResponse>(saveActionDocumentRequest);

                        return new DocumentRelationship(
                            request.DocumentVersion.DocumentId.Value,
                            request.DocumentVersion.DocumentVersionId.Value,
                            fileName,
                            request.DocumentVersion.MimeType,
                            request.ActionstepOrgKey,
                            request.ActionstepMatterId,
                            saveActionDocumentResponse.ActionDocument.Id,
                            new Uri(saveActionDocumentResponse.ActionDocument.SharepointUrl));
                    }
                    catch (InvalidTokenSetException ex)
                    {
                        throw new FailedToUploadGlobalXDocumentToActionstepException(
                            $"Invalid Actionstep Token, unable to upload document. Token revoked at '{ex.TokenSet?.RevokedAt}', Token ID '{ex.TokenSet?.Id}'.",
                            ex);
                    }
                    catch (Exception ex)
                    {
                        throw new FailedToUploadGlobalXDocumentToActionstepException(ex.Message, ex);
                    }
                }
                finally
                {
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                }
            }
        }
    }
}