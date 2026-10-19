namespace Identity.Application.Interfaces
{
    // Request context for the audit and activity logs. Implemented in Identity.API (needs HttpContext).
    public interface ICurrentUserService
    {
        // Signed-in email, "anonymous" for an unauthenticated request, "system" outside a request.
        string UserName { get; }

        // Same value for every log row written while handling one request.
        string CorrelationId { get; }

        string? IpAddress { get; }

        string? UserAgent { get; }
    }
}
