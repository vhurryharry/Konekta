using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationLinks
{
    public class EditModel : PageModel
    {
        private readonly WCA.Data.WCADbContext _context;

        public EditModel(WCA.Data.WCADbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IntegrationLink IntegrationLink { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IntegrationLink = await _context.IntegrationLinks
                .Include(i => i.Integration).FirstOrDefaultAsync(m => m.Id == id);

            if (IntegrationLink == null)
            {
                return NotFound();
            }
           ViewData["IntegrationId"] = new SelectList(_context.Integrations, "Id", "Title");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(IntegrationLink).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntegrationLinkExists(IntegrationLink.Id))
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

        private bool IntegrationLinkExists(Guid id)
        {
            return _context.IntegrationLinks.Any(e => e.Id == id);
        }
    }
}
