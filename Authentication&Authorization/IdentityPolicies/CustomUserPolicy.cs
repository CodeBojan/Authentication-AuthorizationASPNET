using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Identity;

namespace Authentication_Authorization.IdentityPolicies
{
    public class CustomUserPolicy : UserValidator<ApplicationUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(manager, user);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            if (user.UserName == "google")
            {
                errors.Add(new IdentityError
                {
                    Description = "Google cannot be used as a username!"
                });
            }

            if (user.Email.ToLower().EndsWith("@dooge.net"))
            {
                errors.Add(new IdentityError
                {
                    Description = "dooge.net email addresses are not allowed!"
                });
            }
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
