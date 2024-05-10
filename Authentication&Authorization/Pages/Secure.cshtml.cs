using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages
{
    [Authorize(Policy = "Admin")]
    public class SecureModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
