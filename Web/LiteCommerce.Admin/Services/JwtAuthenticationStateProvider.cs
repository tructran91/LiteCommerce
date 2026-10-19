using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace LiteCommerce.Admin.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly AuthenticationState Anonymous =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        private readonly TokenStorageService _tokenStorage;

        public JwtAuthenticationStateProvider(TokenStorageService tokenStorage)
        {
            _tokenStorage = tokenStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokens = await _tokenStorage.GetAsync();

            if (tokens is null || string.IsNullOrWhiteSpace(tokens.AccessToken))
                return Anonymous;

            // An expired access token reports anonymous even when the refresh token is
            // still live; AuthHeaderHandler refreshes on the next call and notifies here.
            if (JwtParser.IsExpired(tokens.AccessToken, TimeSpan.Zero))
                return Anonymous;

            return new AuthenticationState(BuildPrincipal(tokens.AccessToken));
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
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
