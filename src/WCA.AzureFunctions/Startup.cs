using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using System;
using System.IO;
using System.Reflection;
using WCA.Core;
using WCA.Core.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(WCA.AzureFunctions.Startup))]
namespace WCA.AzureFunctions
{
    public class WCAMessageSerializer : IMessageSerializerSettingsFactory
    {
        public JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            return jsonSerializerSettings;
        }
    }

    public class Startup : FunctionsStartup
    {
        private bool? isDevelopment;

        private bool IsDevelopment
        {
            get
            {
                if (isDevelopment.HasValue) return isDevelopment.Value;

                var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (aspNetCoreEnvironment != null &&
                    aspNetCoreEnvironment.Equals("Development", StringComparison.InvariantCultureIgnoreCase))
                    isDevelopment = true;
                else
                    isDevelopment = false;

                return isDevelopment.Value;
            }
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));

            var configurationRoot = GetConfigurationRoot();

            builder.Services.AddOptions<WCACoreSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind(settings);
                });

            builder.Services.AddSingleton<IMessageSerializerSettingsFactory, WCAMessageSerializer>();
            builder.Services.AddSingleton(CloudStorageAccount.Parse(configurationRoot.GetConnectionString("StorageConnection")));

            builder.Services.AddWCACore(o =>
            {
                o.Configuration = configurationRoot;
                o.IsDevelopment = IsDevelopment;
            });

            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            // If you have issues with authenticating to Azure Key Vault, make sure you're using the correct principal
            // by checking azureServiceTokenProvider.PrincipalUsed.UserPrincipalName.
            // For local dev this uses the account that you're logged in to VS2019 with.
#pragma warning disable CA2000 // Dispose objects before losing scope: Not disposed as this is registered as a singleton.
            var wcaKeyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
#pragma warning restore CA2000 // Dispose objects before losing scope

            builder.Services.AddSingleton<IKeyVaultClient>(wcaKeyVaultClient);
        }

        private IConfigurationRoot GetConfigurationRoot()
        {
            // During publishing, the appsetting.json file from WCA.Web will be published
            // with this function app.

            // When running in development mode, we read the WCA.Web appsettings.json file
            // directly. This is achieved via a linked file reference in the .csproj file.

            // The function directory (which contains appsettings.json) doesn't appear to be
            // available via the ExtensionConfigContext, so we're resorting to Assembly information.
            // We get parent of parent to account for the ./bin/ directory.
            var basePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.ToString();

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            if (IsDevelopment)
            {
                builder.AddUserSecrets<Startup>();
            }

            return builder.Build();
        }
    }
}
