using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.IntegrationLinks
{
    public class DetailsModel : PageModel
    {
        private readonly WCA.Data.WCADbContext _context;

        public DetailsModel(WCA.Data.WCADbContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
