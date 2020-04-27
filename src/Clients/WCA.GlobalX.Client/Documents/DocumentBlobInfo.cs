using System;
using System.IO;

namespace WCA.GlobalX.Client.Documents
{
    public class DocumentBlobInfo : IDisposable
    {
        public string FullLocalPath { get; }
        public string FileName { get; }
        public string MimeType { get; }
        public Stream DocumentBlob { get; private set; }

        public DocumentBlobInfo(
            string fullLocalPath,
            string fileName,
            string mimeType,
            Stream documentBlob)
        {
            FullLocalPath = fullLocalPath;
            FileName = fileName;
            MimeType = mimeType;
            DocumentBlob = documentBlob;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!(DocumentBlob is null))
                    {
                        DocumentBlob.Dispose();
                    }

                    DocumentBlob = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}