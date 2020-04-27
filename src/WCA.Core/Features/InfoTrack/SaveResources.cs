using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Domain.CQRS;
using WCA.Domain.InfoTrack;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.InfoTrack
{
    public class SaveResources
    {
        public class SaveResourcesCommand : IAuthenticatedCommand
        {
            public string ActionstepOrgKey { get; set; }
            public WCAUser AuthenticatedUser { get; set; }

            /// <summary>
            /// The <see cref="InfoTrackCredentials"/> to use to authenticate to InfoTrack when downloading resources.
            /// </summary>
            public InfoTrackCredentials InfoTrackCredentials { get; set; }
            public int MatterId { get; set; }
            public string ResourceURL { get; set; }

            public string FolderName { get; set; } = "Documents";
            public string FileNameWithoutExtensionIfNotAvailableFromHeader { get; set; }
            public string FileNameAddition { get; set; }
        }

        public class ValidatorCollection : AbstractValidator<SaveResourcesCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.ActionstepOrgKey).NotEmpty();
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.MatterId).GreaterThan(0);
                RuleFor(c => c.ResourceURL).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<SaveResourcesCommand>
        {
            private readonly ValidatorCollection _validator;
            private readonly IActionstepService _actionstepService;
            private readonly InfoTrackService _infoTrackService;

            public Handler(
                ValidatorCollection validator,
                IActionstepService actionstepService,
                InfoTrackService infoTrackService)
            {
                _validator = validator;
                _actionstepService = actionstepService;
                _infoTrackService = infoTrackService;
            }

            protected override async Task Handle(SaveResourcesCommand message, CancellationToken token)
            {
                if (message is null) throw new ArgumentNullException(nameof(message));

                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Unable to save Property Resources, the command message was invalid.", result.Errors);
                }

                using (var request = new HttpRequestMessage(HttpMethod.Get, message.ResourceURL))
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{message.InfoTrackCredentials.Username}:{message.InfoTrackCredentials.Password}");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    using (var infoTrackFileDownloadResponse = await _infoTrackService.Client.SendAsync(request))
                    {
                        infoTrackFileDownloadResponse.EnsureSuccessStatusCode();

                        using (var infoTrackFileStream = await infoTrackFileDownloadResponse.Content.ReadAsStreamAsync())
                        {
                            if (infoTrackFileStream.Length <= 0)
                            {
                                throw new HttpRequestException($"Unable to download the resource from InfoTrack.");
                            }
                            else
                            {
                                var tokenSetQuery = new TokenSetQuery(message.AuthenticatedUser?.Id, message.ActionstepOrgKey);

                                var fileNameWithExtension = DetermineBestFileNameWithExtension(message, infoTrackFileDownloadResponse);

                                var fileUploadResponse = await _actionstepService.UploadFile(tokenSetQuery, fileNameWithExtension, infoTrackFileStream);

                                if (fileUploadResponse == null)
                                {
                                    throw new Exception("Unknown error uploading document to Actionstep.");
                                }

                                // Get all folders for matter and check to see if the specified folder exists. If it does, we need its ID.
                                var actionFolder = new ActionFolder(message.MatterId);
                                var getFolderRequest = new GetActionFolderRequest(tokenSetQuery, actionFolder);
                                var folderResponse = await _actionstepService.Handle<ListActionFolderResponse>(getFolderRequest);

                                /// Will be null if <see cref="SaveResourcesCommand.FolderName"/> wasn't found. In which case the document will be saved at the root of the matter.
                                var parentFolderId = folderResponse.ActionFolders.FirstOrDefault(af => af.Name == message.FolderName)?.Id;

                                /// <see cref="ActionstepDocument"/> represents the object in "Matter Documents", as opposed to the file content from above (which is just in a big bucket).
                                var document = new ActionDocument(message.MatterId, fileNameWithExtension, fileUploadResponse, parentFolderId);

                                var saveRequest = new SaveActionDocumentRequest(tokenSetQuery, document);

                                await _actionstepService.Handle<SaveActionDocumentResponse>(saveRequest);
                            }
                        }
                    }
                }
            }

            private string DetermineBestFileNameWithExtension(SaveResourcesCommand message, HttpResponseMessage responseFileDownload)
            {
                var fileExtension = FileExtensionFromContentType(responseFileDownload.Content.Headers?.ContentType?.MediaType);

                // Always use the supplied file name if available
                if (!string.IsNullOrEmpty(message.FileNameWithoutExtensionIfNotAvailableFromHeader))
                {
                    var cleanFileName = CleanFileName(message.FileNameWithoutExtensionIfNotAvailableFromHeader);
                    if (!string.IsNullOrEmpty(cleanFileName))
                    {
                        if (!string.IsNullOrEmpty(message.FileNameAddition))
                        {
                            cleanFileName += message.FileNameAddition;
                        }

                        return $"{cleanFileName}.{fileExtension}";
                    }
                }

                // Fall back to the date
                return $"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss-ffff-UTC", CultureInfo.InvariantCulture)}.{fileExtension}";
            }

            private static string CleanFileName(string fileNameToClean)
            {
                if (string.IsNullOrEmpty(fileNameToClean))
                {
                    return fileNameToClean;
                }

                var cleanFileName = fileNameToClean;
                cleanFileName = string.Concat(cleanFileName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));

                if (cleanFileName.Length > 250)
                {
                    cleanFileName = cleanFileName.Substring(0, 250);
                }

                return cleanFileName;
            }

            private static string FileExtensionFromContentType(string mimeType)
            {
                // For more, we could use something like this list:
                // https://www.freeformatter.com/mime-types-list.html

                switch (mimeType)
                {
                    case "application/pdf": return "pdf";
                    case "application/msword": return "doc";
                    case "application/vnd.openxmlformats-officedocument.wordprocessingml.document": return "docx";
                    default:
                        // Fall back to PDF as it's the most likely format in this use case
                        return "pdf";
                }
            }
        }
    }
}
