using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WCA.Data;

namespace WCA.Web.Extensions
{
    public static class DataExtensions
    {
        public static void MigrateDatabasesIfRequired(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new System.ArgumentNullException(nameof(app));
            }

            var services = (IServiceScopeFactory)app.ApplicationServices.GetService(typeof(IServiceScopeFactory));

            using (var scope = services.CreateScope())
            {
                var wcaDb = scope.ServiceProvider.GetRequiredService<WCADbContext>();

                wcaDb.Database.Migrate();
            }
        }
    }
}
