using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Core.Features.Conveyancing.SettlementCalculator;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.Conveyancing.SettlementCalculator
{
    public class ActionstepSavePDF
    {
        public class ActionstepSavePDFCommand : IAuthenticatedCommand<ActionstepDocument>
        {
            public WCAUser AuthenticatedUser { get; set; }
            public string OrgKey { get; set; }
            public int MatterId { get; set; }
            public string FilePath { get; set; }
        }

        public class ValidatorCollection : AbstractValidator<ActionstepSavePDFCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.OrgKey).NotNull().MinimumLength(1);
                RuleFor(c => c.MatterId).GreaterThan(0);
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.FilePath).NotNull();
            }
        }

        public class Handler : IRequestHandler<ActionstepSavePDFCommand, ActionstepDocument>
        {
            private readonly ValidatorCollection _validator;
            private readonly IActionstepService _actionstepService;

            public Handler(
                ValidatorCollection validator,
                IActionstepService actionstepService)
            {
                _validator = validator;
                _actionstepService = actionstepService;
            }

            public async Task<ActionstepDocument> Handle(ActionstepSavePDFCommand message, CancellationToken token)
            {
                if (message is null) throw new ArgumentNullException(nameof(message));

                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid) throw new ValidationException("Invalid input.", result.Errors);

                TokenSetQuery tokenSetQuery = new TokenSetQuery(message.AuthenticatedUser?.Id, message.OrgKey);

                var actionResponse = await _actionstepService.Handle<GetActionResponse>(new GetActionRequest(tokenSetQuery, message.MatterId));

                string fileName = $"{message.MatterId}_Settlement Statement_{DateTime.UtcNow.ToString("_yyyy-MM-dd hh-mm", CultureInfo.InvariantCulture)}.pdf";

                UploadFileResponse file = await _actionstepService.UploadFile(tokenSetQuery, fileName, message.FilePath);

                #region Check Documents Folder
                ActionFolder actionFolder = new ActionFolder(actionResponse.Action.Id);
                GetActionFolderRequest folderRequest = new GetActionFolderRequest(tokenSetQuery, actionFolder);
                ListActionFolderResponse folderResponse = await _actionstepService.Handle<ListActionFolderResponse>(folderRequest);
                var parentFolderId = folderResponse.ActionFolders.Where(af => af.Name == "Documents").Select(af => af.Id).FirstOrDefault();
                #endregion

                ActionDocument document = new ActionDocument(actionResponse.Action.Id, fileName, file, parentFolderId);
                SaveActionDocumentRequest saveRequest = new SaveActionDocumentRequest(tokenSetQuery, document);

                var saveResponse = await _actionstepService.Handle<SaveActionDocumentResponse>(saveRequest);
                var fileUrl = new Uri(saveResponse.ActionDocument.SharepointUrl);

                string documentUrl = $"https://{fileUrl.Host}/mym/asfw/workflow/documents/views/action_id/{actionResponse.Action.Id}#mode=browse&view=list&folder={parentFolderId}&drive=DL";

                return new ActionstepDocument(fileUrl, fileName, new Uri(documentUrl));
            }
        }
    }
}
