using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.User;
using LiteCommerce.Admin.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Identity.Users
{
    public partial class UserManagement
    {
        [Inject] private IAdminUserApi UserApi { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        private readonly List<BreadcrumbItem> _breadcrumbs = [new("Home", href: "/", icon: Icons.Material.Filled.Home), new("System", href: null, disabled: true), new("User Management", href: null, disabled: true)];
        private readonly int[] _pageSizeOptions = [10, AppConstants.PageSize, 50, 100];
        private MudTable<AdminUserResponse> _table = null!;
        private int _rowsPerPage = AppConstants.PageSize;
        private UserQuery _query = new();
        private UserQuery _appliedQuery = new();
        private List<RoleResponse> _roles = new();
        private bool _operationInProgress;
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await UserApi.GetRolesAsync();
                if (result.IsSuccess) _roles = result.Data ?? new();
                else _errorMessage = result.GetErrorMessage(SystemMessages.ErrorOccurred);
            }
            catch (Exception ex) { _errorMessage = ex.Message; }
        }

        private async Task<TableData<AdminUserResponse>> LoadServerData(TableState state, CancellationToken cancellationToken)
        {
            _errorMessage = null;
            try
            {
                var result = await UserApi.GetUsersAsync(state.Page + 1, state.PageSize, Trim(_appliedQuery.Search), _appliedQuery.Role, _appliedQuery.IsActive, _appliedQuery.IsLocked);
                if (result.IsSuccess)
                    return new() { Items = result.Data ?? new(), TotalItems = result.Pagination?.TotalRecords ?? 0 };
                _errorMessage = result.GetErrorMessage(SystemMessages.ErrorOccurred);
            }
            catch (Exception ex) { _errorMessage = ex.Message; }
            if (_errorMessage is not null) Snackbar.Add(_errorMessage, Severity.Error);
            StateHasChanged();
            return new() { Items = [], TotalItems = 0 };
        }

        private Task ReloadCurrentPage() => _table.ReloadServerData();
        private async Task ApplyFilter() { _appliedQuery = Copy(_query); await ReloadFirst(); }
        private async Task ClearFilter() { _query = new(); _appliedQuery = new(); await ReloadFirst(); }
        private async Task OnSearchKeyUp(KeyboardEventArgs e) { if (e.Key == "Enter") await ApplyFilter(); }
        private async Task ReloadFirst() { if (_table.CurrentPage == 0) await _table.ReloadServerData(); else _table.NavigateTo(Page.First); }

        private async Task OpenUserDialog(bool isEdit, AdminUserResponse? user = null)
        {
            var parameters = new DialogParameters<UserDialog> { { x => x.IsEdit, isEdit }, { x => x.User, user }, { x => x.Roles, _roles } };
            var dialog = await DialogService.ShowAsync<UserDialog>(string.Empty, parameters, DialogOptions());
            var result = await dialog.Result;
            if (result is null || result.Canceled || result.Data is not UserDialog.UserDialogResult data) return;

            await RunAction(async () => isEdit && user is not null
                ? await UserApi.UpdateUserAsync(user.Id.ToString(), new UpdateUserRequest { FullName = data.FullName })
                : await UserApi.CreateUserAsync(new CreateUserRequest { Email = data.Email, FullName = data.FullName, Password = data.Password, RoleId = data.RoleId, IsActive = data.IsActive }),
                isEdit ? "User updated." : "User created.");
        }

        private async Task OpenRoleDialog(AdminUserResponse user)
        {
            var parameters = new DialogParameters<RoleDialog> { { x => x.CurrentRoleId, user.RoleId }, { x => x.Roles, _roles } };
            var dialog = await DialogService.ShowAsync<RoleDialog>(string.Empty, parameters, DialogOptions());
            var result = await dialog.Result;
            if (result is null || result.Canceled || result.Data is not Guid roleId || roleId == user.RoleId) return;
            await RunAction(() => UserApi.UpdateRoleAsync(user.Id.ToString(), new(roleId)), "Role changed. Existing sessions were revoked.");
        }

        private async Task ToggleStatus(AdminUserResponse user)
        {
            var activating = !user.IsActive;
            if (!await Confirm(activating ? "Activate user" : "Deactivate user", $"{(activating ? "Activate" : "Deactivate")} {user.Email}?", activating ? null : "The user will be signed out of all devices.", activating ? "Activate" : "Deactivate", activating ? Color.Success : Color.Warning)) return;
            await RunAction(() => UserApi.UpdateStatusAsync(user.Id.ToString(), new(activating)), activating ? "User activated." : "User deactivated.");
        }

        private async Task Unlock(AdminUserResponse user)
        {
            if (!await Confirm("Unlock account", $"Unlock {user.Email}?", null, "Unlock", Color.Primary)) return;
            await RunAction(() => UserApi.UnlockAsync(user.Id.ToString()), "User unlocked.");
        }

        private async Task RevokeSessions(AdminUserResponse user)
        {
            if (!await Confirm("Revoke sessions", $"Sign {user.Email} out of all devices?", "Existing access tokens remain valid until they expire; refresh tokens are revoked immediately.", "Revoke sessions", Color.Warning)) return;
            await RunAction(() => UserApi.RevokeSessionsAsync(user.Id.ToString()), "All sessions revoked.");
        }

        private async Task DeleteUser(AdminUserResponse user)
        {
            if (!await Confirm("Delete user", $"Delete {user.Email}?", "This action cannot be undone.", "Delete", Color.Error)) return;
            await RunAction(() => UserApi.DeleteUserAsync(user.Id.ToString()), "User deleted.");
        }

        private async Task RunAction<T>(Func<Task<Models.Common.BaseResponse<T>>> action, string success)
        {
            _operationInProgress = true;
            try
            {
                var result = await action();
                if (!result.IsSuccess) { Snackbar.Add(result.GetErrorMessage(SystemMessages.ErrorOccurred), Severity.Error); return; }
                Snackbar.Add(success, Severity.Success);
                await _table.ReloadServerData();
            }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }
            finally { _operationInProgress = false; StateHasChanged(); }
        }

        private async Task<bool> Confirm(string title, string message, string? details, string confirmText, Color color)
        {
            var parameters = new DialogParameters<ConfirmActionDialog> { { x => x.Title, title }, { x => x.Message, message }, { x => x.Details, details }, { x => x.ConfirmText, confirmText }, { x => x.Color, color } };
            var dialog = await DialogService.ShowAsync<ConfirmActionDialog>(string.Empty, parameters, new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, CloseOnEscapeKey = true });
            var result = await dialog.Result;
            return result is not null && !result.Canceled;
        }

        private static DialogOptions DialogOptions() => new() { MaxWidth = MaxWidth.Small, FullWidth = true, CloseOnEscapeKey = true, BackdropClick = false };
        private static UserQuery Copy(UserQuery q) => new() { Search = q.Search, Role = q.Role, IsActive = q.IsActive, IsLocked = q.IsLocked };
        private static string? Trim(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        private static string Initial(string name) => string.IsNullOrWhiteSpace(name) ? "?" : name[..1].ToUpperInvariant();
        private static Color RoleColor(string role) => role switch { "Admin" => Color.Error, "Staff" => Color.Info, _ => Color.Default };
    }
}
