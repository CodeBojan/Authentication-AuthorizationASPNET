using Authentication_Authorization.Data;
using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_Authorization.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        private readonly ApplicationDbContext _applicationDbContext;
        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Users = _applicationDbContext.Users;
        }
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
    }
}
