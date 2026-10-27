namespace LiteCommerce.Admin.Models.Business.Auth
{
    public record SessionResponse
    {
        public string UserName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public List<string> Permissions { get; set; } = [];
    }
}
