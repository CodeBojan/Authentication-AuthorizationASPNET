using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Authentication_Authorization.Pages.Account
{
    [AllowAnonymous]
    public class CallbackModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            var result = await HttpContext.AuthenticateAsync("temp"); //access the temp cookie, decrypt it etc.

            if (!result.Succeeded)
            {
                throw new Exception("External Google Authentication Failed!");
            }

            var externalUser = result.Principal;
            var sub = externalUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var issuer = result.Properties.Items["scheme"];

            //the tuple (subjectid, provider/issuer) is the identifier of the user
            //run my logic

            //first time user vs returning user

            var claims = new List<Claim>()
                {
                    new("sub", sub),
                    new("name", externalUser.FindFirst(ClaimTypes.Name).Value),
                    new("role", "Admin"),
                    new("email", externalUser.FindFirst(ClaimTypes.Email).Value)
                    /*new("department", "sales"),
                    new("status", "senior")*/
                };

            var claimsIdentity = new ClaimsIdentity(claims, "pwd", "name", "role");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);

            await HttpContext.SignOutAsync("temp");

            return Redirect(result.Properties.Items["uru"]);
        }
    }
}
