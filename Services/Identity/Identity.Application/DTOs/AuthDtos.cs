namespace Identity.Application.DTOs
{
    public record RegisterRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
    }

    public record LoginRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public record RefreshRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public record RevokeRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public record AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string TokenType { get; set; } = "Bearer";

        public long ExpiresIn { get; set; }
    }

    public record UserResponse
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
