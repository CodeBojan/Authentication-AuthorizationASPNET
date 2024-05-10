using Microsoft.AspNetCore.Authorization;

namespace Authentication_Authorization.IdentityPolicies
{
    public class AllowUserPolicy : IAuthorizationRequirement
    {
        public string[] AllowUsers { get; set; }

        public AllowUserPolicy(params string[] users)
        {
            AllowUsers = users;
        }
    }
}
