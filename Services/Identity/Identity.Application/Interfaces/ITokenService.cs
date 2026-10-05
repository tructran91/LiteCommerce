using Identity.Core.Entities;

namespace Identity.Application.Interfaces
{
    public interface ITokenService
    {
        (string AccessToken, DateTime Expires) CreateAccessToken(User user);

        string CreateRefreshToken();
    }
}
