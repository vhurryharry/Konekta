using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;
using WCA.Domain.Models.Account;

namespace WCA.Web.Areas.Admin.Pages.GlobalX.MatterMappings
{
    public class EditModel : PageModel
    {
        private readonly WCADbContext _context;
        private readonly UserManager<WCAUser> _userManager;

        public EditModel(WCADbContext context, UserManager<WCAUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public GlobalXMatterMapping GlobalXMatterMapping { get; set; }

        public async Task<IActionResult> OnGetAsync(string actionstepOrgKey, string globalxMatterId)
        {
            if (string.IsNullOrEmpty(actionstepOrgKey) || string.IsNullOrEmpty(globalxMatterId))
            {
                return NotFound();
            }

            GlobalXMatterMapping = await _context.GlobalXMatterMappings
                .Include(m => m.CreatedBy)
                .Include(m => m.UpdatedBy)
                .FirstOrDefaultAsync(m => m.ActionstepOrgKey == actionstepOrgKey && m.GlobalXMatterId == globalxMatterId);

            if (GlobalXMatterMapping == null)
            {
                return NotFound();
            }

            ViewData["ActionstepOrgKey"] = new SelectList(_context.ActionstepOrgs, "Key", "Key");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            GlobalXMatterMapping.UpdatedBy = await _userManager.GetUserAsync(User);
            _context.Attach(GlobalXMatterMapping).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GlobalXMatterMappingExists(GlobalXMatterMapping.ActionstepOrgKey, GlobalXMatterMapping.GlobalXMatterId))
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

        private bool GlobalXMatterMappingExists(string actionstepOrgKey, string globalxMatterId)
        {
            return _context.GlobalXMatterMappings.Any(e => e.ActionstepOrgKey == actionstepOrgKey && e.GlobalXMatterId == globalxMatterId);
        }
    }
}
