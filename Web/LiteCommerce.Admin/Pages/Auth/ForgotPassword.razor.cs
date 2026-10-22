using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Auth;
using Microsoft.AspNetCore.Components;

namespace LiteCommerce.Admin.Pages.Auth
{
    public partial class ForgotPassword
    {
        [Inject] private IPasswordRecoveryApi PasswordRecoveryApi { get; set; } = null!;
        private readonly ForgotPasswordRequest _form = new();
        private bool _submitting;
        private bool _submitted;
        private string? _errorMessage;

        private async Task SubmitAsync()
        {
            _submitting = true; _errorMessage = null;
            try
            {
                var result = await PasswordRecoveryApi.ForgotPasswordAsync(new() { Email = _form.Email.Trim() });
                if (!result.IsSuccess) { _errorMessage = result.GetErrorMessage(SystemMessages.ErrorOccurred); return; }
                _submitted = true;
            }
            catch { _errorMessage = SystemMessages.LoginServerUnreachable; }
            finally { _submitting = false; StateHasChanged(); }
        }
    }
}
