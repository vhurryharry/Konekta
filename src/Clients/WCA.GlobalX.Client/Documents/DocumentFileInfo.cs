namespace WCA.GlobalX.Client.Documents
{
    public class DocumentFileInfo
    {
        public string FullLocalPath { get; }
        public string FileName { get; }
        public string MimeType { get; }

        public DocumentFileInfo(
            string fullLocalPath,
            string fileName,
            string mimeType)
        {
            FullLocalPath = fullLocalPath;
            FileName = fileName;
            MimeType = mimeType;
        }
    }
}