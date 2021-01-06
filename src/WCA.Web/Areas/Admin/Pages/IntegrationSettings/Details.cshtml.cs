using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationSettings
{
    public class DetailsModel : PageModel
    {
        private readonly WCADbContext _wcaDbContext;

        public DetailsModel(WCADbContext context)
        {
            _wcaDbContext = context;
        }

        public IntegrationSetting IntegrationSetting { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IntegrationSetting = await _wcaDbContext.IntegrationSettings
                .Include(i => i.ActionstepOrg)
                .Include(i => i.Integration)
                .Include(i => i.User).FirstOrDefaultAsync(m => m.Id == id);

            if (IntegrationSetting == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
