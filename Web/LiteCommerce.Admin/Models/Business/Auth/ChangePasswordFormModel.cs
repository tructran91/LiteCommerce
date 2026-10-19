using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.Auth
{
    // Mirrors ChangePasswordRequestValidator on the server, which is the real check.
    public class ChangePasswordFormModel
    {
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "New password must be between 8 and 100 characters")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm the new password")]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirmation does not match the new password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
