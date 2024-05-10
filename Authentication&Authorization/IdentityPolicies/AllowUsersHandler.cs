using Microsoft.AspNetCore.Authorization;

namespace Authentication_Authorization.IdentityPolicies
{
    public class AllowUsersHandler : AuthorizationHandler<AllowUserPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowUserPolicy requirement)
        {
            if(requirement.AllowUsers.Any(user => user.Equals("bojan")))
            {
                context.Succeed(requirement);
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
