using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages.Admin.Roles
{
    public class IndexModel : PageModel
    {
        public IEnumerable<IdentityRole> Roles { get; set; }
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnGet()
        {
            Roles = _roleManager.Roles.AsEnumerable<IdentityRole>();
            return Page();
        }
    }
}
