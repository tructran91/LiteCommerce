using Catalog.Application.Services;
using System.Diagnostics;

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

        // No authentication yet, so Identity.Name is always null and every change is logged as "system".
        public string UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? SystemUser;

        // W3C trace id, not HttpContext.TraceIdentifier: Kestrel's "connectionId:requestNo" can repeat after a restart.
        public string CorrelationId => _correlationId ??= Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N");
    }
}
