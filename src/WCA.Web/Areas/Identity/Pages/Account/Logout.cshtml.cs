using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WCA.Domain.Models.Account;

namespace WCA.Web.Areas.Identity.Pages.Account
{
    // Anyone can get here. Ignore antiforgery as it's low risk
    // (they're just signing out), plus we sign them out on a
    // GET anyway for convenience.
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<WCAUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<WCAUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

#pragma warning disable CA1056 // Uri properties should not be strings
        public string ReturnUrl { get; set; }

#pragma warning disable CA1054 // Uri parameters should not be strings
        public async Task<IActionResult> OnGet(string returnUrl = null)
            => await Signout(returnUrl);

        public async Task<IActionResult> OnPost(string returnUrl = null)
            => await Signout(returnUrl);
#pragma warning restore CA1054 // Uri parameters should not be strings

        private async Task<IActionResult> Signout(string returnUrl = null)
#pragma warning restore CA1054 // Uri parameters should not be strings
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}