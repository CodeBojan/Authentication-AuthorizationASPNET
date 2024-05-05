using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages.Account
{
    [AllowAnonymous]
    public class GoogleModel : PageModel
    {
        public IActionResult OnGet(string returnUrl)
        {
            //await HttpContext.ChallengeAsync("Google");

            if (!Url.IsLocalUrl(returnUrl))
            {
                throw new Exception("Url is not local!");
            }

            var properties = new AuthenticationProperties()
            {
                RedirectUri = Url.Page("Callback"),

                Items =
                {
                    { "uru", returnUrl },
                    { "scheme", "Google" }
                }
            };

            return Challenge(properties, "Google");
        }
    }
}
