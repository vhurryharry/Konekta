using System.Net.Http;

namespace WCA.Core.Features.InfoTrack
{
    public class InfoTrackService
    {
        public HttpClient Client { get; private set; }

        public InfoTrackService(HttpClient httpClient)
        {
            Client = httpClient;
        }
    }
}
