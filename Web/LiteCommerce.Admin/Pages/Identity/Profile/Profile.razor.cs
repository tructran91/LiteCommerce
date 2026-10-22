using LiteCommerce.Admin.ApiClients;
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

        private List<BreadcrumbItem> _breadcrumbs = new();
        private UserProfileResponse? _profile;
        private UpdateProfileRequest _profileForm = new();
        private List<UserSessionResponse> _sessions = new();
        private bool _loading = true;
        private bool _saving;
        private bool _sessionsLoading;
        private bool _sessionOperation;
        private string? _errorMessage;
        private string Initial => string.IsNullOrWhiteSpace(_profile?.FullName) ? "?" : _profile.FullName[..1].ToUpperInvariant();

        protected override Task OnInitializedAsync()
        {
            _breadcrumbs = new()
            {
                new(L["Common_Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(L["User_Profile"], href: null, disabled: true),
            };

            return LoadAsync();
        }

        private async Task LoadAsync()
        {
            _loading = true; _errorMessage = null;
            try
            {
                var profileResult = await ProfileApi.GetMeAsync();
                if (!profileResult.IsSuccess || profileResult.Data is null) { _errorMessage = profileResult.GetErrorMessage(L["Common_ErrorOccurred"]); return; }
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
                else Snackbar.Add(result.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error);
            }
            catch (Exception ex) { Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error); }
            finally { _sessionsLoading = false; }
        }

        private async Task SaveProfileAsync()
        {
            _saving = true; _errorMessage = null;
            try
            {
                var result = await ProfileApi.UpdateMeAsync(new() { FullName = _profileForm.FullName.Trim() });
                if (!result.IsSuccess || result.Data is null) { _errorMessage = result.GetErrorMessage(L["Common_ErrorOccurred"]); return; }
                _profile = result.Data;
                _profileForm.FullName = _profile.FullName;
                Snackbar.Add(L["Profile_Updated"], Severity.Success);
            }
            catch (Exception ex) { _errorMessage = ex.Message; }
            finally { _saving = false; StateHasChanged(); }
        }

        private async Task RevokeSessionAsync(UserSessionResponse session)
        {
            if (!await Confirm(L["Profile_RevokeTitle"], L["Profile_RevokeQuestion", DescribeDevice(session.UserAgent)], L["Profile_RevokeDetails"])) return;
            _sessionOperation = true;
            try
            {
                var result = await ProfileApi.RevokeSessionAsync(session.Id.ToString());
                if (!result.IsSuccess) { Snackbar.Add(result.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error); return; }
                Snackbar.Add(L["Profile_SessionRevoked"], Severity.Success);
                await LoadSessionsAsync();
            }
            catch (Exception ex) { Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error); }
            finally { _sessionOperation = false; StateHasChanged(); }
        }

        private async Task RevokeAllSessionsAsync()
        {
            if (!await Confirm(L["Profile_RevokeAllTitle"], L["Profile_RevokeAllQuestion"], L["Profile_RevokeAllDetails"])) return;
            _sessionOperation = true;
            try
            {
                var result = await ProfileApi.RevokeAllSessionsAsync();
                if (!result.IsSuccess) { Snackbar.Add(result.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error); return; }
                await AuthService.ClearAsync();
                Navigation.NavigateTo("/login", replace: true);
            }
            catch (Exception ex) { Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error); }
            finally { _sessionOperation = false; }
        }

        private async Task<bool> Confirm(string title, string message, string details)
        {
            var parameters = new DialogParameters<ConfirmActionDialog> { { x => x.Title, title }, { x => x.Message, message }, { x => x.Details, details }, { x => x.ConfirmText, L["Profile_Revoke"].Value }, { x => x.Color, Color.Error }, { x => x.Icon, Icons.Material.Filled.Logout } };
            var dialog = await DialogService.ShowAsync<ConfirmActionDialog>(string.Empty, parameters, new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true });
            var result = await dialog.Result;
            return result is not null && !result.Canceled;
        }

        private string DescribeDevice(string? agent)
        {
            if (string.IsNullOrWhiteSpace(agent)) return L["Profile_UnknownDevice"];
            var browser = agent.Contains("Edg/", StringComparison.OrdinalIgnoreCase) ? "Edge" : agent.Contains("Chrome/", StringComparison.OrdinalIgnoreCase) ? "Chrome" : agent.Contains("Firefox/", StringComparison.OrdinalIgnoreCase) ? "Firefox" : agent.Contains("Safari/", StringComparison.OrdinalIgnoreCase) ? "Safari" : L["Profile_Browser"].Value;
            var platform = agent.Contains("Windows", StringComparison.OrdinalIgnoreCase) ? "Windows" : agent.Contains("Android", StringComparison.OrdinalIgnoreCase) ? "Android" : agent.Contains("iPhone", StringComparison.OrdinalIgnoreCase) ? "iPhone" : agent.Contains("Mac OS", StringComparison.OrdinalIgnoreCase) ? "macOS" : L["Profile_UnknownOs"].Value;
            return L["Profile_DeviceFormat", browser, platform];
        }
    }
}
