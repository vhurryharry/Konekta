using System.Net.Http;

namespace WCA.Core.Extensions
{
    public static class HttpMessageHandlerExtensions
    {
        public static DelegatingHandler DecorateWith(this HttpMessageHandler httpMessageHandler, DelegatingHandler delegatingHandler)
        {
            delegatingHandler.InnerHandler = httpMessageHandler;
            return delegatingHandler;
        }
    }
}
