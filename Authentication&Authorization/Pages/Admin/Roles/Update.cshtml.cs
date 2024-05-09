using Authentication_Authorization.Data;
using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages.Admin.Roles
{
    public class UpdateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<ApplicationUser> Members { get; set; }
        public List<ApplicationUser> NonMembers { get; set; }

        [BindProperty]
        public IdentityRole Role { get; set; }

        [BindProperty]
        public string[] AddIds { get; set; }

        [BindProperty]
        public string[] RemoveIds { get; set; }

        public UpdateModel(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet(string id) 
        {
            Role = await _roleManager.FindByIdAsync(id);
            Members = new List<ApplicationUser>();
            NonMembers = new List<ApplicationUser>();
            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, Role.Name) ? Members : NonMembers;
                list.Add(user);
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(string id, string roleName, string roleId)
        {
            IdentityResult result;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Model state is invalid!");
                return Page();
            }

            foreach(var userId in AddIds ?? new string[] { })
            {
                var user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.AddToRoleAsync(user, roleName);
                if(!result.Succeeded)
                {
                    ModelState.AddModelError("", $"Unable to add user {user.UserName} to role {roleName}");
                }
            }

            foreach(var userId in RemoveIds ?? new string[] { })
            {
                var user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", $"Unable to remove user {user.UserName} from role {roleName}");
                }
            }
            return RedirectToPage("Update", new { id = Role.Id });
        }
    }
}
