using Authentication_Authorization.Data;
using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages.Admin
{
    public class UpdateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        [BindProperty]
        public UserUpdateDto UserUpdateDto { get; set; }

        public UpdateModel(ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _context = applicationDbContext;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }
        public async Task<IActionResult> OnGet(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                UserUpdateDto = new UserUpdateDto()
                {
                    Id = id,
                    Username = user.UserName,
                    Email = user.Email,
                };
            }
            else
            {
                UserUpdateDto = new UserUpdateDto();
                ModelState.AddModelError("", "User could not be fetched from database!");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(string id, string newEmail, string newPassword)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(newEmail) && !newEmail.Equals(user.Email))
                    user.Email = newEmail;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(newPassword))
                    user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToPage("Index");
                else
                    ModelState.AddModelError("", "Update was not successful!");

            }
            else
                ModelState.AddModelError("", "User Not Found");
            //return RedirectToPage("Update", new { id = id });
            return Page();
        }
    }
}
