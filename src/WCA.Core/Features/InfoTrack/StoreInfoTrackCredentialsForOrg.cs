using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Services.Email;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.InfoTrack
{
    public class StoreInfoTrackCredentialsForOrg
    {
        public class StoreInfoTrackCredentialsForOrgCommand : IAuthenticatedCommand
        {
            public WCAUser AuthenticatedUser { get; set; }
            public string ActionstepOrgKey { get; set; }
            public string InfoTrackUsername { get; set; }
            public string InfoTrackPassword { get; set; }
            public bool AcceptedTermsAndConditions { get; set; }
            public bool IsSignUp { get; set; }
        }

        public class Validator : AbstractValidator<StoreInfoTrackCredentialsForOrgCommand>
        {
            public Validator()
            {
                RuleFor(c => c.ActionstepOrgKey).NotEmpty();
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.InfoTrackUsername).NotEmpty();
                RuleFor(c => c.InfoTrackPassword).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<StoreInfoTrackCredentialsForOrgCommand>
        {
            private readonly WCADbContext _wCADbContext;
            private readonly Validator _validator;
            private readonly WCACoreSettings _appSettings;
            private readonly IInfoTrackCredentialRepository _infoTrackCredentialRepository;
            private readonly IEmailSender _emailSender;

            public Handler(
                WCADbContext wCADbContext,
                Validator validator,
                IOptions<WCACoreSettings> appSettingsAccessor,
                IInfoTrackCredentialRepository infoTrackCredentialRepository,
                IEmailSender emailSender)
            {
                if (appSettingsAccessor is null) throw new ArgumentNullException(nameof(appSettingsAccessor));

                _appSettings = appSettingsAccessor.Value;
                _wCADbContext = wCADbContext;
                _validator = validator;
                _infoTrackCredentialRepository = infoTrackCredentialRepository;
                _emailSender = emailSender;
            }

            protected override async Task Handle(StoreInfoTrackCredentialsForOrgCommand message, CancellationToken token)
            {
                if (message is null) throw new ArgumentNullException(nameof(message));
                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Unable to save Property Resources, the command message was invalid.", result.Errors);
                }

                var userHasNonExpiredCredentialsForOrg = _wCADbContext.ActionstepCredentials
                    .Any(c => c.Owner == message.AuthenticatedUser &&
                         c.ActionstepOrg.Key == message.ActionstepOrgKey &&
                         c.RefreshTokenExpiryUtc > DateTime.UtcNow);

                if (!userHasNonExpiredCredentialsForOrg)
                {
                    throw new UnauthorizedAccessException(
                        $"User {message.AuthenticatedUser.Id} doesn't have valid Actionstep credentials " +
                        $"for the Actionste organisation {message.ActionstepOrgKey}.");
                }

                var saveInfoTrackCredentials = _infoTrackCredentialRepository.SaveOrUpdateCredential(
                    message.ActionstepOrgKey, message.InfoTrackUsername, message.InfoTrackPassword);

                var emailTask = _emailSender.SendEmailAsync(new EmailSenderRequest()
                {
                    To = { new EmailRecipient(_appSettings.InfoTrackSettings.NewInfoTrackEmailNotifications) },
                    Cc = { new EmailRecipient(_appSettings.WCANotificationEmail) },
                    Subject = "Customer sign-up for WorkCloud/Actionstep/InfoTrack Integration",
                    MessageIsHtml = false,
                    Message = $"A customer has signed up for WorkCloud's Actionstep and InfoTrack integration. Please update your records to ensure this customer is registered with WorkCloud:"
                        + Environment.NewLine + Environment.NewLine +
                        $" Customer contact:                {message.AuthenticatedUser.Email}" + Environment.NewLine +
                        $" First Name:                      {message.AuthenticatedUser.FirstName}" + Environment.NewLine +
                        $" Last Name:                       {message.AuthenticatedUser.LastName}" + Environment.NewLine +
                        $" Actionstep Org:                  {message.ActionstepOrgKey}" + Environment.NewLine +
                        $" InfoTrack Username:              {message.InfoTrackUsername}" + Environment.NewLine +
                        $" Accepted WCA Terms & Conditions: {message.AcceptedTermsAndConditions}" + Environment.NewLine +
                        Environment.NewLine +
                        "This is an automated message, please do not reply."
                });

                await Task.WhenAll(saveInfoTrackCredentials, emailTask);
            }
        }
    }
}
