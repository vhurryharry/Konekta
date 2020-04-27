using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Core.Features.Actionstep;
using WCA.Core.Services;
using WCA.Data;
using WCA.Domain.Models;

namespace WCA.Core.Features
{
    public class NewCustomerCommandHandler : IRequestHandler<NewCustomerCommand, NewCustomerCreated>
    {
        private readonly IOptions<WCACoreSettings> appSettings;
        private readonly IMediator mediator;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly WCADbContext wCADbContext;
        private readonly ITelemetryLogger telemetryLogger;
        private readonly IActionstepService actionstepService;

        public NewCustomerCommandHandler(
            IOptions<WCACoreSettings> appSettings,
            IMediator mediator,
            IHttpClientFactory httpClientFactory,
            WCADbContext wCADbContext,
            ITelemetryLogger telemetryLogger,
            IActionstepService actionstepService)
        {
            this.appSettings = appSettings;
            this.mediator = mediator;
            this.httpClientFactory = httpClientFactory;
            this.wCADbContext = wCADbContext;
            this.telemetryLogger = telemetryLogger;
            this.actionstepService = actionstepService;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        async Task<NewCustomerCreated> IRequestHandler<NewCustomerCommand, NewCustomerCreated>.Handle(NewCustomerCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (!request.AcceptedTermsAndConditions)
                throw new ArgumentException("Terms and Conditions were not accepted.");

            NewCustomerCreated newCustomerCreated;
            if (request.ConveyancingApp.Equals("NSW", StringComparison.InvariantCultureIgnoreCase))
            {
                newCustomerCreated = new NewCustomerCreated("https://go.actionstep.com/frontend/billing/system/install-workflow-template?template_identifier=f19634cc10b8b2aa287aaf30f2a777af17ea4654");
            }
            else if (request.ConveyancingApp.Equals("VIC", StringComparison.InvariantCultureIgnoreCase))
            {
                newCustomerCreated = new NewCustomerCreated("https://go.actionstep.com/frontend/billing/system/install-workflow-template?template_identifier=bb4081087eb213e4943569e5bc1a2f92c80b8e2c");
            }
            else if (request.ConveyancingApp.Equals("QLD", StringComparison.InvariantCultureIgnoreCase))
            {
                newCustomerCreated = new NewCustomerCreated("https://go.actionstep.com/frontend/billing/system/install-workflow-template?template_identifier=0c06b95c570199e88269034aff296e7672e3c71c");
            }
            else
            {
                throw new ArgumentOutOfRangeException("ConveyancingApp", "Invalid Conveyancing App value was specified. Only the following values are allowed: NSW, VIC, or QLD.");
            }

            var supportPlanOption = "No";
            if (!string.IsNullOrEmpty(request.OrgKey))
            {
                // If not trial, then all new customers will have support enabled.
                supportPlanOption = request.OrgKey.ToLower().StartsWith("trial") ? "Trial" : "Yes";
            }

            var customerService = new StripeCustomerService(request.StripeApiSecret);
            var newCustomer = new StripeCustomerCreateOptions();
            newCustomer.SourceToken = request.PaymentGatewayToken;
            newCustomer.Email = request.Email;
            newCustomer.Description = request.Company;
            newCustomer.Metadata = new Dictionary<string, string>
            {
                ["Phone"] = request.Phone,
                ["Email"] = request.Email,
                ["Company"] = request.Company,
                ["ABN"] = request.ABN,
                ["ConveyancingApp"] = request.ConveyancingApp,
                ["PromoCode"] = request.PromoCode,
                ["OrgKey"] = request.OrgKey,
                ["SupportPlanOption"] = supportPlanOption,
            };

            StripeCustomer stripeCustomer = customerService.Create(newCustomer);

            // Save to database
            // TODO: Use Automapper
            wCADbContext.Add(new ConveyancingSignupSubmission
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                ABN = request.ABN,
                AcceptedTermsAndConditions = request.AcceptedTermsAndConditions,
                SupportPlanOption = supportPlanOption,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                Company = request.Company,
                ConveyancingApp = request.ConveyancingApp,
                Email = request.Email,
                Phone = request.Phone,
                Postcode = request.Postcode,
                PromoCode = request.PromoCode,
                OrgKey = request.OrgKey,
                State = request.State,
                SubmittedDateTimeUtc = request.SubmittedDateTimeUtc
            });

            var saveTask = wCADbContext.SaveChangesAsync();

            // Send customer data to Actionstep admin org as customer record
            using (var actionstepSignupFormData = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "organization_key", "btrcloud" },
                { "uid", "b039ac3664e3b2d0765384107f7a083754c70928" },
                { "p28__first_name", request.Firstname },
                { "p28__last_name", request.Lastname },
                { "p27__company_name", request.Company },
                { "p27__phone1_description", "Business" }, // Business, Home, Mobile, Direct Dial, Home Fax, Assistant, Business 2, Home 2, Mobile 2, Other
                { "p27__phone1_area_code", "" },
                { "p27__phone1", request.Phone },
                { "p27__e_mail", request.Email },
                { "dc17__Product", TranslateConveyancingAppName(request.ConveyancingApp) },
                { "dc16__MatterVolume", "" },
                { "p27__mailing_address_line_1", request.Address1 },
                { "p28__mailing_address_line_2", request.Address2 },
                { "p28__mailing_city", request.City },
                { "p27__mailing_state_province", request.State },
                { "p27__mailing_post_code", request.Postcode },
                { "p27__mailing_country_id", "AU" }, // For conveyancing currently all clients will be in Australia
                { "dc17__InstallDate", ActionstepUtilities.GetActionstepFormattedStringValue(request.SubmittedDateTimeUtc.ToLocalTime()) },
                { "dc16__PromoCode", request.PromoCode },
                { "dc17__ORGKEY", request.OrgKey },
                { "dc17__Support_Option", supportPlanOption },
                { "Submit", "Submit" },
            }))
            {

#pragma warning disable CA2000 // Dispose objects before losing scope. See https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
                var httpClient = httpClientFactory.CreateClient();
#pragma warning restore CA2000 // Dispose objects before losing scope

                var actionstepSignupFormTask = httpClient.PostAsync(
                    actionstepService.WebFormPostUri,
                    actionstepSignupFormData);

                Task.WaitAll(saveTask, actionstepSignupFormTask);

                // The response will be a redirect if successful, but we'll
                // also check for a SuccessStatusCode just in case
                if (!actionstepSignupFormTask.Result.IsSuccessStatusCode &&
                    actionstepSignupFormTask.Result.StatusCode != HttpStatusCode.Redirect)
                {
                    telemetryLogger.TrackTrace("Failed to submit Actionstep form for new customer",
                        WCASeverityLevel.Error,
                        new Dictionary<string, string>() {
                        { "New customer app requested", request.ConveyancingApp },
                        { "New customer email", request.Email },
                        { "New customer company", request.Company }
                    });
                }

                return newCustomerCreated;
            }
        }

        private string TranslateConveyancingAppName(string formAppName)
        {
            switch (formAppName)
            {
                case "QLD": return "QLD Conveyancing";
                case "NSW": return "NSW Conveyancing";
                case "VIC": return "VIC Conveyancing";
                default: return "Invalid";
            }
        }
    }
}
