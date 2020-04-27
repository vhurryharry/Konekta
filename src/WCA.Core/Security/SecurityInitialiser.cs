using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WCA.Domain.Models.Account;

namespace WCA.Core.Security
{
    public class SecurityInitialiser : IDisposable
    {
        private readonly ILogger<SecurityInitialiser> logger;
        private readonly UserManager<WCAUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public SecurityInitialiser(
            //WCADbContext wCADbContext,
            ILogger<SecurityInitialiser> logger,
            UserManager<WCAUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task EnsureRoles()
        {
            foreach (var roleName in Enum.GetNames(typeof(SecurityRoles)))
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    logger.LogInformation($"Ensuring security role '{roleName}': Already exists.");
                }
                else
                {
                    logger.LogInformation($"Ensuring security role '{roleName}': Doesn't exist, creating now.");
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        roleManager?.Dispose();
                    } catch { /* Swallow */ }

                    try
                    {
                        roleManager?.Dispose();
                    } catch { /* Swallow */ }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}