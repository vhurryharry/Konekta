using System.Net.Http;

namespace WCA.PEXA.Client.Resources
{
    public abstract class PEXARequestBase
    {
        public string BearerToken { get; set; }
        public abstract string Path { get; }
        public abstract HttpMethod HttpMethod { get; }
        public virtual HttpContent Content { get; } = null;
        public abstract int Version { get; }
    }
}
