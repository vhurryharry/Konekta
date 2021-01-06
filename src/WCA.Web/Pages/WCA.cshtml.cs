using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WCA.Web.Pages
{
    [AllowAnonymous]
    public class WCAModel : PageModel
    {
        public string AppName { get; set; }
        public string LoadingLogo { get; set; }

        public void OnGet()
        {
            bool isWCA = Request.Host.Value.Contains("workcloud", System.StringComparison.OrdinalIgnoreCase) 
                || Request.Host.Value.Contains("appwca-test", System.StringComparison.OrdinalIgnoreCase);

            AppName = isWCA ? "WorkCloud" : "Konekta";
            LoadingLogo = isWCA ? "/images/wca-spinner.svg" : "/images/Konekta_loading.svg";
        }
    }
}
