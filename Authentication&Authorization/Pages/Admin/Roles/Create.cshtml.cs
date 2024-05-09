using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages.Admin.Roles
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public string RoleName { get; set; }
        private readonly RoleManager<IdentityRole>  _roleManager;

        public CreateModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(RoleName));
            if(result.Succeeded)
            {
                return RedirectToPage("Index");
            }
            ModelState.AddModelError("", "Role name already exists");
            return Page();
        }
    }
}
