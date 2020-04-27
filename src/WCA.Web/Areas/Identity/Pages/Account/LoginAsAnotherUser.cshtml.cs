using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WCA.Domain.Models.Account;

namespace WCA.Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "GlobalAdministrator")]
    public class LoginAsAnotherUserModel : PageModel
    {
        private readonly SignInManager<WCAUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<WCAUser> _userManager;

        public LoginAsAnotherUserModel(
            SignInManager<WCAUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<WCAUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

#pragma warning disable CA1056 // Uri properties should not be strings
        public string ReturnUrl { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(2048, MinimumLength = 3)]
            [EmailAddress]
            public string Email { get; set; }
        }

#pragma warning disable CA1054 // Uri parameters should not be strings
        public void OnGet(string returnUrl = null)
#pragma warning restore CA1054 // Uri parameters should not be strings
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? GetDefaultRedirect();

            ReturnUrl = returnUrl;
        }

#pragma warning disable CA1054 // Uri parameters should not be strings
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
#pragma warning restore CA1054 // Uri parameters should not be strings
        {
            returnUrl = returnUrl ?? GetDefaultRedirect();
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var currentlySignedInUser = await _userManager.GetUserAsync(User);
                var userToSignInAs = await _userManager.FindByEmailAsync(Input.Email);

                if (userToSignInAs == null)
                {
                    ModelState.AddModelError("InvalidEmail", "No account found with that email address.");
                    return Page();
                }

                await _signInManager.SignInAsync(userToSignInAs, false, "LoginAsAnotherUser");

                _logger.LogWarning(1, $"User '{currentlySignedInUser.Id}' successfully used LoginAsAnotherUser to log in as '{userToSignInAs.Id}'.");

                return LocalRedirect(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private string GetDefaultRedirect()
        {
            return Url.Content("~/wca/");
        }
    }
}
