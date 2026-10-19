namespace LiteCommerce.Admin.Models.Business.Auth
{
    public record AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string TokenType { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }
    }
}
