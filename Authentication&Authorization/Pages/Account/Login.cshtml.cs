using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Authentication_Authorization.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost() 
        {
            if(!string.IsNullOrWhiteSpace(Username) && Username == Password)
            {
                var claims = new List<Claim>()
                {
                    new("sub", "123"),
                    new("name", "Bojan"),
                    new("role", "Admin"),
                    /*new("department", "sales"),
                    new("status", "senior")*/
                };

                var claimsIdentity = new ClaimsIdentity(claims, "pwd", "name", "role");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);
                return LocalRedirect(ReturnUrl);
            }
           
            return Page();
        }    
    }
}
