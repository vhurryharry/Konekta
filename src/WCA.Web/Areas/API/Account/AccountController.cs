using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WCA.Core.Features.Account;
using WCA.Core.Services;
using WCA.Domain.Models.Account;

namespace WCA.Web.Areas.API.Account
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<WCAUser> _userManager;
        private readonly IMediator _mediator;
        private readonly ITelemetryLogger _telemetryLogger;

        public AccountController(
            UserManager<WCAUser> userManager,
            IMediator mediator,
            ITelemetryLogger telemetryLogger)
        {
            _userManager = userManager;
            _mediator = mediator;
            _telemetryLogger = telemetryLogger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<AccountModel> CurrentUser()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return new AccountModel()
                {
                    IsLoggedIn = false
                };
            }
            else
            {
                return new AccountModel()
                {
                    IsLoggedIn = true,
                    Email = currentUser.Email,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Roles = (await _userManager.GetRolesAsync(currentUser))
                };
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AuthFailed(string encodedJwt)
        {
            _telemetryLogger.TrackTrace("Authentication failed for several times.", WCASeverityLevel.Verbose,
                new Dictionary<string, string> {
                    { "EncodedJwt", encodedJwt }
                });

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddCreditCard([FromBody]AddCreditCard.AddCreditCardCommand  addCreditCardCommand)
        {
            if (addCreditCardCommand is null)
            {
                throw new System.ArgumentNullException(nameof(addCreditCardCommand));
            }

            var currentUser = await _userManager.GetUserAsync(User);

            addCreditCardCommand.AuthenticatedUser = currentUser;

            var result = await _mediator.Send(addCreditCardCommand);

            switch (result.Status)
            {
                case Core.Features.Account.AddCreditCard.AddCreditCardResult.AddCreditCardStatus.CreditCardSaved:
                    return new CreatedResult(string.Empty, null);
                default:
                    return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}