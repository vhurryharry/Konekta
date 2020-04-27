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
    [Authorize(Roles = "AllowedToHavePassword")]
    public class AddOrChangePasswordModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<WCAUser> _userManager;

        public AddOrChangePasswordModel(
            ILogger<LoginModel> logger,
            UserManager<WCAUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

#pragma warning disable CA1056 // Uri properties should not be strings
        public string ReturnUrl { get; set; }

        public string BackUrl { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password.")]
            [StringLength(2048, MinimumLength = 8, ErrorMessage = "The password length must be greater than 8 characters.")]
            public string Password { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please re-enter your password to confirm.")]
            [StringLength(2048, MinimumLength = 8, ErrorMessage = "The password length must be greater than 8 characters.")]
            [Compare("Password", ErrorMessage = "The passwords do not match.")]
            public string ConfirmPassword { get; set; }
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
            BackUrl = Url.Content("~/wca/");
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

                if (await _userManager.HasPasswordAsync(currentlySignedInUser))
                {
                    await _userManager.RemovePasswordAsync(currentlySignedInUser);
                }

                var result = await _userManager.AddPasswordAsync(currentlySignedInUser, Input.Password);

                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Error", error.Description);
                    }

                    return Page();
                }

                _logger.LogWarning(1, $"Password for user '{currentlySignedInUser.Id}' has been successfully set.");

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
