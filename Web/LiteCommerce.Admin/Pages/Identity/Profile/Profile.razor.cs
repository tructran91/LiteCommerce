using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Profile;
using LiteCommerce.Admin.Services;
using LiteCommerce.Admin.Shared.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Identity.Profile
{
    public partial class Profile
    {
        [Inject] private IProfileApi ProfileApi { get; set; } = null!;
        [Inject] private AuthService AuthService { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private readonly List<BreadcrumbItem> _breadcrumbs = [new("Home", href: "/", icon: Icons.Material.Filled.Home), new("Profile", href: null, disabled: true)];
        private UserProfileResponse? _profile;
        private UpdateProfileRequest _profileForm = new();
        private List<UserSessionResponse> _sessions = new();
        private bool _loading = true;
        private bool _saving;
        private bool _sessionsLoading;
        private bool _sessionOperation;
        private string? _errorMessage;
        private string Initial => string.IsNullOrWhiteSpace(_profile?.FullName) ? "?" : _profile.FullName[..1].ToUpperInvariant();

        protected override Task OnInitializedAsync() => LoadAsync();

        private async Task LoadAsync()
        {
            _loading = true; _errorMessage = null;
            try
            {
                var profileResult = await ProfileApi.GetMeAsync();
                if (!profileResult.IsSuccess || profileResult.Data is null) { _errorMessage = profileResult.GetErrorMessage(SystemMessages.ErrorOccurred); return; }
                _profile = profileResult.Data;
                _profileForm = new() { FullName = _profile.FullName };
                await LoadSessionsAsync();
            }
            catch (Exception ex) { _errorMessage = ex.Message; }
            finally { _loading = false; StateHasChanged(); }
        }

        private async Task LoadSessionsAsync()
        {
            _sessionsLoading = true;
            try
            {
                var result = await ProfileApi.GetSessionsAsync();
                if (result.IsSuccess) _sessions = result.Data ?? new();
                else Snackbar.Add(result.GetErrorMessage(SystemMessages.ErrorOccurred), Severity.Error);
            }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }
            finally { _sessionsLoading = false; }
        }

        private async Task SaveProfileAsync()
        {
            _saving = true; _errorMessage = null;
            try
            {
                var result = await ProfileApi.UpdateMeAsync(new() { FullName = _profileForm.FullName.Trim() });
                if (!result.IsSuccess || result.Data is null) { _errorMessage = result.GetErrorMessage(SystemMessages.ErrorOccurred); return; }
                _profile = result.Data;
                _profileForm.FullName = _profile.FullName;
                Snackbar.Add("Profile updated.", Severity.Success);
            }
            catch (Exception ex) { _errorMessage = ex.Message; }
            finally { _saving = false; StateHasChanged(); }
        }

        private async Task RevokeSessionAsync(UserSessionResponse session)
        {
            if (!await Confirm("Revoke session", $"Revoke the session from {DescribeDevice(session.UserAgent)}?", "The device must sign in again after its access token expires.")) return;
            _sessionOperation = true;
            try
            {
                var result = await ProfileApi.RevokeSessionAsync(session.Id.ToString());
                if (!result.IsSuccess) { Snackbar.Add(result.GetErrorMessage(SystemMessages.ErrorOccurred), Severity.Error); return; }
                Snackbar.Add("Session revoked.", Severity.Success);
                await LoadSessionsAsync();
            }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }
            finally { _sessionOperation = false; StateHasChanged(); }
        }

        private async Task RevokeAllSessionsAsync()
        {
            if (!await Confirm("Revoke all sessions", "Sign out of every device, including this browser?", "You will be redirected to sign in again.")) return;
            _sessionOperation = true;
            try
            {
                var result = await ProfileApi.RevokeAllSessionsAsync();
                if (!result.IsSuccess) { Snackbar.Add(result.GetErrorMessage(SystemMessages.ErrorOccurred), Severity.Error); return; }
                await AuthService.ClearAsync();
                Navigation.NavigateTo("/login", replace: true);
            }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }
            finally { _sessionOperation = false; }
        }

        private async Task<bool> Confirm(string title, string message, string details)
        {
            var parameters = new DialogParameters<ConfirmActionDialog> { { x => x.Title, title }, { x => x.Message, message }, { x => x.Details, details }, { x => x.ConfirmText, "Revoke" }, { x => x.Color, Color.Error }, { x => x.Icon, Icons.Material.Filled.Logout } };
            var dialog = await DialogService.ShowAsync<ConfirmActionDialog>(string.Empty, parameters, new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true });
            var result = await dialog.Result;
            return result is not null && !result.Canceled;
        }

        private static string DescribeDevice(string? agent)
        {
            if (string.IsNullOrWhiteSpace(agent)) return "Unknown device";
            var browser = agent.Contains("Edg/", StringComparison.OrdinalIgnoreCase) ? "Edge" : agent.Contains("Chrome/", StringComparison.OrdinalIgnoreCase) ? "Chrome" : agent.Contains("Firefox/", StringComparison.OrdinalIgnoreCase) ? "Firefox" : agent.Contains("Safari/", StringComparison.OrdinalIgnoreCase) ? "Safari" : "Browser";
            var platform = agent.Contains("Windows", StringComparison.OrdinalIgnoreCase) ? "Windows" : agent.Contains("Android", StringComparison.OrdinalIgnoreCase) ? "Android" : agent.Contains("iPhone", StringComparison.OrdinalIgnoreCase) ? "iPhone" : agent.Contains("Mac OS", StringComparison.OrdinalIgnoreCase) ? "macOS" : "Unknown OS";
            return $"{browser} on {platform}";
        }
    }
}
