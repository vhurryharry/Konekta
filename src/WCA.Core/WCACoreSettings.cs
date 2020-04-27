using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using WCA.Actionstep.Client;
using WCA.FirstTitle.Client;
using WCA.GlobalX.Client;
using WCA.PEXA.Client;

namespace WCA.Core
{
    public class WCACoreSettings
    {
        public string RootURL { get; set; }
        public AppUrlSettings AppUrlSettings { get; set; }
        public string SendGridApiKey { get; set; }
        public string EmailFromAddress { get; set; }
        public string EmailFromName { get; set; }
        public string CredentialAzureKeyVaultUrl { get; set; }
        public string AzureFunctionsUrl { get; set; }
        public string AzureFunctionsHubName { get; set; }
        public string AzureFunctionsHostKey { get; set; }
        public bool UseAzureStorageAndKeyVaultForDPAPI { get; set; }

        public string RedirectTicketsToEmail { get; set; }

        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "DTO")]
        public string[] AllowedFrameAncestorUrls { get; set; }

        /// <summary>
        /// The email address at WCA where notifications will be sent, such as
        /// when customer signs up.
        /// </summary>
        public string WCANotificationEmail { get; set; }

        public bool DisableHttpsRedirection { get; set; } = false;

        public StripeSettings StripeSettings { get; set; }

        public InfoTrackSettings InfoTrackSettings { get; set; }

        public ActionstepSettings ActionstepSettings { get; set; }

        public ConveyancingSettings ConveyancingSettings { get; set; }

        public PEXASettings PEXASettings { get; set; }

        public GlobalXOptions GlobalXOptions { get; set; }

        public FirstTitleSettings FirstTitleSettings { get; set; }

        public EmailToSmsSettings EmailToSmsSettings { get; set; }

        public FreshDeskSettings FreshDeskSettings { get; set; }
    }

    public class AppUrlSetting
    {
        public string AppUrl { get; set; }
        public string SupportUrl { get; set; }
        public string DomainUrl { get; set; }
    }

    public class AppUrlSettings
    {
        public AppUrlSetting WorkCloud { get; set; }
        public AppUrlSetting Konekta { get; set; }
    }

    public class ConveyancingSettings
    {
        public string SettlementCalculatorBaseUrlNSW { get; set; }
        public string SettlementCalculatorBaseUrlVIC { get; set; }
        public string SettlementCalculatorBaseUrlQLD { get; set; }
    }

    public class StripeSettings
    {
        public string ApiPublicKey { get; set; }
        public string ApiSecret { get; set; }
    }

    public class ActionstepSettings
    {
        public ActionstepEnvironment ActionstepEnvironment { get; set; } = ActionstepEnvironment.Staging;
        public string AuthClientId { get; set; }
        public string AuthClientSecret { get; set; }

        public string ApiClientId { get; set; }
        public string ApiClientSecret { get; set; }
        public string ApiScopes { get; set; }
        public bool LogInWithActionstepEnabled { get; set; }
        public string WCAAdminOrgKey { get; set; }
        public string WCAAdminMatterType { get; set; }
        public string ValidTokenAudience { get; set; }
    }

    public class ActionstepSettingsValidatorCollection : AbstractValidator<ActionstepSettings>
    {
        public ActionstepSettingsValidatorCollection()
        {
            RuleFor(s => s.AuthClientId).NotEmpty();
            RuleFor(s => s.AuthClientSecret).NotEmpty();
            RuleFor(s => s.ApiClientId).NotEmpty();
            RuleFor(s => s.ApiClientSecret).NotEmpty();
            RuleFor(s => s.ApiScopes).NotEmpty();
            RuleFor(s => s.WCAAdminOrgKey).NotEmpty();
            RuleFor(s => s.WCAAdminMatterType).NotEmpty();
        }
    }

    public class InfoTrackSettings
    {
        public string BaseApiUrl { get; set; }
        public string WCAUserName { get; set; }
        public string WCAPassword { get; set; }
        public string NewInfoTrackEmailNotifications { get; set; }

        /// <summary>
        /// Gets or sets the restricted actionstep org key.
        /// </summary>
        /// <value>
        /// If set, searches will only be allowed if the user has Actionstep
        /// credentials for the given key. If not set, searches will always be
        /// allowed.
        /// </value>
        public string RestrictedActionstepOrgKey { get; set; }
    }

    public class PEXASettings
    {
        public PEXAEnvironment Environment { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class FirstTitleSettings
    {
        public FirstTitleEnvironment Environment { get; set; }
    }

    public class EmailToSmsSettings
    {
        public bool SkipSendingSms { get; set; }
        public string ResellerApiKey { get; set; }
        public string ResellerApiSecret { get; set; }
        public string MailboxAddress { get; set; }
        public string MicrosoftGraphApiClientId { get; set; }
        public string MicrosoftGraphApiTenantID { get; set; }
        public string MicrosoftGraphApiSecret { get; set; }
    }

    public class FreshDeskSettings
    {
        public string Domain { get; set; }
        public string APIKey { get; set; }
    }
}
