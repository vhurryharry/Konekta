using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;
using WCA.Domain.Models.Account;

namespace WCA.Web.Areas.Admin.Pages.GlobalX.MatterMappings
{
    public class CreateModel : PageModel
    {
        private readonly WCADbContext _context;
        private readonly UserManager<WCAUser> _userManager;

        public CreateModel(WCADbContext context, UserManager<WCAUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            ViewData["ActionstepOrgKey"] = new SelectList(_context.ActionstepOrgs, "Key", "Key");
            return Page();
        }

        [BindProperty]
        public GlobalXMatterMapping GlobalXMatterMapping { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            GlobalXMatterMapping.CreatedBy = await _userManager.GetUserAsync(User);
            GlobalXMatterMapping.UpdatedBy = await _userManager.GetUserAsync(User);
            _context.GlobalXMatterMappings.Add(GlobalXMatterMapping);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
