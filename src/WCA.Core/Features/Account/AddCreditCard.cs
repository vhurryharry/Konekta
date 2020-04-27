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
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Account
{
    public class AddCreditCard
    {
        public class AddCreditCardCommand : IAuthenticatedCommand<AddCreditCardResult>
        {
            public WCAUser AuthenticatedUser { get; set; }
            public string PaymentGatewayToken { get; set; }
            public bool AcceptedTermsAndConditions { get; set; }
        }

        public class AddCreditCardResult
        {
            public enum AddCreditCardStatus
            {
                CreditCardSaved = 0,
            }

            public AddCreditCardStatus Status { get;set;}
        }

        public class Validator : AbstractValidator<AddCreditCardCommand >
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.PaymentGatewayToken).NotNull();
                RuleFor(c => c.AcceptedTermsAndConditions)
                    .Equal(true)
                    .WithMessage("Terms and Conditions were not accepted.");
            }
        }

        public class Handler : IRequestHandler<AddCreditCardCommand , AddCreditCardResult>
        {
            private readonly IOptions<WCACoreSettings> _appSettings;
            private readonly WCADbContext _wCADbContext;

            public Handler(
                IOptions<WCACoreSettings> appSettings,
                WCADbContext wCADbContext)
            {
                _appSettings = appSettings;
                _wCADbContext = wCADbContext;
            }

            public async Task<AddCreditCardResult> Handle(AddCreditCardCommand  command, CancellationToken token)
            {
                // Find the most recent conveyancing sign-up for 
                var orgsForUser = _wCADbContext.ActionstepCredentials
                    .AsNoTracking()
                    .Where(c => c.Owner == command.AuthenticatedUser)
                    .Select(c => c.ActionstepOrg.Key.ToUpperInvariant());

                var customerService = new StripeCustomerService(_appSettings.Value.StripeSettings.ApiSecret);
                var newCustomer = new StripeCustomerCreateOptions();
                newCustomer.SourceToken = command.PaymentGatewayToken;
                newCustomer.Email = command.AuthenticatedUser.Email;

                // Try to find a conveyancing sign-up for the user. Append metadata to stripe record if we find one.
                if (orgsForUser != null)
                {
                    var mostRecentConveyancingSignupForOneOfUserOrgs = await _wCADbContext.ConveyancingSignupSubmissions
                        .AsNoTracking()
                        .OrderByDescending(s => s.SubmittedDateTimeUtc)
                        .FirstOrDefaultAsync(s => orgsForUser.Contains(s.OrgKey.ToUpperInvariant()));

                    if (mostRecentConveyancingSignupForOneOfUserOrgs != null)
                    {
                        newCustomer.Description = mostRecentConveyancingSignupForOneOfUserOrgs.Company;
                        newCustomer.Metadata = new Dictionary<string, string>
                        {
                            ["Phone"] = mostRecentConveyancingSignupForOneOfUserOrgs.Phone,
                            ["Email"] = mostRecentConveyancingSignupForOneOfUserOrgs.Email,
                            ["Company"] = mostRecentConveyancingSignupForOneOfUserOrgs.Company,
                            ["ABN"] = mostRecentConveyancingSignupForOneOfUserOrgs.ABN,
                            ["ConveyancingApp"] = mostRecentConveyancingSignupForOneOfUserOrgs.ConveyancingApp,
                            ["PromoCode"] = mostRecentConveyancingSignupForOneOfUserOrgs.PromoCode,
                            ["OrgKey"] = mostRecentConveyancingSignupForOneOfUserOrgs.OrgKey,
                            ["SupportPlanOption"] = mostRecentConveyancingSignupForOneOfUserOrgs.SupportPlanOption,
                        };
                    }
                    else
                    {
                        // If no conveyancing signups, add all org keys found to make it easier to link the stripe account to a client.
                        newCustomer.Metadata = new Dictionary<string, string>
                        {
                            ["OrgKey"] = string.Join(", ", orgsForUser)
                        };
                    }
                }

                StripeCustomer stripeCustomer = customerService.Create(newCustomer);

                return new AddCreditCardResult() { Status = AddCreditCardResult.AddCreditCardStatus.CreditCardSaved };
            }
        }
    }
}