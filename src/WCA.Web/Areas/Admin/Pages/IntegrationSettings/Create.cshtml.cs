using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationSettings
{
    public class CreateModel : PageModel
    {
        private readonly WCADbContext _wcaDbContext;

        public CreateModel(WCADbContext context)
        {
            _wcaDbContext = context;
        }

        public IActionResult OnGet()
        {
            ViewData["ActionstepOrgKey"] = new SelectList(_wcaDbContext.ActionstepOrgs.OrderBy(u => u.Key), "Key", "Key");
            ViewData["IntegrationId"] = new SelectList(_wcaDbContext.Integrations.OrderBy(u => u.Title), "Id", "Title");
            ViewData["UserId"] = new SelectList(_wcaDbContext.Users.OrderBy(u => u.Email), "Id", "Email");
            return Page();
        }

        [BindProperty]
        public IntegrationSetting IntegrationSetting { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _wcaDbContext.IntegrationSettings.Add(IntegrationSetting);
            await _wcaDbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
