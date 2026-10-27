using LiteCommerce.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Identity.API.Extensions
{
    public static class EndpointPermissionGuard
    {
        private const string ProtectedPrefix = "api/admin";

        /// <summary>
        /// Fails startup if an api/admin/* action declares no permission. Without it a new
        /// action silently falls back to "any authenticated user", which looks fine in testing
        /// and is a hole in production. There are no tests in this repo, so this is the only
        /// thing standing between a forgotten attribute and an open endpoint.
        /// </summary>
        public static void AssertAdminEndpointsHavePermissions(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<IActionDescriptorCollectionProvider>();
            var unprotected = new List<string>();

            foreach (var action in provider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>())
            {
                var template = action.AttributeRouteInfo?.Template;
                if (template is null || !template.StartsWith(ProtectedPrefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (action.EndpointMetadata.OfType<IAllowAnonymous>().Any())
                    continue;

                if (action.EndpointMetadata.OfType<HasPermissionAttribute>().Any())
                    continue;

                unprotected.Add($"{action.ControllerName}.{action.ActionName} ({template})");
            }

            if (unprotected.Count == 0)
                return;

            throw new InvalidOperationException(
                "These api/admin/* endpoints have no [HasPermission(...)] and would be reachable "
                + "by any authenticated user:" + Environment.NewLine + "  - "
                + string.Join(Environment.NewLine + "  - ", unprotected));
        }
    }
}
