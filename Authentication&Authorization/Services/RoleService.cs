using Microsoft.AspNetCore.Identity;

namespace Authentication_Authorization.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager) 
        {
            _roleManager = roleManager;

        }
    }
}
