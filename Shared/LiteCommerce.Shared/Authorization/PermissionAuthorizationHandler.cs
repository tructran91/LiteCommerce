using Microsoft.AspNetCore.Authorization;

namespace LiteCommerce.Shared.Authorization
{
    // Reads the claim instead of calling Identity, so the check stays local and the services
    // stay independent. The cost is that a permission change only takes effect on token
    // reissue, so Identity revokes the sessions of an affected role when its grants change.
    public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated != true)
                return Task.CompletedTask;

            // Normally one claim, but tolerate several so a large grant could be split later.
            var granted = context.User
                .FindAll(AppClaimTypes.Permission)
                .SelectMany(claim => PermissionMatcher.Split(claim.Value));

            if (PermissionMatcher.IsGranted(granted, requirement.Permission))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
