namespace Catalog.Application.Services
{
    public interface ICurrentUserService
    {
        // Email from the access token, or "system" for background work with no request context.
        string UserName { get; }

        // Same value for every log row written while handling one request.
        string CorrelationId { get; }
    }
}
