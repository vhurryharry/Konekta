using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;

namespace WCA.Web.Pages
{
    [AllowAnonymous]
    public class ErrorModel : PageModel
    {
        public string SupportUrl { get; set; }

        private AppSettings _appSettings;

        public ErrorModel(IOptions<AppSettings> appSettings)
        {
            if (appSettings is null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            _appSettings = appSettings.Value;
        }

        public void OnGet()
        {
            bool isWCA = Request.Host.Value.Contains("workcloud", System.StringComparison.OrdinalIgnoreCase)
                || Request.Host.Value.Contains("appwca-test", System.StringComparison.OrdinalIgnoreCase);

            SupportUrl = isWCA ? _appSettings.WCACoreSettings.AppUrlSettings.WorkCloud.SupportUrl : _appSettings.WCACoreSettings.AppUrlSettings.Konekta.SupportUrl;
        }
    }
}
