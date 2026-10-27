using Identity.Application.Interfaces;
using Identity.Application.Options;
using Identity.Core.Constants;
using Identity.Core.Entities;
using Identity.Infrastructure.Security;
using LiteCommerce.Shared.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Identity.Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        // Short claim name. ClaimTypes.Role would be written to the token as the full
        // WS-Federation URI, which every verifier would then have to special-case.
        public const string RoleClaimType = "role";

        private readonly JwtOptions _options;
        private readonly RsaSecurityKey _signingKey;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;

            _signingKey = RsaKeyLoader.Load(_options.PrivateKeyPath, "Private");
        }

        public (string AccessToken, DateTime Expires) CreateAccessToken(User user, IReadOnlyCollection<string> permissions)
        {
            var expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(RoleClaimType, user.Role?.Name ?? IdentityRoles.Customer),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Omitted entirely when empty, so a storefront token carries no permission claim.
            if (permissions.Count > 0)
                claims.Add(new Claim(AppClaimTypes.Permission, PermissionMatcher.Join(permissions)));

            var handler = new JwtSecurityTokenHandler();
            // Keep sub/email/role as-is instead of expanding them into the long URIs.
            handler.OutboundClaimTypeMap.Clear();

            var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256);
            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                expires: expires,
                signingCredentials: credentials);

            return (handler.WriteToken(token), expires);
        }

        public string CreateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
