using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business.Auth;
using Microsoft.AspNetCore.Components;

namespace LiteCommerce.Admin.Pages.Auth
{
    public partial class ResetPassword
    {
        [Inject] private IPasswordRecoveryApi PasswordRecoveryApi { get; set; } = null!;
        [SupplyParameterFromQuery(Name = "token")] public string? Token { get; set; }
        private readonly ResetPasswordRequest _form = new();
        private bool _submitting;
        private bool _showPassword;
        private bool _success;
        private string? _errorMessage;

        protected override void OnParametersSet()
        {
            if (Token is not null)
                Token = Token.Replace(' ', '+');
        }

        private async Task SubmitAsync()
        {
            if (string.IsNullOrWhiteSpace(Token)) return;
            _submitting = true; _errorMessage = null;
            try
            {
                var result = await PasswordRecoveryApi.ResetPasswordAsync(new() { Token = Token, NewPassword = _form.NewPassword, ConfirmPassword = _form.ConfirmPassword });
                if (!result.IsSuccess) { _errorMessage = result.GetErrorMessage(L["Auth_ResetLinkInvalidOrExpired"]); return; }
                _success = true;
            }
            catch { _errorMessage = L["Common_LoginServerUnreachable"]; }
            finally { _submitting = false; StateHasChanged(); }
        }
    }
}
