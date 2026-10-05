using Identity.Application.Interfaces;
using Identity.Application.Options;
using Identity.Core.Constants;
using Identity.Core.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Identity.Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtOptions _options;
        private readonly RsaSecurityKey _signingKey;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;

            if (string.IsNullOrWhiteSpace(_options.PrivateKeyPath) || !File.Exists(_options.PrivateKeyPath))
                throw new InvalidOperationException(
                    $"RSA private key not found at '{_options.PrivateKeyPath}'. Check Jwt:PrivateKeyPath configuration.");

            var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText(_options.PrivateKeyPath));
            _signingKey = new RsaSecurityKey(rsa) { KeyId = _options.KeyId };
        }

        public (string AccessToken, DateTime Expires) CreateAccessToken(User user)
        {
            var expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? IdentityRoles.Customer),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256);
            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                expires: expires,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        public string CreateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
