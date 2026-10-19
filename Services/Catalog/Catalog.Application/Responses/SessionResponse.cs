namespace Catalog.Application.Responses
{
    public record SessionResponse
    {
        public string UserName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
