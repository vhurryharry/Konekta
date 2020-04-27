using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using WCA.Domain.Models.Account;

namespace WCA.Web.Pages
{
    public abstract class KonektaPage : PageModel
    {
        private IMediator _mediator;
        private UserManager<WCAUser> _userManager;
        private WCAUser _wcaUser;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected UserManager<WCAUser> UserManager => _userManager ??= HttpContext.RequestServices.GetService<UserManager<WCAUser>>();

        protected WCAUser WCAUser => _wcaUser ??= UserManager.GetUserAsync(User).GetAwaiter().GetResult();

    }
}
