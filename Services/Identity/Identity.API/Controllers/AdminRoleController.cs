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
    [Route("api/admin/roles")]
    [Authorize]
    public class AdminRoleController : BaseApiController
    {
        private readonly IAdminRoleService _roles;

        public AdminRoleController(IAdminRoleService roles)
        {
            _roles = roles;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<RoleDetailResponse>>), 200)]
        [HasPermission(AppPermissions.Role.View)]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
            => ToActionResult(await _roles.GetRolesAsync(cancellationToken));

        // Before "{id}" so the literal segment is not captured as an id.
        [HttpGet("permissions")]
        [ProducesResponseType(typeof(BaseResponse<List<PermissionGroupResponse>>), 200)]
        [HasPermission(AppPermissions.Role.ViewPermissions)]
        public async Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
            => ToActionResult(await _roles.GetPermissionCatalogAsync(cancellationToken));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 404)]
        [HasPermission(AppPermissions.Role.View)]
        public async Task<IActionResult> GetRole(string id, CancellationToken cancellationToken)
            => ToActionResult(await _roles.GetRoleAsync(id, cancellationToken));

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 201)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 409)]
        [HasPermission(AppPermissions.Role.Create)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
            => ToActionResult(await _roles.CreateRoleAsync(request, cancellationToken));

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 404)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 409)]
        [HasPermission(AppPermissions.Role.Update)]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
            => ToActionResult(await _roles.UpdateRoleAsync(id, request, cancellationToken));

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 404)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 409)]
        [HasPermission(AppPermissions.Role.Delete)]
        public async Task<IActionResult> DeleteRole(string id, CancellationToken cancellationToken)
            => ToActionResult(await _roles.DeleteRoleAsync(id, cancellationToken));

        [HttpPut("{id}/permissions")]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 404)]
        [ProducesResponseType(typeof(BaseResponse<RoleDetailResponse>), 409)]
        [HasPermission(AppPermissions.Role.UpdatePermissions)]
        public async Task<IActionResult> UpdatePermissions(
            string id, [FromBody] UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
            => TryGetUserId(out var actorId)
                ? ToActionResult(await _roles.UpdateRolePermissionsAsync(actorId, id, request, cancellationToken))
                : Unauthorized();

        private bool TryGetUserId(out Guid userId)
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(sub, out userId);
        }
    }
}
