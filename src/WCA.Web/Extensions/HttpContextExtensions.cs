using Microsoft.AspNetCore.Http;

namespace WCA.Web.Extensions
{
    public static class HttpContextExtensions
    {
        private const string OrgKeyContextKey = "CurrentOrgKey";

        public static void SetCurrentOrgKey(this HttpContext httpContext, string currentOrgKey)
        {
            if (httpContext is null || string.IsNullOrEmpty(currentOrgKey))
            {
                return;
            }

            httpContext.Items.Add(OrgKeyContextKey, currentOrgKey);
        }

        public static string GetCurrentOrgKey(this HttpContext httpContext)
        {
            if (!(httpContext is null))
            {
                if (httpContext.Items.ContainsKey(OrgKeyContextKey))
                {
                    if (httpContext.Items.TryGetValue(OrgKeyContextKey, out object currentOrgKey))
                    {
                        return (string)currentOrgKey;
                    }
                }
            }

            return null;
        }
    }
}
