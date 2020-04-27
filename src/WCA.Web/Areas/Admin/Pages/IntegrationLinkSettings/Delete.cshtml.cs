using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationLinkSettings
{
    public class DeleteModel : PageModel
    {
        private readonly WCADbContext _wcaDbContext;

        public DeleteModel(WCADbContext context)
        {
            _wcaDbContext = context;
        }

        [BindProperty]
        public IntegrationLinkSetting IntegrationLinkSetting { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IntegrationLinkSetting = await _wcaDbContext.IntegrationLinkSettings
                .Include(i => i.ActionstepOrg)
                .Include(i => i.IntegrationLink)
                .Include(i => i.User).FirstOrDefaultAsync(m => m.Id == id);

            if (IntegrationLinkSetting == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IntegrationLinkSetting = await _wcaDbContext.IntegrationLinkSettings.FindAsync(id);

            if (IntegrationLinkSetting != null)
            {
                _wcaDbContext.IntegrationLinkSettings.Remove(IntegrationLinkSetting);
                await _wcaDbContext.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
