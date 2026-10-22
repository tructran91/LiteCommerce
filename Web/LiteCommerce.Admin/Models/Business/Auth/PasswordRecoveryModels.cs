using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.Auth
{
    public class ForgotPasswordRequest
    {
        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(100)]
        public string NewPassword { get; set; } = string.Empty;

        [Required, Compare(nameof(NewPassword), ErrorMessage = "Confirmation does not match the new password.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
