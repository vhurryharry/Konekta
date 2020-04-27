using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationSettings
{
    public class IndexModel : PageModel
    {
        private readonly WCADbContext _wcaDbContext;

        public IndexModel(WCADbContext context)
        {
            _wcaDbContext = context;
        }

        public IList<IntegrationSetting> IntegrationSetting { get; set; }

        public async Task OnGetAsync()
        {
            IntegrationSetting = await _wcaDbContext.IntegrationSettings
                .Include(i => i.ActionstepOrg)
                .Include(i => i.Integration)
                .Include(i => i.User).ToListAsync();
        }
    }
}
