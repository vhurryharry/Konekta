using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationLinkSettings
{
    public class IndexModel : PageModel
    {
        private readonly WCADbContext _wcaDbContext;

        public IndexModel(WCADbContext context)
        {
            _wcaDbContext = context;
        }

        public IList<IntegrationLinkSetting> IntegrationLinkSetting { get; set; }

        public async Task OnGetAsync()
        {
            IntegrationLinkSetting = await _wcaDbContext.IntegrationLinkSettings
                .Include(i => i.ActionstepOrg)
                .Include(i => i.IntegrationLink)
                .Include(i => i.User).ToListAsync();
        }
    }
}
