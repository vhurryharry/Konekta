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
    public class IndexModel : PageModel
    {
        private readonly WCA.Data.WCADbContext _context;

        public IndexModel(WCA.Data.WCADbContext context)
        {
            _context = context;
        }

        public IList<IntegrationLink> IntegrationLink { get;set; }

        public async Task OnGetAsync()
        {
            IntegrationLink = await _context.IntegrationLinks
                .Include(i => i.Integration).ToListAsync();
        }
    }
}
