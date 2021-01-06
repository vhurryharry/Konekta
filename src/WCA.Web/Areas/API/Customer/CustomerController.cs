using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WCA.Core.Features;

namespace WCA.Web.Areas.API.Customer
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private AppSettings appSettings;
        private readonly IMediator mediator;
        private ILogger<CustomerController> logger;

        public CustomerController(IOptions<AppSettings> appSettings,
            IMediator mediator,
            ILogger<CustomerController> logger)
        {
            if (appSettings is null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            this.appSettings = appSettings.Value;
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpPost()]
        [ValidateModelFilter]
        public async Task<IActionResult> Post([FromBody]NewCustomerViewModel newCustomerData)
        {
            if (!(newCustomerData is null))
            {
                try
                {
                    var newCustomerCreated = await mediator.Send(new NewCustomerCommand
                    {
                        Firstname = newCustomerData.Firstname,
                        Lastname = newCustomerData.Lastname,
                        Company = newCustomerData.Company,
                        ConveyancingApp = newCustomerData.ConveyancingApp,
                        ABN = newCustomerData.ABN,
                        Email = newCustomerData.Email,
                        Phone = newCustomerData.Phone,
                        PaymentGatewayToken = newCustomerData.PaymentGatewayToken,
                        StripeApiSecret = appSettings.WCACoreSettings.StripeSettings.ApiSecret,
                        Address1 = newCustomerData.Address1,
                        Address2 = newCustomerData.Address2,
                        City = newCustomerData.City,
                        State = newCustomerData.State,
                        Postcode = newCustomerData.Postcode,
                        PromoCode = newCustomerData.PromoCode,
                        OrgKey = newCustomerData.OrgKey,
                        AcceptedTermsAndConditions = newCustomerData.acceptedTermsAndConditions,
                        SubmittedDateTimeUtc = DateTime.UtcNow
                    });

                    return new OkObjectResult(newCustomerCreated);
                }
#pragma warning disable CA1031 // Do not catch general exception types: ex will be logged and an error returned
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    logger.LogError("Error creating customer", ex);

                }
            }

            bool isWCA = Request.Host.Value.Contains("workcloud", System.StringComparison.OrdinalIgnoreCase)
                || Request.Host.Value.Contains("appwca-test", System.StringComparison.OrdinalIgnoreCase);
            string domainUrl = isWCA ? appSettings.WCACoreSettings.AppUrlSettings.WorkCloud.DomainUrl : appSettings.WCACoreSettings.AppUrlSettings.Konekta.DomainUrl;

            return new BadRequestObjectResult(
                new ErrorViewModel("Sorry, there was a problem signing up. Please try again, " +
                "or let us know at support@" + domainUrl + " if you continue to have trouble."));
        }
    }

    public class CustomerCreated
    {
        public string ActionstepInstallLink { get; set; }
    }
}