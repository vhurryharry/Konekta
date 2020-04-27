using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace WCA.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static void UseUtc(this PropertyBuilder<DateTime> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasConversion(p => p, p => DateTime.SpecifyKind(p, DateTimeKind.Utc));
        }

        public static void UseUtc(this PropertyBuilder<DateTime?> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasConversion(p => p, p => DateTime.SpecifyKind(p.Value, DateTimeKind.Utc));
        }
    }
}
