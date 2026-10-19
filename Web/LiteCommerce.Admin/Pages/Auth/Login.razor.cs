using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Auth;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace LiteCommerce.Admin.Pages.Auth
{
    public partial class Login
    {
        [Inject] private AuthService AuthService { get; set; } = null!;
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        [SupplyParameterFromQuery(Name = "returnUrl")]
        private string? ReturnUrl { get; set; }

        private readonly LoginFormModel _form = new();
        private bool _submitting;
        private bool _showPassword;
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity?.IsAuthenticated == true)
            {
                Navigation.NavigateTo(ResolveRedirectTarget(), replace: true);
            }
        }

        private void TogglePasswordVisibility() => _showPassword = !_showPassword;

        private async Task SubmitAsync()
        {
            _submitting = true;
            _errorMessage = null;

            try
            {
                var outcome = await AuthService.LoginAsync(_form.Email, _form.Password);

                if (!outcome.IsSuccess)
                {
                    _errorMessage = outcome.ErrorMessage ?? SystemMessages.LoginFailed;
                    return;
                }

                Navigation.NavigateTo(ResolveRedirectTarget(), replace: true);
            }
            catch (Exception)
            {
                // ApiClientSettings only suppresses exceptions for JSON bodies, so reaching
                // here means a network or non-JSON failure with no server message to show.
                _errorMessage = SystemMessages.LoginServerUnreachable;
            }
            finally
            {
                _submitting = false;
                StateHasChanged();
            }
        }

        // Relative paths only, so a crafted returnUrl cannot bounce the user off-site.
        private string ResolveRedirectTarget()
        {
            if (string.IsNullOrWhiteSpace(ReturnUrl))
                return "/";

            var target = Uri.UnescapeDataString(ReturnUrl);

            if (!target.StartsWith('/') || target.StartsWith("//"))
                return "/";

            return target;
        }
    }
}
