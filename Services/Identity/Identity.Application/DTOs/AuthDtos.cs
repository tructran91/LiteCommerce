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

    public record ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public record UpdateProfileRequest
    {
        public string FullName { get; set; } = string.Empty;
    }

    public record ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public record ResetPasswordRequest
    {
        public string Token { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
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

    public record SessionResponse
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime Expires { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }
    }
}
