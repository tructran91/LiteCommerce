using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace LiteCommerce.Shared.Authorization
{
    // Builds "perm:*" policies on demand so permissions need no AddPolicy call. Everything
    // else falls through, keeping hand-written and fallback policies working.
    public sealed class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackProvider;

        // GetPolicyAsync runs on every authorized request.
        private readonly ConcurrentDictionary<string, AuthorizationPolicy> _cache = new(StringComparer.Ordinal);

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (HasPermissionAttribute.TryGetPermission(policyName, out var permission))
            {
                var policy = _cache.GetOrAdd(permission, static code =>
                    new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddRequirements(new PermissionRequirement(code))
                        .Build());

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            return _fallbackProvider.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => _fallbackProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => _fallbackProvider.GetFallbackPolicyAsync();
    }
}
