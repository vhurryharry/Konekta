using System;

namespace WCA.Core.Features.Conveyancing.SettlementCalculator
{
    public class ActionstepDocument
    {
        public ActionstepDocument()
        {
        }

        public ActionstepDocument(Uri url, string fileName, Uri documentUrl)
        {
            Url = url;
            FileName = fileName;
            DocumentUrl = documentUrl;
        }

        public string FileName { get; set; }
        public Uri Url { get; set; }
        public Uri DocumentUrl { get; set; }
    }
}
