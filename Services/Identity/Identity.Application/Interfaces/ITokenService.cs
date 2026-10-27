using Identity.Core.Entities;

namespace Identity.Application.Interfaces
{
    public interface ITokenService
    {
        // Permissions are passed in rather than looked up: this service is a singleton and
        // holds no DbContext.
        (string AccessToken, DateTime Expires) CreateAccessToken(User user, IReadOnlyCollection<string> permissions);

        string CreateRefreshToken();
    }
}
