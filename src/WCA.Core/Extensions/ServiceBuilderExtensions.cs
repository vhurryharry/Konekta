using AutoMapper;
using DBA.FreshdeskSharp;
using FluentValidation;
using MediatR;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Extensions;
using WCA.Core.AutoMapper;
using WCA.Core.CQRS;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Features.Conveyancing.Services;
using WCA.Core.Features.FirstTitle.Connection;
using WCA.Core.Features.GlobalX.Authentication;
using WCA.Core.Features.InfoTrack;
using WCA.Core.Features.Pexa;
using WCA.Core.Logging;
using WCA.Core.Services;
using WCA.Core.Services.DurableFunctions;
using WCA.Core.Services.Email;
using WCA.Core.Services.SupportSystem;
using WCA.Data.Extensions;
using WCA.Domain.Actionstep;
using WCA.Domain.CQRS;
using WCA.FirstTitle.Client;
using WCA.GlobalX.Client;
using WCA.GlobalX.Client.Authentication;
using Type = System.Type;

namespace WCA.Core.Extensions.Configuration
{
    public class WCACoreOptions
    {
        /// <summary>
        /// Gets or sets the AutoMapper profile assembly marker types.
        /// </summary>
        /// <value>
        /// The AutoMapper profile assembly marker types.
        /// </value>
        public List<Type> AutoMapperProfileAssemblyMarkerTypes { get; } = new List<Type>();

        public IConfiguration Configuration { get; set; }

        public bool IsDevelopment { get; set; } = false;
    }

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Core WCA services. Registeres WCA Core dependencies.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWCACore(this IServiceCollection services, Action<WCACoreOptions> setupAction)
        {
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction), "A setup action is required to supply a valid IConfiguration object.");

            var wcaCoreOptions = new WCACoreOptions();
            setupAction(wcaCoreOptions);

            if (wcaCoreOptions.Configuration == null)
                throw new ArgumentNullException(
                    nameof(setupAction),
                    "A setup action was supplied, however the Configuration property was null. " +
                    "A value must be supplied for the Configuration property.");

            var wcaCoreConfiguration = wcaCoreOptions.Configuration.GetSection("WCACoreSettings");
            services.Configure<WCACoreSettings>(wcaCoreConfiguration);
            var coreSettings = wcaCoreConfiguration.Get<WCACoreSettings>();

            services.Configure<GlobalXOptions>(wcaCoreOptions.Configuration.GetSection("WCACoreSettings:GlobalXOptions"));

            JsonConvert.DefaultSettings = () =>
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                return jsonSerializerSettings;
            };

            services.AddMemoryCache();
            services.AddSingleton<IEmailSender, SendGridEmailSender>();
            services.AddSingleton<ISupportSystem, FreshDeskSupportSystem>();
            services.AddSingleton<IDurableFunctionsService, DurableFunctionsService>();
            AddFreshDesk(services, coreSettings.FreshDeskSettings);

#pragma warning disable CA2000 // Dispose objects before losing scope. Added as Singleton so will exist for lifetime of app.
            services.AddSingleton<IEventStore>(sp =>
            {
                // Event store configuration
                var eventStoreBuilder = Wireup.Init()
                    .UsingSqlPersistence(SqlClientFactory.Instance, wcaCoreOptions.Configuration.GetConnectionString("DefaultConnection"))
                    .WithDialect(new MsSqlDialect())
                    .InitializeStorageEngine()
                    .UsingCustomSerialization(new NEventStoreJsonSerializer())
                    .Compress();

                if (wcaCoreOptions.IsDevelopment)
                {
                    eventStoreBuilder.LogTo((typeToLog) => new NEventStoreTelemetryLoggerAdapter(sp.GetService<IOptions<TelemetryConfiguration>>(), typeToLog, NEventStore.Logging.LogLevel.Verbose));
                    eventStoreBuilder.LogToConsoleWindow();
                }
                else
                {
                    eventStoreBuilder.LogTo((typeToLog) => new NEventStoreTelemetryLoggerAdapter(sp.GetService<IOptions<TelemetryConfiguration>>(), typeToLog, NEventStore.Logging.LogLevel.Info));
                }

                return new NEventStoreImplementation(eventStoreBuilder.Build());
            });

#pragma warning restore CA2000 // Dispose objects before losing scope
            services.AddSingleton<IEventSourcedAggregateRepository<ActionstepMatter>, AggregateRepository<ActionstepMatter>>();
            services.AddSingleton<IActionstepToWCAMapper>(new ActionstepToWCAMapper());

            // AutoMapper
            var autoMapperProfileTypes = new Type[] { typeof(CommandProfile) };
            if (wcaCoreOptions.AutoMapperProfileAssemblyMarkerTypes == null)
            {
                services.AddAutoMapper(autoMapperProfileTypes);
            }
            else
            {
                services.AddAutoMapper(
                    autoMapperProfileTypes.Concat(
                        wcaCoreOptions.AutoMapperProfileAssemblyMarkerTypes).ToArray());
            }

            services.AddWCAData(
                wcaCoreOptions.Configuration.GetConnectionString("DefaultConnection"));

            services.AddTransient<ITelemetryLogger, AppInsightsTelemetryLogger>();

            Assembly wcaCoreAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly;

            var allValidatorTypes = wcaCoreAssembly
                .GetTypes()
                .Where(t => typeof(IValidator).IsAssignableFrom(t));

            foreach (var validatorType in allValidatorTypes)
            {
                services.AddSingleton(validatorType);
            }

            services.AddMediatR(wcaCoreAssembly);

            services.AddTransient<ITokenSetRepository, TokenSetRepository>();
            services.AddTransient<IGlobalXApiTokenRepository, GlobalXApiTokenRepository>();
            services.AddTransient<IGlobalXCredentialsRepository, GlobalXCredentialsRepository>();
            services.AddTransient<IFirstTitleCredentialRepository, FirstTitleCredentialRepository>();
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddSingleton<IInfoTrackCredentialRepository, InfoTrackCredentialRepository>();

            // Using IHttpClientFactory typed approach which is new in netcoreapp2.1 as per
            // https://github.com/aspnet/HttpClientFactory/wiki/Consumption-Patterns
            var productInfoHeaderValue = new ProductInfoHeaderValue("WorkCloudApplicationsAutomation", "1.0");
            services.AddTransient<HttpClientAppInsightsHandler>();

            services.AddHttpClient();

            // For more info on Polly and retry policies
            // See https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory#using-addtransienthttperrorpolicy

            var defaultSleepDurations = new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                };

            services.AddHttpClient<IGlobalXService, GlobalXService>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(defaultSleepDurations))
                .AddHttpMessageHandler<HttpClientAppInsightsHandler>();

            services.AddHttpClient<IExtendedPexaService, ExtendedPexaService>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(defaultSleepDurations))
                .AddHttpMessageHandler<HttpClientAppInsightsHandler>();

            services.AddActionstepService(
                productInfoHeaderValue,
                options =>
                {
                    options.ClientId = coreSettings.ActionstepSettings.ApiClientId;
                    options.ClientSecret = coreSettings.ActionstepSettings.ApiClientSecret;
                    options.ActionstepEnvironment = coreSettings.ActionstepSettings.ActionstepEnvironment;
                })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(defaultSleepDurations))
                .AddHttpMessageHandler<HttpClientAppInsightsHandler>();

            services.AddHttpClient<InfoTrackService>(c =>
            {
                c.DefaultRequestHeaders.UserAgent.Add(productInfoHeaderValue);
            })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(defaultSleepDurations))
                .AddHttpMessageHandler<HttpClientAppInsightsHandler>();

            services.AddSingleton<IFirstTitleToWCAMapper>(new FirstTitleToWCAMapper());
            services.AddSingleton<IWCAToFirstTitleMapper>(new WCAToFirstTitleMapper());

            services.AddHttpClient<IFirstTitleClient, FirstTitleClient>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(defaultSleepDurations))
                .AddHttpMessageHandler<HttpClientAppInsightsHandler>();

            return services;
        }

        private static void AddFreshDesk(IServiceCollection services, FreshDeskSettings freshDeskSettings)
        {
            if (freshDeskSettings is null)
                throw new ArgumentNullException(nameof(freshDeskSettings));

            // TODO: update tests so these can be re-enabled
            //if (string.IsNullOrEmpty(freshDeskSettings.Domain))
            //    throw new ArgumentException("FreshDesk Domain must be set", nameof(freshDeskSettings));

            //if (string.IsNullOrEmpty(freshDeskSettings.APIKey))
            //    throw new ArgumentException("FreshDesk APIKey must be set", nameof(freshDeskSettings));

            var credentials = new FreshdeskCredentials(freshDeskSettings.APIKey);
            var config = new FreshdeskConfig
            {
                Domain = freshDeskSettings.Domain,
                Credentials = credentials,
                MultiCompanySupport = true,
                RetryWhenThrottled = true
            };

#pragma warning disable CA2000 // Dispose objects before losing scope - Shouldn't be disposed as all it does is dispose the HttpClient
            var freshDeskClient = new FreshdeskClient(config);
#pragma warning restore CA2000 // Dispose objects before losing scope

            services.AddSingleton(freshDeskClient);
        }
    }
}
