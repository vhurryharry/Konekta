using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationLinks
{
    public class CreateModel : PageModel
    {
        private readonly WCA.Data.WCADbContext _context;

        public CreateModel(WCA.Data.WCADbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["IntegrationId"] = new SelectList(_context.Integrations, "Id", "Title");
            return Page();
        }

        [BindProperty]
        public IntegrationLink IntegrationLink { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.IntegrationLinks.Add(IntegrationLink);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
