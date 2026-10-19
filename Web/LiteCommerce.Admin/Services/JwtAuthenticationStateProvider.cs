using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace LiteCommerce.Admin.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly AuthenticationState Anonymous =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        private readonly TokenStorageService _tokenStorage;
        private readonly AuthService _authService;

        public JwtAuthenticationStateProvider(TokenStorageService tokenStorage, AuthService authService)
        {
            _tokenStorage = tokenStorage;
            _authService = authService;

            _tokenStorage.TokensChanged += () => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokens = await _tokenStorage.GetAsync();

            if (tokens is null || string.IsNullOrWhiteSpace(tokens.AccessToken))
                return Anonymous;

            var accessToken = tokens.AccessToken;

            // A live refresh token is still a session, so refresh instead of bouncing to login.
            if (JwtParser.IsExpired(accessToken, TimeSpan.Zero))
            {
                var refreshed = await _authService.RefreshAsync();
                if (string.IsNullOrWhiteSpace(refreshed))
                    return Anonymous;

                accessToken = refreshed;
            }

            return new AuthenticationState(BuildPrincipal(accessToken));
        }

        private static ClaimsPrincipal BuildPrincipal(string accessToken)
        {
            // The name/role claim types must match what Identity writes, otherwise
            // AuthorizeView and [Authorize(Roles = ...)] see a nameless, role-less user.
            var identity = new ClaimsIdentity(
                JwtParser.ParseClaims(accessToken),
                authenticationType: "jwt",
                nameType: JwtParser.EmailClaim,
                roleType: JwtParser.RoleClaim);

            return new ClaimsPrincipal(identity);
        }
    }
}
