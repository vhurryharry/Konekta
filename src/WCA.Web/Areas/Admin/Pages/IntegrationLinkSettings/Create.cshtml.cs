using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationLinkSettings
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
            ViewData["IntegrationLinkId"] = new SelectList(_wcaDbContext.IntegrationLinks.OrderBy(u => u.Title), "Id", "Title");
            ViewData["UserId"] = new SelectList(_wcaDbContext.Users.OrderBy(u => u.Email), "Id", "Email");
            return Page();
        }

        [BindProperty]
        public IntegrationLinkSetting IntegrationLinkSetting { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _wcaDbContext.IntegrationLinkSettings.Add(IntegrationLinkSetting);
            await _wcaDbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
