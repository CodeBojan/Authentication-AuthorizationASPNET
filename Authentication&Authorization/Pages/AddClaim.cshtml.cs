using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Authentication_Authorization.Pages
{
    [Authorize(Policy = "Admin")]
    public class AddClaimModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public string ClaimName { get; set; }
        [BindProperty]
        public string ClaimValue { get; set; }

        public AddClaimModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    Claim claim = new Claim(ClaimName, ClaimValue);
                    var result = await _userManager.AddClaimAsync(user, claim);
                    if(!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Error while adding claim!");
                    }
                    return RedirectToPage("Secure");
                }
                ModelState.AddModelError("", "User cannot be found!");
            }
            return Page();
        }
    }
}
