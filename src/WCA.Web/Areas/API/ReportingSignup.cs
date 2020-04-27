using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WCA.Core.Features.ReportSync;
using WCA.Data;
using WCA.Domain.Models.Account;

namespace WCA.Web.Areas.API
{
    [Route("api/[controller]")]
    public class ReportingSignupController : Controller
    {
        private readonly UserManager<WCAUser> userManager;
        private readonly AppSettings appSettings;
        private readonly WCADbContext wCADbContext;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<ReportingSignupController> logger;

        public ReportingSignupController(IOptions<AppSettings> appSettings,
            UserManager<WCAUser> userManager,
            WCADbContext wCADbContext,
            IMediator mediator,
            IMapper mapper,
            ILogger<ReportingSignupController> logger)
        {
            if (appSettings is null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            this.appSettings = appSettings.Value;
            this.userManager = userManager;
            this.wCADbContext = wCADbContext;
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ReportSyncSignup.ReportSyncSignupCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            try
            {
                // var command = mapper.Map<ReportSyncSignup.Command>(newCustomerData);
                command.StripeApiSecret = appSettings.WCACoreSettings.StripeSettings.ApiSecret;
                command.AuthenticatedUser = await userManager.GetUserAsync(User);

                await mediator.Send(command);
                return new AcceptedResult();
            }
            catch (ValidationException vex)
            {
                var errorViewModel = new ErrorViewModel(vex);
                return new BadRequestObjectResult(errorViewModel);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                logger.LogError("Error signing up for reporting", ex);


                bool isWCA = Request.Host.Value.Contains("workcloud", System.StringComparison.OrdinalIgnoreCase)
                    || Request.Host.Value.Contains("appwca-test", System.StringComparison.OrdinalIgnoreCase);
                string domainUrl = isWCA ? appSettings.WCACoreSettings.AppUrlSettings.WorkCloud.DomainUrl : appSettings.WCACoreSettings.AppUrlSettings.Konekta.DomainUrl;

                return new BadRequestObjectResult(
                    new ErrorViewModel("Sorry, there was a problem signing up. Please try again, " +
                    "or let us know at support@" + domainUrl + " if you continue to have trouble."));
            }
        }
    }
}