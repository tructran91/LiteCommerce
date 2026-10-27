using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LiteCommerce.Shared.Authorization
{
    public static class PermissionServiceCollectionExtensions
    {
        public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
        {
            // Replace, not TryAdd: this must win regardless of whether AddAuthorization
            // (which registers the default provider via TryAdd) ran first.
            services.Replace(ServiceDescriptor.Singleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>());

            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IAuthorizationHandler, PermissionAuthorizationHandler>());

            return services;
        }
    }
}
