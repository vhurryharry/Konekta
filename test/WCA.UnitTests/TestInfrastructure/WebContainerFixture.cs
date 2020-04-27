using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using System;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Web;

namespace WCA.UnitTests.TestInfrastructure
{
    public class WebContainerFixture : IDisposable
    {
        private readonly Checkpoint _checkpoint;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHost _host;
        private readonly string _connectionString;

        public WebContainerFixture()
        {
            _host = Program.CreateHostBuilder(null).Build();

            var configuration = _host.Services.GetService<IConfiguration>();
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            _scopeFactory = _host.Services.GetService<IServiceScopeFactory>();
            _checkpoint = new Checkpoint()
            {
                TablesToIgnore = new[]
                {
                    "__EFMigrationsHistory",
                    "AspNetRoleClaims",
                    "AspNetRoles",
                    "AspNetUserClaims",
                    "AspNetUserLogins",
                    "AspNetUserRoles",
                    "AspNetUsers"
                }
            };


            using (var scope = _scopeFactory.CreateScope())
            using (var wcaDb = scope.ServiceProvider.GetRequiredService<WCADbContext>())
            {
                wcaDb.Database.Migrate();
                wcaDb.SeedTestUsers(scope.ServiceProvider).Wait();
                ResetCheckpoint();
            }
        }

        public void ResetCheckpoint()
        {
            _checkpoint.Reset(_connectionString);
        }

        public void ExecuteScope(Action<IServiceProvider> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<WCADbContext>();

                try
                {
                    dbContext.Database.BeginTransaction();

                    action(scope.ServiceProvider);

                    dbContext.SaveChanges();
                    dbContext.Database.CommitTransaction();
                }
                catch (Exception)
                {
                    dbContext.Database.CurrentTransaction?.Rollback();
                    throw;
                }
            }
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<WCADbContext>();

                try
                {
                    dbContext.Database.BeginTransaction();

                    if (action != null)
                    {
                        await action(scope.ServiceProvider);
                    }

                    dbContext.SaveChanges();
                    dbContext.Database.CommitTransaction();
                }
                catch (Exception)
                {
                    dbContext.Database.CurrentTransaction?.Rollback();
                    throw;
                }
            }
        }

        public async Task ExecuteDbContextAsync(Func<WCADbContext, Task> action)
        {
            await ExecuteScopeAsync(sp => action(sp.GetService<WCADbContext>()));
        }

        public async Task SendAsync(IRequest request)
        {
            await ExecuteScopeAsync(async sp =>
            {
                var mediator = sp.GetService<IMediator>();
                await mediator.Send(request);
            });
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var response = default(TResponse);
            await ExecuteScopeAsync(async sp =>
            {
                var mediator = sp.GetService<IMediator>();
                response = await mediator.Send(request);
            });
            return response;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _host.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}