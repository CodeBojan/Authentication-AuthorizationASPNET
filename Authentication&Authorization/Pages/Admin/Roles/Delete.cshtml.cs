using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages.Admin.Roles
{
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<IdentityResult> _roleManager;

        public DeleteModel(RoleManager<IdentityResult> roleManager)
        {
            _roleManager = roleManager;
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string id)
        {
            var result = await _roleManager.FindByIdAsync(id);
            if (result == null) 
            {
                ModelState.AddModelError("", "Could not find specified role!");
                return RedirectToPage("Index");
            }
            result = await _roleManager.DeleteAsync(result);
            if (result.Succeeded)
                return Page();
            ModelState.AddModelError("", "Could not delete specified role!");
            return RedirectToPage("Index");
        }
    }
}
