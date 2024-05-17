using Authentication_Authorization.Data;
using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Authentication_Authorization.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        [Required]
        public string UserName { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Email is not of valid format!")]
        public string Email { get; set; }

        public CreateModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = UserName,
                    Email = Email,
                };
                IdentityResult result = await _userManager.CreateAsync(user, Password);
                if (result.Succeeded)
                    return RedirectToPage("/Admin/Index");
                else
                {
                    foreach(IdentityError error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
