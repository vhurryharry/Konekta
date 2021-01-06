using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSwag;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCA.Core.Security;
using WCA.Data;
using WCA.Domain.Models.Account;

namespace WCA.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Trace.TraceInformation("WCA Startup: Entering Main");

            Console.Title = "WCA.Web";

            var host = CreateHostBuilder(args).Build();

            if (args != null && args.Length > 0)
            {
                ProcessCliCommands(args, host).Wait();
            }
            else
            {
                host.Run();
            }

            Trace.TraceInformation("WCA Startup: Leaving Main");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var settings = config.Build();
                        var appConfigConnectionString = settings["ConnectionStrings:AppConfig"];
                        var managedIdentityConnectionEndpoint = settings["AppConfig:Endpoint"];

                        if (!string.IsNullOrEmpty(appConfigConnectionString) || !string.IsNullOrEmpty(managedIdentityConnectionEndpoint))
                        {
                            config.AddAzureAppConfiguration(options =>
                            {
                                if (string.IsNullOrEmpty(appConfigConnectionString))
                                {
                                    options.Connect(new Uri(settings["AppConfig:Endpoint"]), new ManagedIdentityCredential());
                                }
                                else
                                {
                                    options.Connect(appConfigConnectionString);
                                }

                                options.UseFeatureFlags();
                                options.ConfigureRefresh(configure => configure
                                    .Register("Sentinel", true)
                                    .SetCacheExpiration(TimeSpan.FromMinutes(30)));
                            });
                        }
                    })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                    logging.AddAzureWebAppDiagnostics();
                    logging.AddApplicationInsights();
                })
                .UseStartup<Startup>());

        /// <summary>
        /// This is pretty hacky, but a quick solution to be able to run some CLI commands.
        ///
        /// Primarily useful for basic database management, and security administration.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        private static async Task ProcessCliCommands(string[] args, IHost host)
        {
            if (args.Contains("/?") || args.Contains("-?") ||
                args.Contains("/h") || args.Contains("-h") ||
                args.Contains("/help") || args.Contains("--help") || args.Contains("help"))
            {
                Console.WriteLine(@"
Usage: WCA.Web command [args]

Only one command may be used at a time. After the command has executed, the web server will NOT be started.

Commands:
  initsecurity               Initialise security configuration. E.g. creates security roles and other dependencies.
  listusers                  Lists users currently in the system.
  listroles                  Lists all available roles.
  addrole [user] [role]      Adds the specified user to the specified role. Use 'email' for user, obtained from 'listusers'.
  removerole [user] [role]   Removes the specified user from the specified role. Use 'email' for user, obtained from 'listusers'.
  dropdb                     Drop the databases associated with the contexts associated with this project.
  migratedb                  Run entity framework migrations on all contexts associated with this project.
");
            }

            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            using (var scope = services.CreateScope())
            using (var wCADbContext = scope.ServiceProvider.GetRequiredService<WCADbContext>())
            using (var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WCAUser>>())
            using (var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>())
            {
                switch (args[0])
                {
                    case "initsecurity":
                        var securityInitialiserLogger = scope.ServiceProvider.GetRequiredService<ILogger<SecurityInitialiser>>();
                        using (var securityInitialiser = new SecurityInitialiser(securityInitialiserLogger, userManager, roleManager))
                        {
                            await securityInitialiser.EnsureRoles();
                        }
                        break;

                    case "listusers":
                        LogWcaCliMessage(ConsoleColor.Green, "listusers", "Listing users:");
                        foreach (var userToList in userManager.Users)
                        {
                            LogWcaCliMessage(ConsoleColor.Green, "listusers", $"    {userToList.Email}");
                            foreach (var roleForUser in await userManager.GetRolesAsync(userToList))
                            {
                                LogWcaCliMessage(ConsoleColor.Green, "listusers", $"        - In role: {roleForUser}");
                            }
                        }
                        break;

                    case "listroles":
                        foreach (var roleToList in roleManager.Roles)
                        {
                            LogWcaCliMessage(ConsoleColor.Green, "listroles", $"    {roleToList}");
                        }
                        break;

                    case "addrole":
                        var userEmail = args[1];
                        var role = args[2];
                        var user = await userManager.FindByEmailAsync(userEmail);
                        if (user is null)
                        {
                            var identityResult = await userManager.CreateAsync(new WCAUser() { Email = userEmail, UserName = userEmail });
                            user = await userManager.FindByEmailAsync(userEmail);
                        }
                        LogWcaCliMessage(ConsoleColor.Green, "addrole", $"Adding '{userEmail}' to role '{role}'");
                        await userManager.AddToRoleAsync(user, role);
                        break;

                    case "removerole":
                        var userEmailToRemove = args[1];
                        var roleToRemove = args[2];
                        var userToRemoveFromRole = await userManager.FindByEmailAsync(userEmailToRemove);
                        LogWcaCliMessage(ConsoleColor.Green, "removerole", $"Removing '{userEmailToRemove}' from role '{roleToRemove}'");
                        await userManager.RemoveFromRoleAsync(userToRemoveFromRole, roleToRemove);
                        break;

                    case "dropdb":
                        LogWcaCliMessage(ConsoleColor.Red, "EF", "DATABASE WILL BE DELETED - ARE YOU SURE? (type DELETE to continue)");
                        if (Console.ReadLine() == "DELETE")
                        {
                            LogWcaCliMessage(ConsoleColor.Red, "EF", "Dropping database(s)");
                            wCADbContext.Database.EnsureDeleted();
                        }
                        else
                        {
                            LogWcaCliMessage(ConsoleColor.Red, "EF", "Verification failed. Database NOT dropped. (tip: DELETE is case sensitive)");
                        }
                        break;

                    case "migratedb":
                        LogWcaCliMessage(ConsoleColor.Yellow, "EF", "Migrating database");
                        wCADbContext.Database.Migrate();
                        break;

                    default:
                        LogWcaCliMessage(ConsoleColor.Red, null, "Invalid command line argument");
                        break;
                }
            }
        }

        private static void LogWcaCliMessage(ConsoleColor color, string area, string message)
        {
            Console.ForegroundColor = color;
            Console.Write("WCA");

            if (!string.IsNullOrEmpty(area))
            {
                Console.Write(" ");
                Console.Write(area);
            }

            Console.ResetColor();
            Console.Write(": ");
            Console.WriteLine(message);
        }
    }
}