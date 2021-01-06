using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WCA.Data;
using WCA.Domain.Integrations;

namespace WCA.Web.Areas.Admin.Pages.Integrations
{
    public class DetailsModel : PageModel
    {
        private readonly WCA.Data.WCADbContext _context;

        public DetailsModel(WCA.Data.WCADbContext context)
        {
            _context = context;
        }

        public Integration Integration { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Integration = await _context.Integrations.FirstOrDefaultAsync(m => m.Id == id);

            if (Integration == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
