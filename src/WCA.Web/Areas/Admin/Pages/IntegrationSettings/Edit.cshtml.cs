using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationSettings
{
    public class EditModel : PageModel
    {
        private readonly WCADbContext _wcaDbContext;

        public EditModel(WCADbContext context)
        {
            _wcaDbContext = context;
        }

        [BindProperty]
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
            ViewData["ActionstepOrgKey"] = new SelectList(_wcaDbContext.ActionstepOrgs.OrderBy(u => u.Key), "Key", "Key");
            ViewData["IntegrationId"] = new SelectList(_wcaDbContext.Integrations.OrderBy(u => u.Title), "Id", "Title");
            ViewData["UserId"] = new SelectList(_wcaDbContext.Users.OrderBy(u => u.Email), "Id", "Email");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _wcaDbContext.Attach(IntegrationSetting).State = EntityState.Modified;

            try
            {
                await _wcaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntegrationSettingExists(IntegrationSetting.Id))
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

        private bool IntegrationSettingExists(Guid id)
        {
            return _wcaDbContext.IntegrationSettings.Any(e => e.Id == id);
        }
    }
}
