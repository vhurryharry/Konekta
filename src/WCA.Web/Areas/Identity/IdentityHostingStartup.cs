using Microsoft.AspNetCore.Hosting;
using System;

[assembly: HostingStartup(typeof(WCA.Web.Areas.Identity.IdentityHostingStartup))]
namespace WCA.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ConfigureServices((context, services) => {
            });
        }
    }
}