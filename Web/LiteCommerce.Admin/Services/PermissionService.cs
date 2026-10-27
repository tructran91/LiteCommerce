using LiteCommerce.Shared.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace LiteCommerce.Admin.Services
{
    // Reads the "perm" claim to decide what the UI shows. This is presentation only: the
    // APIs enforce the same permissions, so a stale or tampered claim costs a 403, not access.
    public class PermissionService
    {
        private readonly AuthenticationStateProvider _authStateProvider;

        // Keyed on the raw claim, so a refreshed token with different grants invalidates it.
        private string? _cachedClaim;
        private string[] _cached = [];

        public PermissionService(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> HasAsync(string permission)
            => PermissionMatcher.IsGranted(await GetGrantedAsync(), permission);

        public async Task<bool> HasAnyAsync(params string[] permissions)
        {
            if (permissions.Length == 0)
                return true;

            var granted = await GetGrantedAsync();
            return permissions.Any(p => PermissionMatcher.IsGranted(granted, p));
        }

        // Still wildcarded. Match with PermissionMatcher rather than comparing strings.
        public async Task<string[]> GetGrantedAsync()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var claim = state.User.FindFirst(AppClaimTypes.Permission)?.Value;

            if (claim == _cachedClaim)
                return _cached;

            _cachedClaim = claim;
            _cached = PermissionMatcher.Split(claim);
            return _cached;
        }
    }
}
