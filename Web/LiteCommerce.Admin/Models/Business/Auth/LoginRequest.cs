namespace LiteCommerce.Admin.Models.Business.Auth
{
    public record LoginRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
