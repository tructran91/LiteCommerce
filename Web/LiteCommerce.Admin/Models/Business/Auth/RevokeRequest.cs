namespace LiteCommerce.Admin.Models.Business.Auth
{
    public record RevokeRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
