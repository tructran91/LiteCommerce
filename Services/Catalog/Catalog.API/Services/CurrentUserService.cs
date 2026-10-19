using Catalog.Application.Services;
using LiteCommerce.Shared.Authorization;
using System.Diagnostics;
using System.Security.Claims;

namespace Catalog.API.Services
{
    // Scoped: one instance per request, so CorrelationId stays the same for every log row of that request.
    public class CurrentUserService : ICurrentUserService
    {
        public const string SystemUser = "system";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _correlationId;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Background work (seeding, startup) has no HttpContext and stays "system".
        public string UserName
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                    return SystemUser;

                return FirstNonEmpty(
                    user.Identity.Name,
                    user.FindFirstValue(AppClaimTypes.Email),
                    user.FindFirstValue(ClaimTypes.Email),
                    user.FindFirstValue(AppClaimTypes.Subject),
                    user.FindFirstValue(ClaimTypes.NameIdentifier)) ?? SystemUser;
            }
        }

        // W3C trace id, not HttpContext.TraceIdentifier: Kestrel's "connectionId:requestNo" can repeat after a restart.
        public string CorrelationId => _correlationId ??= Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N");

        private static string? FirstNonEmpty(params string?[] values)
            => values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
    }
}
