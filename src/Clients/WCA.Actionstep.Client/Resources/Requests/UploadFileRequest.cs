using System.Net.Http;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class UploadFileRequest : IActionstepRequest
    {
        public int PartCount { get; set; }
        public int PartNumber { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays: DTO
        public byte[] FileContent { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        public HttpMethod HttpMethod => HttpMethod.Post;
        public string RelativeResourcePath =>
            string.IsNullOrEmpty(FileId)
                ? $"rest/files?part_count={PartCount}&part_number={PartNumber}"
                : $"rest/files/{FileId}?part_count={PartCount}&part_number={PartNumber}";

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => null;
    }
}
