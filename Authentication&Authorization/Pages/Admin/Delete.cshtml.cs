using Authentication_Authorization.Data;
using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public DeleteModel(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet(string id)
        {
            return await OnPost(id);
        }

        public async Task<IActionResult> OnPost(string id) 
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                ModelState.AddModelError("", "User not found!");
                return NotFound();
            }
            else
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToPage("Index"); 
        }
    }
}
