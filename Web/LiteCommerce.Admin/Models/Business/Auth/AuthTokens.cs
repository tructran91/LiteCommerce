namespace LiteCommerce.Admin.Models.Business.Auth
{
    // What gets persisted to localStorage between sessions.
    public class AuthTokens
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
