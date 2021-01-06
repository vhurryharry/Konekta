using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json.Converters;
using System;
using System.Diagnostics;
using System.Globalization;
using WCA.Actionstep.AspNetCore.Authentication;
using WCA.Core;
using WCA.Core.Extensions.Configuration;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Security;
using WCA.Core.Services;
using WCA.Data;
using WCA.Domain.Models.Account;
using WCA.GlobalX.Client;
using WCA.Web.AutoMapper;
using WCA.Web.Extensions;
using WCA.Web.FeatureFlags;
using WCA.Web.Filters;
using WCA.Web.Security;

namespace WCA.Web
{
    public static class UIDefinitions
    {
        public static string BuildNumber { get; set; }
        public static int YearWcaIncorporated { get; set; }
        public static string BingMapsAPIKey { get; set; }
        public static bool UseTestScenarios { get; set; }
        public static bool IsDevEnvironment { get; set; }
    }

    public class Startup
    {
        private const string AllowAdminSitePolicyName = "AllowAdminSite";
        private readonly IWebHostEnvironment _env;
        private AppSettings appSettings;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Trace.TraceInformation("WCA Startup: Entering Startup Constructor");

            Configuration = configuration;
            _env = env;

            Trace.TraceInformation("WCA Startup: Entering Leaving Constructor");
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(options => { options.DeveloperMode = _env.IsDevelopment(); });

            Trace.TraceInformation("WCA Startup: Entering ConfigureServices");

            services.AddFeatureManagement()
                .AddFeatureFilter<OrgKeyFilter>()
                .AddFeatureFilter<UserFilter>();
            services.AddOptions();
            services.AddLogging();
            services.Configure<AppSettings>(Configuration);
            services.Configure<ActionstepSettings>(Configuration.GetSection(nameof(WCACoreSettings)).GetSection(nameof(ActionstepSettings)));
            services.Configure<GlobalXOptions>(Configuration.GetSection(nameof(WCACoreSettings)).GetSection(nameof(GlobalXOptions)));
            appSettings = Configuration.Get<AppSettings>();

            services.AddWCACore(o =>
            {
                o.Configuration = Configuration;
                o.AutoMapperProfileAssemblyMarkerTypes.Add(typeof(WebAutoMapperProfile));
                o.IsDevelopment = _env.IsDevelopment();
            });

            AddWCADataProtection(services, appSettings.WCACoreSettings);

            services.AddScoped<WCADbContextTransactionFilter>();

            // Use a custom signin manager that automatically persists external login information
            services.AddScoped<SignInManager<WCAUser>, WCASignInManager>();
            services.AddScoped<WCASignInManager>();

            services.AddMvc(options =>
            {
                // Require authorization across the application
                var authorizePolicy = new AuthorizationPolicyBuilder(
                        IdentityConstants.ApplicationScheme,
                        "ActionstepJwt")
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(authorizePolicy));

                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                options.Filters.Add<ApiExceptionFilter>();
            })
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<AddOrUpdateActionstepCredential.ValidatorCollection>();
                });

            // Swagger currently only to be used in development. Further work
            // is required to clean this up before we expose documentation publicly.
            services.AddSwaggerDocument(config =>
            {
                config.DocumentName = "v0.1";
                config.PostProcess = document =>
                {
                    document.Info.Version = "v0.1";
                    document.Info.Title = "WorkCloud API";
                    document.Info.Description = "WorkCloud integrations and services.";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "WorkCloud Support",
                        Email = "support@workcloud.com.au",
                        Url = "https://www.workcloud.support/"
                    };
                };
            });

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeAreaFolder("Admin", "/", AllowAdminSitePolicyName);
            });

            // Add application services.
            // TODO: Refactor AccountController stuff to use SendGrid, or
            //       implement IEmailSender and ISmsSender
            // services.AddTransient<IEmailSender, AuthMessageSender>();
            // services.AddTransient<ISmsSender, AuthMessageSender>();

            // Configure authentication middleware
            services.AddIdentity<WCAUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.ClaimsIdentity.UserIdClaimType = "KonektaUserID";
            })
                .AddEntityFrameworkStores<WCADbContext>();

            services.ConfigureActionstepJwtOptions()
                .Configure<ILogger<WCASignInManager>>((options, logger) =>
                {
                    options.Audience = appSettings.WCACoreSettings.ActionstepSettings.ValidTokenAudience;
                    options.Events.OnTokenValidated = context => WCASignInManager.SignInWithActionstepJwt(context, logger);

                });

            services.ConfigureActionstepLoginOptions();

            services.ConfigureApplicationCookie(o =>
            {
                const int days = 30;
                o.SlidingExpiration = true;
                var validCookieTime = new TimeSpan(days, 0, 0, 0, 0);
                o.Cookie.MaxAge = validCookieTime;
                o.ExpireTimeSpan = validCookieTime;
                o.LoginPath = new PathString("/Identity/Account/Login");
                o.LogoutPath = new PathString("/Identity/Account/Logout");
                o.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                // SameSite must be none so that it works in the Actionstep iframe.
                o.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
               .AddActionstepJwt()
               .AddActionstepLogin();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AllowAdminSitePolicyName, policy =>
                    policy.RequireRole(
                        SecurityRoles.AllowAdminSite.ToString(),
                        SecurityRoles.GlobalAdministrator.ToString()));
            });

            // Add application services.
            services.AddTransient<IStampDutyService, StampDutyService>();

            // Configure antiforgery to accept token in header
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-Token";
                options.SuppressXFrameOptionsHeader = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                // Also check Origin header?

                // SameSite must be none so that it works in the Actionstep iframe.
                options.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddCors();

#pragma warning disable CA2000 // Dispose objects before losing scope: Not disposing because this is added as a singleton
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
#pragma warning restore CA2000 // Dispose objects before losing scope

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "client-app/build";
            });

            Trace.TraceInformation("WCA Startup: Leaving ConfigureServices");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IAntiforgery antiforgery)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            Trace.TraceInformation("WCA Startup: Entering Configure");

            app.MigrateDatabasesIfRequired();

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // using (var scope = app.ApplicationServices.CreateScope())
                // {
                //     var dbContext = scope.ServiceProvider.GetService<WCADbContext>();
                //     dbContext.SeedTestData();
                // }

                // [Announcement] Obsoleting Microsoft.AspNetCore.SpaServices and Microsoft.AspNetCore.NodeServices
                // https://github.com/aspnet/AspNetCore/issues/12890
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ConfigFile = "./ClientApp/webpack.config.js",
                    // Error when using HMR with ASP.NET CORE 2.0, see https://github.com/aspnet/JavaScriptServices/issues/1204
                    HotModuleReplacementEndpoint = "/dist/__webpack_hmr"
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(h => h.MaxAge(days: 365).IncludeSubdomains().Preload());
            }

            if (!appSettings.WCACoreSettings.DisableHttpsRedirection)
            {
                app.UseHttpsRedirection();
            }

            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            // Add enforced Content Security Policies
            app.UseCsp(options => options
                .FrameAncestors(s => s.CustomSources(appSettings.WCACoreSettings.AllowedFrameAncestorUrls))
                .ReportUris(r => r.Uris("https://konekta.report-uri.com/r/d/csp/enforce"))
            );

            // Add Content Security Policies in report mode (to be eventually moved to UseCsp above
            app.UseCspReportOnly(options => options
                .DefaultSources(s => s.Self())
                .FrameSources(s => s.CustomSources(new[] { "https://js.stripe.com", "https://m.stripe.network", "https://vars.hotjar.com", "https://az416426.vo.msecnd.net", "https://ajax.aspnetcdn.com", "https://*.globalx.com.au" }))
                .ConnectSources(s => s.Self().CustomSources(new[] { "https://in.hotjar.com", "https://vc.hotjar.io", "https://dc.services.visualstudio.com" }))
                .ScriptSources(s => s.Self().UnsafeInline().UnsafeEval().CustomSources(new[] { "https://js.stripe.com", "https://static.hotjar.com", "https://script.hotjar.com", "https://az416426.vo.msecnd.net", "https://ajax.aspnetcdn.com", "https://widget.freshworks.com", "https://*.globalx.com.au" }))
                .StyleSources(s => s.Self().UnsafeInline().CustomSources(new[] { "https://fonts.googleapis.com" }))
                .FontSources(s => s.Self().CustomSources(new[] { "https://fonts.gstatic.com", "https://fonts.googleapis.com", "https://static2.sharepointonline.com", "https://spoprod-a.akamaihd.net" }))
                .ReportUris(r => r.Uris("https://konekta.report-uri.com/r/d/csp/reportOnly")));

            app.UseAzureAppConfiguration();
            app.UseAuthentication();

            app.UseOpenApi();

            if (_env.IsDevelopment())
            {
                // Swagger UI currently only to be used in development. Further work
                // is required to clean this up before we expose documentation publicly.
                app.UseSwaggerUi3();
            }

            // provide antiforgery token in a (js-readable) cookie, for use by aurelia
            app.Use(next => context =>
            {
                var path = context.Request.Path.Value;
                var tokens = antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions()
                {
                    HttpOnly = false,
                    Secure = true,
                    // SameSite must be none so that it works in the Actionstep iframe.
                    SameSite = SameSiteMode.None
                });
                return next(context);
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                // Aurelia SPA
                endpoints.MapFallbackToPage("wca/{*path}", "/wca");

                // MVC and API routes
                endpoints.MapDefaultControllerRoute();
            });

            app.UseSpa(spa =>
            {
                // 'client-app' is the new client side app.
                // 'ClientApp' is the old app.
                spa.Options.SourcePath = "client-app";

                if (_env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            UIDefinitions.BuildNumber = Configuration["BuildNumber"];
            UIDefinitions.YearWcaIncorporated = Convert.ToInt32(Configuration["YearWcaIncorporated"], CultureInfo.InvariantCulture);
            UIDefinitions.IsDevEnvironment = _env.IsDevelopment();

            Trace.TraceInformation("WCA Startup: Leaving Configure");
        }

        private void AddWCADataProtection(IServiceCollection services, WCACoreSettings wcaCoreSettings)
        {
            const string dpapiKeyName = "wca-web-dpapi";

            var cloudStorageAccount = CloudStorageAccount.Parse(Configuration.GetConnectionString("StorageConnection"));
            var newCloudStorageAccount = Microsoft.Azure.Storage.CloudStorageAccount.Parse(Configuration.GetConnectionString("StorageConnection"));
            services.AddSingleton(cloudStorageAccount);

            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            // If you have issues with authenticating to Azure Key Vault, make sure you're using the correct principal
            // by checking azureServiceTokenProvider.PrincipalUsed.UserPrincipalName.
            // For local dev this uses the account that you're logged in to VS2017 with.

#pragma warning disable CA2000 // Dispose objects before losing scope: Not disposing as we're registering it as a singleton
            var wcaKeyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
#pragma warning restore CA2000 // Dispose objects before losing scope

            services.AddSingleton<IKeyVaultClient>(wcaKeyVaultClient);
            if (wcaCoreSettings.UseAzureStorageAndKeyVaultForDPAPI)
            {
                string dpapiKeyIdentifier;

                try
                {
                    var keyTask = wcaKeyVaultClient.GetKeyAsync(wcaCoreSettings.CredentialAzureKeyVaultUrl, dpapiKeyName);
                    keyTask.Wait();
                    dpapiKeyIdentifier = keyTask.Result.KeyIdentifier.Identifier;
                }
                catch (AggregateException aex)
                {
                    if ((aex.InnerException as KeyVaultErrorException)?.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {

                        var createKeyTask = wcaKeyVaultClient.CreateKeyAsync(
                            wcaCoreSettings.CredentialAzureKeyVaultUrl,
                            dpapiKeyName,
                            new NewKeyParameters()
                            {
                                Kty = "RSA",
                                KeySize = 4096
                            });
                        createKeyTask.Wait();
                        dpapiKeyIdentifier = createKeyTask.Result.KeyIdentifier.Identifier;
                    }
                    else
                    {
                        throw;
                    }
                }

                // Ensure container exists for DPAPI
                cloudStorageAccount.CreateCloudBlobClient().GetContainerReference(dpapiKeyName).CreateIfNotExistsAsync();

                // Configure Data Protection (DPAPI) to use Azure Storage/Key Vault
                // so that sessions are persisted across deployments/slot swaps.
                services.AddDataProtection(opts =>
                {
                    opts.ApplicationDiscriminator = "WCA.Web";
                })
                    .PersistKeysToAzureBlobStorage(newCloudStorageAccount, $"/{dpapiKeyName}/keys.xml")
                    .ProtectKeysWithAzureKeyVault(wcaKeyVaultClient, dpapiKeyIdentifier);
            }
            else
            {
                services.AddDataProtection(opts =>
                {
                    opts.ApplicationDiscriminator = "WCA.Web";
                });
            }
        }
    }
}