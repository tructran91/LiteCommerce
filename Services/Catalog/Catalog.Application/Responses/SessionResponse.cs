namespace Catalog.Application.Responses
{
    public record SessionResponse
    {
        public string UserName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        // Still wildcarded, exactly as the token carries them. Match with PermissionMatcher
        // rather than comparing strings.
        public List<string> Permissions { get; set; } = [];
    }
}
