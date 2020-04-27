using System.Net.Http;

namespace WCA.FirstTitle.Client.Resources
{
    public abstract class FirstTitleRequestBase
    {
        public abstract FirstTitleCredential FirstTitleCredential { get; set; }
        public abstract HttpMethod HttpMethod { get; }
        public abstract string SOAPAction { get; }
        public virtual string Content { get; } = null;
    }
}
