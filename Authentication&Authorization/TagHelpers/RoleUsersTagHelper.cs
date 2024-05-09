using Authentication_Authorization.Data;
using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Authentication_Authorization.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "i-role")]
    public class RoleUsersTagHelper : TagHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleUsersTagHelper(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HtmlAttributeName("i-role")]
        public string RoleId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new();
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
                return;
            foreach(var user in _userManager.Users)
            {
                if (user != null && await _userManager.IsInRoleAsync(user, role.Name))
                    names.Add(user.UserName);
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));

        }
    }
}
