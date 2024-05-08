using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages
{
    [Authorize]
    public class SecureModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
