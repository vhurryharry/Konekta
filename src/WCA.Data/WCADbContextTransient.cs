using Microsoft.EntityFrameworkCore;

namespace WCA.Data
{
    /// <summary>
    /// Wrapper just to provide a separate Transient registration in DI.
    /// </summary>
    public class WCADbContextTransient : WCADbContext
    {
        public WCADbContextTransient(DbContextOptions<WCADbContext> options) : base(options)
        {
        }

        public WCADbContextTransient() : base() { }

        /// <summary>
        /// Useful references:
        /// - Value Conversions: https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions
        /// </summary>
        /// <param name="builder"></param>
    }
}
