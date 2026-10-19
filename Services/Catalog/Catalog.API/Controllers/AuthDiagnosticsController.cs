using Catalog.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;

namespace Catalog.API.Controllers
{
    // Diagnostics for "the signature key was not found": shows the key ids this service
    // resolved from Identity's JWKS next to the key id in a token, so the mismatch is
    // visible instead of guessed. Development only.
    [Route("api/diagnostics/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthDiagnosticsController : ControllerBase
    {
        private readonly IdentitySettings _identity;
        private readonly IOptionsMonitor<JwtBearerOptions> _jwtBearerOptions;
        private readonly IWebHostEnvironment _environment;

        public AuthDiagnosticsController(
            IOptions<IdentitySettings> identity,
            IOptionsMonitor<JwtBearerOptions> jwtBearerOptions,
            IWebHostEnvironment environment)
        {
            _identity = identity.Value;
            _jwtBearerOptions = jwtBearerOptions;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            if (!_environment.IsDevelopment())
                return NotFound();

            var result = new Dictionary<string, object?>
            {
                ["authority"] = _identity.Authority,
                ["expectedIssuer"] = _identity.Issuer,
                ["expectedAudience"] = _identity.Audience,
                ["requireHttpsMetadata"] = _identity.RequireHttpsMetadata
            };

            // Same configuration manager JwtBearer uses, so this reflects what validation sees.
            var options = _jwtBearerOptions.Get(JwtBearerDefaults.AuthenticationScheme);

            try
            {
                var configuration = options.ConfigurationManager is null
                    ? null
                    : await options.ConfigurationManager.GetConfigurationAsync(cancellationToken);

                result["metadataIssuer"] = configuration?.Issuer;
                result["jwksUri"] = configuration?.JwksUri;
                result["resolvedKeyIds"] = configuration?.SigningKeys.Select(x => x.KeyId).ToArray();
            }
            catch (Exception ex)
            {
                result["metadataError"] = $"{ex.GetType().Name}: {ex.Message}";
                result["metadataInnerError"] = ex.InnerException?.Message;
            }

            // Optional: pass a token to compare its header and claims against the above.
            var token = ReadBearerToken();
            if (token is not null)
            {
                try
                {
                    var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    result["tokenKeyId"] = jwt.Header.Kid;
                    result["tokenAlgorithm"] = jwt.Header.Alg;
                    result["tokenIssuer"] = jwt.Issuer;
                    result["tokenAudiences"] = jwt.Audiences.ToArray();
                    result["tokenExpiresUtc"] = jwt.ValidTo;
                }
                catch (Exception ex)
                {
                    result["tokenError"] = $"{ex.GetType().Name}: {ex.Message}";
                }
            }

            return Ok(result);
        }

        private string? ReadBearerToken()
        {
            var header = Request.Headers.Authorization.ToString();

            return header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? header["Bearer ".Length..].Trim()
                : null;
        }
    }
}
