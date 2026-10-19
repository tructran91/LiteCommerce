using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Auth;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Auth
{
    public partial class ChangePassword
    {
        [Inject] private AuthService AuthService { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private readonly ChangePasswordFormModel _form = new();
        private bool _submitting;
        private bool _showCurrent;
        private bool _showNew;
        private string? _errorMessage;

        private async Task SubmitAsync()
        {
            _submitting = true;
            _errorMessage = null;

            try
            {
                var request = new ChangePasswordRequest
                {
                    CurrentPassword = _form.CurrentPassword,
                    NewPassword = _form.NewPassword,
                    ConfirmPassword = _form.ConfirmPassword
                };

                var outcome = await AuthService.ChangePasswordAsync(request);

                if (!outcome.IsSuccess)
                {
                    _errorMessage = outcome.ErrorMessage ?? SystemMessages.ErrorOccurred;
                    return;
                }

                // The tokens are already cleared, so this page is no longer authorized.
                Snackbar.Add(SystemMessages.ChangePasswordSuccess, Severity.Success);
                Navigation.NavigateTo("/login", replace: true);
            }
            catch (Exception)
            {
                _errorMessage = SystemMessages.LoginServerUnreachable;
            }
            finally
            {
                _submitting = false;
                StateHasChanged();
            }
        }
    }
}
