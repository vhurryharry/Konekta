using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;

namespace WCA.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
#pragma warning disable CA1056 // Uri properties should not be strings
        public string ReturnUrl { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings

        public string AppName { get; set; }
        public string AppUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        private AppSettings _appSettings;

        public LoginModel(IOptions<AppSettings> appSettings)
        {
            if (appSettings is null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            _appSettings = appSettings.Value;
        }

#pragma warning disable CA1054 // Uri parameters should not be strings
        public void OnGet(string returnUrl = null)
#pragma warning restore CA1054 // Uri parameters should not be strings
        {
            ReturnUrl = returnUrl;

            bool isWCA = Request.Host.Value.Contains("workcloud", System.StringComparison.OrdinalIgnoreCase)
                || Request.Host.Value.Contains("appwca-test", System.StringComparison.OrdinalIgnoreCase);

            AppName = isWCA ? "WorkCloud" : "Konekta";
            AppUrl = isWCA ? _appSettings.WCACoreSettings.AppUrlSettings.WorkCloud.AppUrl : _appSettings.WCACoreSettings.AppUrlSettings.Konekta.AppUrl;
        }
    }
}
