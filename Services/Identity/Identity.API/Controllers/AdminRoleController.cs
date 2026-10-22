using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using LiteCommerce.Shared.Authorization;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/admin/roles")]
    [Authorize(Policy = AppPolicies.AdminOnly)]
    public class AdminRoleController : BaseApiController
    {
        private readonly IAdminUserService _users;

        public AdminRoleController(IAdminUserService users)
        {
            _users = users;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<RoleResponse>>), 200)]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
            => ToActionResult(await _users.GetRolesAsync(cancellationToken));
    }
}
