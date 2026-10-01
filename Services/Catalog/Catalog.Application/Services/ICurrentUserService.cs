namespace Catalog.Application.Services
{
    public interface ICurrentUserService
    {
        // "system" until authentication is added.
        string UserName { get; }

        // Same value for every log row written while handling one request.
        string CorrelationId { get; }
    }
}
