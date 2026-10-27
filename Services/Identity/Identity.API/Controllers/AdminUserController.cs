using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using LiteCommerce.Shared.Authorization;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [Route("api/admin/users")]
    [Authorize]
    public class AdminUserController : BaseApiController
    {
        private readonly IAdminUserService _users;

        public AdminUserController(IAdminUserService users)
        {
            _users = users;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<AdminUserResponse>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [HasPermission(AppPermissions.User.View)]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query, CancellationToken cancellationToken)
            => ToActionResult(await _users.GetUsersAsync(query, cancellationToken));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 404)]
        [HasPermission(AppPermissions.User.View)]
        public async Task<IActionResult> GetUser(string id, CancellationToken cancellationToken)
            => ToActionResult(await _users.GetUserAsync(id, cancellationToken));

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 201)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 404)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 403)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 409)]
        [HasPermission(AppPermissions.User.Create)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
            => TryGetUserId(out var actorId)
                ? ToActionResult(await _users.CreateUserAsync(actorId, request, cancellationToken))
                : Unauthorized();

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 404)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 409)]
        [HasPermission(AppPermissions.User.Update)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
            => ToActionResult(await _users.UpdateUserAsync(id, request, cancellationToken));

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 404)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 409)]
        [HasPermission(AppPermissions.User.Delete)]
        public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken)
            => TryGetUserId(out var actorId)
                ? ToActionResult(await _users.DeleteUserAsync(actorId, id, cancellationToken))
                : Unauthorized();

        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 404)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 409)]
        [HasPermission(AppPermissions.User.UpdateStatus)]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateUserStatusRequest request, CancellationToken cancellationToken)
            => TryGetUserId(out var actorId)
                ? ToActionResult(await _users.UpdateStatusAsync(actorId, id, request, cancellationToken))
                : Unauthorized();

        [HttpPut("{id}/role")]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 404)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 403)]
        [ProducesResponseType(typeof(BaseResponse<AdminUserResponse>), 409)]
        [HasPermission(AppPermissions.User.UpdateRole)]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateUserRoleRequest request, CancellationToken cancellationToken)
            => TryGetUserId(out var actorId)
                ? ToActionResult(await _users.UpdateRoleAsync(actorId, id, request, cancellationToken))
                : Unauthorized();

        [HttpPost("{id}/unlock")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 404)]
        [HasPermission(AppPermissions.User.Unlock)]
        public async Task<IActionResult> Unlock(string id, CancellationToken cancellationToken)
            => ToActionResult(await _users.UnlockAsync(id, cancellationToken));

        [HttpPost("{id}/revoke-sessions")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 404)]
        [HasPermission(AppPermissions.User.RevokeSessions)]
        public async Task<IActionResult> RevokeSessions(string id, CancellationToken cancellationToken)
            => ToActionResult(await _users.RevokeSessionsAsync(id, cancellationToken));

        private bool TryGetUserId(out Guid userId)
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(sub, out userId);
        }
    }
}
