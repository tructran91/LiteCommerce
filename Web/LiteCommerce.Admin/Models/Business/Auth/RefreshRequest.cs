namespace LiteCommerce.Admin.Models.Business.Auth
{
    public record RefreshRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
