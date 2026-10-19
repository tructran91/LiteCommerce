using Identity.Application.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Identity.API.Controllers
{
    // Publishes the RSA public key so other services validate access tokens without a copy
    // of the PEM file. JwtBearer with Authority = this service fetches the discovery
    // document, then jwks_uri, and caches the keys. Rotating keys only means bumping
    // Jwt:KeyId and restarting Identity.
    //
    // Not a BaseApiController: these are well-known OIDC endpoints and must return the raw
    // spec shape, not BaseResponse<T>.
    [ApiController]
    [AllowAnonymous]
    public class DiscoveryController : ControllerBase
    {
        private readonly JwtOptions _jwt;

        public DiscoveryController(IOptions<JwtOptions> jwt)
        {
            _jwt = jwt.Value;
        }

        [HttpGet("/.well-known/openid-configuration")]
        public IActionResult GetConfiguration()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            return Ok(new
            {
                issuer = _jwt.Issuer,
                jwks_uri = $"{baseUrl}/.well-known/jwks.json",
                token_endpoint = $"{baseUrl}/api/auth/login",
                id_token_signing_alg_values_supported = new[] { SecurityAlgorithms.RsaSha256 },
                response_types_supported = new[] { "token" },
                subject_types_supported = new[] { "public" },
                claims_supported = new[] { "sub", "email", "role", "jti", "iss", "aud", "exp" }
            });
        }

        [HttpGet("/.well-known/jwks.json")]
        public IActionResult GetJwks()
        {
            if (string.IsNullOrWhiteSpace(_jwt.PublicKeyPath) || !System.IO.File.Exists(_jwt.PublicKeyPath))
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Signing key is not configured." });

            using var rsa = RSA.Create();
            rsa.ImportFromPem(System.IO.File.ReadAllText(_jwt.PublicKeyPath));
            var parameters = rsa.ExportParameters(includePrivateParameters: false);

            return Ok(new
            {
                keys = new[]
                {
                    new
                    {
                        kty = "RSA",
                        use = "sig",
                        alg = SecurityAlgorithms.RsaSha256,
                        kid = _jwt.KeyId,
                        n = Base64UrlEncoder.Encode(parameters.Modulus),
                        e = Base64UrlEncoder.Encode(parameters.Exponent)
                    }
                }
            });
        }
    }
}
