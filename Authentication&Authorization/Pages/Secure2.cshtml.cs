using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages
{

    [Authorize(Policy = "AllowBojan")]
    public class Secure2Model : PageModel
    {
        public void OnGet()
        {
        }
    }
}
