using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;

namespace WCA.Web.Areas.Admin.Pages.GlobalX.MatterMappings
{
    public class IndexModel : PageModel
    {
        private readonly WCADbContext _context;

        public IndexModel(WCADbContext context)
        {
            _context = context;
        }

        public IList<GlobalXMatterMapping> GlobalXMatterMapping { get; set; }

        public async Task OnGetAsync()
        {
            GlobalXMatterMapping = await _context.GlobalXMatterMappings
                .AsNoTracking()
                .Include(m => m.CreatedBy)
                .Include(m => m.UpdatedBy)
                .ToListAsync();
        }
    }
}
