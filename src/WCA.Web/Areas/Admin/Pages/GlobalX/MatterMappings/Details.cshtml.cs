using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;

namespace WCA.Web.Areas.Admin.Pages.GlobalX.MatterMappings
{
    public class DetailsModel : PageModel
    {
        private readonly WCADbContext _context;

        public DetailsModel(WCADbContext context)
        {
            _context = context;
        }

        public GlobalXMatterMapping GlobalXMatterMapping { get; set; }

        public async Task<IActionResult> OnGetAsync(string actionstepOrgKey, string globalxMatterId)
        {
            if (string.IsNullOrEmpty(actionstepOrgKey) || string.IsNullOrEmpty(globalxMatterId))
            {
                return NotFound();
            }

            GlobalXMatterMapping = await _context.GlobalXMatterMappings
                .AsNoTracking()
                .Include(m => m.CreatedBy)
                .Include(m => m.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ActionstepOrgKey == actionstepOrgKey && m.GlobalXMatterId == globalxMatterId);

            if (GlobalXMatterMapping == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
