using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationLinkSettings
{
    public class EditModel : PageModel
    {
        private readonly WCADbContext _wcaDbContext;

        public EditModel(WCADbContext context)
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
            ViewData["ActionstepOrgKey"] = new SelectList(_wcaDbContext.ActionstepOrgs.OrderBy(u => u.Key), "Key", "Key");
            ViewData["IntegrationLinkId"] = new SelectList(_wcaDbContext.IntegrationLinks.OrderBy(u => u.Title), "Id", "Title");
            ViewData["UserId"] = new SelectList(_wcaDbContext.Users.OrderBy(u => u.Email), "Id", "Email");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _wcaDbContext.Attach(IntegrationLinkSetting).State = EntityState.Modified;

            try
            {
                await _wcaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntegrationLinkSettingExists(IntegrationLinkSetting.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IntegrationLinkSettingExists(Guid id)
        {
            return _wcaDbContext.IntegrationLinkSettings.Any(e => e.Id == id);
        }
    }
}
