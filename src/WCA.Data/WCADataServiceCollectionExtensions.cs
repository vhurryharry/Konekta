using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WCA.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWCAData(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly.GetName().Name;

            // SQL options.EnableRetryOnFailure(); disabled as changes are required elsewhere to make this work.
            // TODO: enable EnableRetryOnFailure() when possible.

            services.AddDbContext<WCADbContext>(builder =>
                builder.UseSqlServer(connectionString, options => {
                    options.MigrationsAssembly(migrationsAssembly);
                })
            );

            services.AddDbContext<WCADbContextTransient>(builder =>
                builder.UseSqlServer(connectionString, options => {
                    options.MigrationsAssembly(migrationsAssembly);
                }),
                ServiceLifetime.Transient);

            return services;
        }
    }
}
