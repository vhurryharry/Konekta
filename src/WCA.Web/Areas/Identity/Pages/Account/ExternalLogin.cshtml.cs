using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using WCA.Domain.Models.Account;

namespace WCA.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<WCAUser> _signInManager;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly UserManager<WCAUser> _userManager;

        public ExternalLoginModel(
            SignInManager<WCAUser> signInManager,
            ILogger<ExternalLoginModel> logger,
            UserManager<WCAUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        [TempData]
        public string ErrorMessage { get; set; }

#pragma warning disable CA1056 // Uri properties should not be strings
        public string ReturnUrl { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings

        public string AppName { get; set; }

#pragma warning disable CA1054 // Uri parameters should not be strings
        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            bool isWCA = Request.Host.Value.Contains("workcloud", System.StringComparison.OrdinalIgnoreCase)
                || Request.Host.Value.Contains("appwca-test", System.StringComparison.OrdinalIgnoreCase);

            AppName = isWCA ? "WorkCloud" : "Konekta";
        }

        public IActionResult OnPost(string provider, string returnUrl = null, string actionstepOrg = null)
#pragma warning restore CA1054 // Uri parameters should not be strings
        {
            returnUrl = returnUrl ?? GetDefaultRedirect();

            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            // When logging in from a url with an "actionstepOrg" parameter, include that in the authentication properties
            // so that it can be passed through OIDC
            if (!string.IsNullOrEmpty(actionstepOrg))
            {
                properties.Items.Add("orgkey", actionstepOrg);
            }

            return Challenge(properties, provider);
        }

#pragma warning disable CA1054 // Uri parameters should not be strings
        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
#pragma warning restore CA1054 // Uri parameters should not be strings
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = $"Error loading external login information";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return Redirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                ErrorMessage = $"This account has been locked out. Please try again later.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                //see if this is a known user
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // If the user does not have an account we'll create one now.
                    user = new WCAUser { UserName = email, Email = email };

                    var createUserResult = await _userManager.CreateAsync(user);
                    if (!createUserResult.Succeeded)
                    {
                        ErrorMessage = $"It looks like this is the first time you're logging in to " + AppName + ". " +
                            $"We're sorry, something went wrong registering you with the " + AppName + " system. " +
                            $"Please try again, or contact " + AppName + " support if you continue to have issues.";
                        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                    }
                }

                // Register the external login for this user
                user.FirstName = firstName;
                user.LastName = lastName;
                var addNewLoginResult = await _userManager.AddLoginAsync(user, info);
                if (addNewLoginResult.Succeeded)
                {
                    var saveResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
                    if (saveResult.Succeeded)
                    {
                        // Now sign in the user
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        ErrorMessage = $"It looks like this is the first time you're logging in to " + AppName + ". " +
                            $"We're sorry, something went wrong registering you with the " + AppName + " system. " +
                            $"Please try again, or contact " + AppName + " support if you continue to have issues.";
                        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                    }
                }

                return Redirect(returnUrl);
            }
        }

        private string GetDefaultRedirect()
        {
            return Url.Content("~/wca/");
        }
    }
}
