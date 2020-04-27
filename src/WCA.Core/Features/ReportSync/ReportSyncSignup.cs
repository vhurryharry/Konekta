using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Services.Email;
using WCA.Data;
using WCA.Domain.Actionstep;
using WCA.Domain.CQRS;
using WCA.Domain.Models;
using WCA.Domain.Models.Account;
using WCA.Domain.Validators;

namespace WCA.Core.Features.ReportSync
{
    public class ReportSyncSignup
    {
        public class ReportSyncSignupCommand : IAuthenticatedCommand
        {
            public ActionstepOrg ActionstepOrg { get; set; }
            public string StripeApiSecret { get; set; }
            public string PaymentGatewayToken { get; set; }
            public string ServiceContactFirstname { get; set; }
            public string ServiceContactLastname { get; set; }
            public string ServiceContactEmail { get; set; }
            public string ServiceContactPhone { get; set; }
            public string BillingContactFirstname { get; set; }
            public string BillingContactLastname { get; set; }
            public string BillingContactEmail { get; set; }
            public string BillingContactPhone { get; set; }
            public string Company { get; set; }
            public string BillingFrequency { get; set; }
            public string ABN { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Postcode { get; set; }
            public bool AcceptedTermsAndConditions { get; set; }
            public bool AcknowledgedFeesAndCharges { get; set; }

            /// <summary>
            /// IAuthenticatedCommand property
            /// </summary>
            public WCAUser AuthenticatedUser { get; set; }
        }

        public class Validator : AbstractValidator<ReportSyncSignupCommand>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotEmpty();
                RuleFor(c => c.PaymentGatewayToken).NotEmpty();
                RuleFor(c => c.StripeApiSecret).NotEmpty();
                RuleFor(c => c.Company).NotEmpty();

                var allowedBillingFrequencies = new List<string> {
                    "Monthly, at AU$125/month (inc. GST)",
                    "Quarterly, at AU$375/3 months (inc. GST)",
                    "Annually, at AU$1,500/year (inc. GST)"
                };

                // Must match one of the items in allowedBillingFrequencies
                RuleFor(c => c.BillingFrequency).Must(commandBillingFrequency =>
                    allowedBillingFrequencies.Any(testFrequency => commandBillingFrequency == testFrequency)
                ).WithMessage($"{{PropertyName}} must be one of the following: {string.Join(", ", allowedBillingFrequencies)}");


                // Contact information
                RuleFor(c => c.BillingContactFirstname).NotEmpty();
                RuleFor(c => c.BillingContactLastname).NotEmpty();
                RuleFor(c => c.BillingContactEmail).NotEmpty();
                RuleFor(c => c.ServiceContactFirstname).NotEmpty();
                RuleFor(c => c.ServiceContactLastname).NotEmpty();
                RuleFor(c => c.ServiceContactEmail).NotEmpty();

                var abnValidatorAttribute = new ABNAttribute();
                abnValidatorAttribute.AllowNullOrEmpty = true;
                RuleFor(c => c.ABN)
                    .Must(abn => abnValidatorAttribute.IsValid(abn))
                    .WithMessage($"{{PropertyName}} must be a valid Australian Busines Number.");

                RuleFor(c => c.AcceptedTermsAndConditions)
                    .Equal(true)
                    .WithMessage("Terms and Conditions were not accepted.");

                RuleFor(c => c.AcknowledgedFeesAndCharges)
                    .Equal(true)
                    .WithMessage("Fees and charges were not acknowledged.");
            }
        }

        public class Handler : AsyncRequestHandler<ReportSyncSignupCommand>
        {
            private readonly IOptions<WCACoreSettings> _appSettings;
            private readonly WCADbContext _wCADbContext;
            private readonly IEmailSender _emailSender;
            private readonly IMapper _mapper;
            private readonly Validator _validator;

            public Handler(IOptions<WCACoreSettings> appSettings,
                WCADbContext wCADbContext,
                IEmailSender emailSender,
                IMapper mapper,
                Validator validator)
            {
                _appSettings = appSettings;
                _wCADbContext = wCADbContext;
                _emailSender = emailSender;
                _mapper = mapper;
                _validator = validator;
            }

            protected override async Task Handle(ReportSyncSignupCommand message, CancellationToken token)
            {
                if (message is null) throw new ArgumentNullException(nameof(message));

                await _validator.ValidateAndThrowAsync(message);

                var actionstepOrgKey = "Unknown";

                // Get the org associated with the current user. If the user
                // has multiple orgs, get the one whose credentials were most recently updated.
                message.ActionstepOrg = _wCADbContext.ActionstepCredentials
                    .Include(b => b.ActionstepOrg)
                    .Where(org => org.Owner.Id == message.AuthenticatedUser.Id)
                    .OrderBy(org => org.LastUpdatedUtc).FirstOrDefault()?.ActionstepOrg;

                actionstepOrgKey = message.ActionstepOrg?.Key;

                var customerService = new StripeCustomerService(message.StripeApiSecret);
                var newStripeCustomer = new StripeCustomerCreateOptions();
                newStripeCustomer.SourceToken = message.PaymentGatewayToken;
                newStripeCustomer.Email = message.BillingContactEmail;
                newStripeCustomer.Description = message.Company;
                newStripeCustomer.Metadata = new Dictionary<string, string>
                {
                    ["Billing Contact Firstname"] = message.BillingContactFirstname,
                    ["Billing Contact Lastname"] = message.BillingContactLastname,
                    ["Billing Contact Email"] = message.BillingContactEmail,
                    ["Billing Contact Phone"] = message.BillingContactPhone,
                    ["Company"] = message.Company,
                    ["ABN"] = message.ABN,
                    ["Reporting Sync Billing Frequency"] = message.BillingFrequency,
                    ["Reporting Sync Actionstep Org Key"] = actionstepOrgKey,
                };

                StripeCustomer stripeCustomer = customerService.Create(newStripeCustomer);
                // TODO: To determine what stripe fees will be applied, we need to know the
                // country of the credit card. Australian cards have one charge, whereas
                // international cards have a slightly higher charge. Here is a sample showing
                // how to get to the card information.
                // var testCountry = stripeCustomer.Sources.Data[0].Card.Country;

                // Save to database
                var newDbEntry = _mapper.Map<ReportSyncSignupSubmission>(message);
                newDbEntry.StripeId = stripeCustomer.Id;
                newDbEntry.SubmittedDateTimeUtc = DateTime.UtcNow;
                _wCADbContext.Add(newDbEntry);
                var saveTask = _wCADbContext.SaveChangesAsync();


                // Send notification to Customer
                var sendCustomerEmailTask = _emailSender.SendEmailAsync(new EmailSenderRequest
                {
                    To = {
                        new EmailRecipient(
                            message.BillingContactEmail,
                            $"{message.BillingContactFirstname} {message.BillingContactLastname}"),
                        new EmailRecipient(
                            message.ServiceContactEmail,
                            $"{message.ServiceContactFirstname} {message.ServiceContactLastname}")
                    },
                    TemplateId = "135b7ecb-96c9-4e44-a3e1-723ace8c3509",
                    Substitutions = {
                        { "--Firstname--", message.ServiceContactFirstname },
                        { "--Actionstep_OrgKey--", actionstepOrgKey },
                        { "--Billing_Contact_Firstname--", message.BillingContactFirstname },
                        { "--Billing_Contact_Lastname--", message.BillingContactLastname },
                        { "--Billing_Contact_Email--", message.BillingContactEmail },
                        { "--Service_Contact_Firstname--", message.ServiceContactFirstname },
                        { "--Service_Contact_Lastname--", message.ServiceContactLastname },
                        { "--Service_Contact_Email--", message.ServiceContactEmail },
                        { "--Billing_Frequency--", message.BillingFrequency }
                    }
                });

                // Send notification to WCA
                var sendWcaNotificationEmailTask = _emailSender.SendEmailAsync(new EmailSenderRequest
                {
                    To = { new EmailRecipient(_appSettings.Value.WCANotificationEmail) },
                    Subject = "New Reporting Sync Sign-Up",
                    MessageIsHtml = false,
                    Message = $"A customer has signed up for the Reporting Sync service. Details as follows:" + Environment.NewLine + Environment.NewLine +
                    $" Created by user:               {message.AuthenticatedUser.Email}" + Environment.NewLine +
                    $" Service Contact Firstname:     {message.ServiceContactFirstname}" + Environment.NewLine +
                    $" Service Contact Lastname:      {message.ServiceContactLastname}" + Environment.NewLine +
                    $" Service Contact Email:         {message.ServiceContactEmail}" + Environment.NewLine +
                    $" Service Contact Phone:         {message.ServiceContactPhone}" + Environment.NewLine +
                    $" Actionstep org:                {actionstepOrgKey}" + Environment.NewLine +
                    $" Company:                       {message.Company}" + Environment.NewLine +
                    $" ABN:                           {message.ABN}" + Environment.NewLine +
                    $" Address line 1:                {message.Address1}" + Environment.NewLine +
                    $" Address line 2:                {message.Address2}" + Environment.NewLine +
                    $" City:                          {message.City}" + Environment.NewLine +
                    $" Postcode:                      {message.Postcode}" + Environment.NewLine +
                    $" State:                         {message.State}" + Environment.NewLine +
                    $" Billing Contact Firstname:     {message.BillingContactFirstname}" + Environment.NewLine +
                    $" Billing Contact Lastname:      {message.BillingContactLastname}" + Environment.NewLine +
                    $" Billing Contact Email:         {message.BillingContactEmail}" + Environment.NewLine +
                    $" Billing Contact Phone:         {message.BillingContactPhone}" + Environment.NewLine +
                    $" Billing Frequency:             {message.BillingFrequency}" + Environment.NewLine +
                    $" Accepted Terms & Conditions:   {message.AcceptedTermsAndConditions}" + Environment.NewLine +
                    $" Acknowledged fees and charges: {message.AcknowledgedFeesAndCharges}" + Environment.NewLine
                });

                await Task.WhenAll(saveTask, sendCustomerEmailTask, sendWcaNotificationEmailTask);
            }
        }
    }
}
