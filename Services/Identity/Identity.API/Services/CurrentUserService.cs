using Identity.Application.Interfaces;
using LiteCommerce.Shared.Authorization;
using System.Diagnostics;

namespace Identity.API.Services
{
    // Scoped: one instance per request, so CorrelationId stays the same for every log row of that request.
    public class CurrentUserService : ICurrentUserService
    {
        public const string SystemUser = "system";
        public const string AnonymousUser = "anonymous";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _correlationId;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Login and register are anonymous requests; the email they are about is stored on the activity row instead.
        public string UserName
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext is null)
                    return SystemUser;

                var user = httpContext.User;
                if (user.Identity?.IsAuthenticated != true)
                    return AnonymousUser;

                return user.FindFirst(AppClaimTypes.Email)?.Value
                    ?? user.FindFirst(AppClaimTypes.Subject)?.Value
                    ?? AnonymousUser;
            }
        }

        // W3C trace id, not HttpContext.TraceIdentifier: Kestrel's "connectionId:requestNo" can repeat after a restart.
        public string CorrelationId => _correlationId ??= Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N");

        // Real client IP: UseForwardedHeaders runs first in the pipeline.
        public string? IpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        public string? UserAgent => _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString() is { Length: > 0 } userAgent
            ? userAgent
            : null;
    }
}
