using Newtonsoft.Json;
using NodaTime;
using System;
using System.Globalization;
using WCA.Actionstep.Client.Converters;
using WCA.Actionstep.Client.Resources.Responses;

namespace WCA.Actionstep.Client.Resources
{
    public class ActionDocument
    {
        public ActionDocument()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="fileNameWithExtension"></param>
        /// <param name="uploadFileResponse"></param>
        /// <param name="parentFolderId">May be <see langword="null"/> or empty, in which case the document will be stored at the root of the matter.</param>
        public ActionDocument(int actionId, string fileNameWithExtension, UploadFileResponse uploadFileResponse, string parentFolderId)
        {
            if (string.IsNullOrEmpty(fileNameWithExtension)) throw new ArgumentException(Helper.NullOrEmptyParameterString, nameof(fileNameWithExtension));
            if (uploadFileResponse is null) throw new ArgumentNullException(nameof(uploadFileResponse));

            Id = actionId;
            Name = fileNameWithExtension;
            File = $"{uploadFileResponse.File.Id};{fileNameWithExtension}";
            Links = new Link()
            {
                Action = actionId.ToString(CultureInfo.InvariantCulture),
                Folder = parentFolderId
            };
        }

        [JsonConverter(typeof(IntStringConverter))]
        public int Id { get; set; }

        public string Name{ get; set; }
        public OffsetDateTime? ModifiedTimestamp { get; set; }
        public string Status { get; set; }
        public string Keywords { get; set; }
        public string Summary { get; set; }
        public OffsetDateTime? CheckedOutTimestamp { get; set; }
        public string CheckedOutTo { get; set; }
        public OffsetDateTime? CheckedOutEtaTimestamp { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool? IsDeleted { get; set; }
#pragma warning disable CA1056 // Uri properties should not be strings: Actionstep DTO
        public string SharepointUrl { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings
        public string File { get; set; }

        public Link Links { get; set; } = new Link();

        public class Link
        {
            public string Action { get; set; }
            public string CheckedOutBy { get; set; }
            public string Folder { get; set; }
            public string CreatedBy { get; set; }
            public string Tag { get; set; }
            public string DocumentTemplate { get; set; }
        }
    }
}
