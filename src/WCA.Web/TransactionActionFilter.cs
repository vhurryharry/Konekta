using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using WCA.Data;

namespace WCA.Web
{
    public class WCADbContextTransactionFilter : IAsyncActionFilter
    {
        private readonly WCADbContext _wCADbContext;

        public WCADbContextTransactionFilter(WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            try
            {
                _wCADbContext.Database.BeginTransaction();

                if (next != null)
                {
                    await next();
                }

                _wCADbContext.SaveChanges();
                _wCADbContext.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _wCADbContext.Database.RollbackTransaction();
                throw;
            }
        }
    }
}
