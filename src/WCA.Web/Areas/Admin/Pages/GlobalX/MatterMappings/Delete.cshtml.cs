using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;

namespace WCA.Web.Areas.Admin.Pages.GlobalX.MatterMappings
{
    public class DeleteModel : PageModel
    {
        private readonly WCADbContext _context;

        public DeleteModel(WCADbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GlobalXMatterMapping GlobalXMatterMapping { get; set; }

        public async Task<IActionResult> OnGetAsync(string actionstepOrgKey, string globalxMatterId)
        {
            if (string.IsNullOrEmpty(actionstepOrgKey) || string.IsNullOrEmpty(globalxMatterId))
            {
                return NotFound();
            }

            GlobalXMatterMapping = await _context.GlobalXMatterMappings.FindAsync(actionstepOrgKey, globalxMatterId);

            if (GlobalXMatterMapping == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string actionstepOrgKey, string globalxMatterId)
        {
            if (string.IsNullOrEmpty(actionstepOrgKey) || string.IsNullOrEmpty(globalxMatterId))
            {
                return NotFound();
            }

            GlobalXMatterMapping = await _context.GlobalXMatterMappings.FindAsync(actionstepOrgKey, globalxMatterId);

            if (GlobalXMatterMapping != null)
            {
                _context.GlobalXMatterMappings.Remove(GlobalXMatterMapping);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
