using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages.Account
{
    public class LoginCallbackModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = ReturnUrl
            };
            return Challenge(props, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
