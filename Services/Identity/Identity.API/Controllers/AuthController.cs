using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [Route("api/auth")]
    [EnableRateLimiting("auth")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 201)]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 409)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
            => ToActionResult(await _auth.RegisterAsync(request, cancellationToken));

        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 401)]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 429)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
            => ToActionResult(await _auth.LoginAsync(request, cancellationToken));

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<AuthResponse>), 401)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken cancellationToken)
            => ToActionResult(await _auth.RefreshAsync(request, cancellationToken));

        [HttpPost("revoke")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
        public async Task<IActionResult> Revoke([FromBody] RevokeRequest request, CancellationToken cancellationToken)
            => ToActionResult(await _auth.RevokeAsync(request, cancellationToken));

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(BaseResponse<UserResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UserResponse>), 401)]
        [ProducesResponseType(typeof(BaseResponse<UserResponse>), 404)]
        public async Task<IActionResult> Me(CancellationToken cancellationToken)
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();

            return ToActionResult(await _auth.GetMeAsync(userId, cancellationToken));
        }
    }
}
