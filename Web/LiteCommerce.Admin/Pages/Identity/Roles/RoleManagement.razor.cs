using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business.Role;
using LiteCommerce.Admin.Services;
using LiteCommerce.Shared.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Identity.Roles
{
    public partial class RoleManagement
    {
        [Inject] private IAdminRoleApi RoleApi { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private PermissionService Permissions { get; set; } = default!;
        [Inject] private Base.DeleteOperationHelper DeleteHelper { get; set; } = default!;

        private List<BreadcrumbItem> _breadcrumbs = new();
        private List<RoleDetailResponse> _roles = new();
        private List<PermissionGroupResponse> _permissionCatalog = new();
        private bool _loading;
        private string? _errorMessage;

        private readonly DialogOptions _dialogOptions = new()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true,
            BackdropClick = false
        };

        private readonly DialogOptions _permissionDialogOptions = new()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true,
            BackdropClick = false
        };

        protected override async Task OnInitializedAsync()
        {
            _breadcrumbs =
            [
                new(L["Common_Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(L["Menu_System"], href: null, disabled: true),
                new(L["Role_Title"], href: null, disabled: true)
            ];

            await LoadData();
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            try
            {
                var result = await RoleApi.GetRolesAsync();
                if (result.IsSuccess)
                {
                    _roles = result.Data ?? new();
                }
                else
                {
                    _errorMessage = result.GetErrorMessage(L["Common_ErrorOccurred"]);
                    Snackbar.Add(_errorMessage, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _loading = false;
                StateHasChanged();
            }
        }

        private async Task OpenRoleDialog(bool isEdit, RoleDetailResponse? role = null)
        {
            var parameters = new DialogParameters
            {
                [nameof(RoleDialog.IsEdit)] = isEdit,
                [nameof(RoleDialog.InitialId)] = isEdit ? role?.Id.ToString() : null,
                [nameof(RoleDialog.InitialName)] = isEdit ? role?.Name ?? string.Empty : string.Empty,
                [nameof(RoleDialog.InitialDescription)] = isEdit ? role?.Description : null
            };

            var dialog = await DialogService.ShowAsync<RoleDialog>("", parameters, _dialogOptions);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not RoleDialog.RoleDialogResult data)
                return;

            _loading = true;

            try
            {
                var response = data.IsEdit
                    ? await RoleApi.UpdateRoleAsync(data.Id!, new UpdateRoleRequest { Name = data.Name, Description = data.Description })
                    : await RoleApi.CreateRoleAsync(new CreateRoleRequest { Name = data.Name, Description = data.Description });

                if (response.IsSuccess)
                {
                    Snackbar.Add(data.IsEdit ? L["Common_UpdateDataSuccess"] : L["Common_AddDataSuccess"], Severity.Success);
                    await LoadData();
                }
                else
                {
                    Snackbar.Add(response.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _loading = false;
                StateHasChanged();
            }
        }

        private async Task OpenPermissionDialog(RoleDetailResponse role)
        {
            if (!await EnsurePermissionCatalogAsync())
                return;

            var parameters = new DialogParameters
            {
                [nameof(RolePermissionDialog.RoleName)] = role.Name,
                [nameof(RolePermissionDialog.Groups)] = _permissionCatalog,
                [nameof(RolePermissionDialog.InitialSelected)] = role.PermissionCodes,
                [nameof(RolePermissionDialog.ReadOnly)] = !await Permissions.HasAsync(AppPermissions.Role.UpdatePermissions)
            };

            var dialog = await DialogService.ShowAsync<RolePermissionDialog>("", parameters, _permissionDialogOptions);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not List<string> selected)
                return;

            _loading = true;

            try
            {
                var response = await RoleApi.UpdatePermissionsAsync(
                    role.Id.ToString(),
                    new UpdateRolePermissionsRequest { PermissionCodes = selected });

                if (response.IsSuccess)
                {
                    Snackbar.Add(L["Role_PermissionsUpdated"], Severity.Success);
                    await LoadData();
                }
                else
                {
                    Snackbar.Add(response.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _loading = false;
                StateHasChanged();
            }
        }

        // Fetched once: the catalog only changes when the API is redeployed.
        private async Task<bool> EnsurePermissionCatalogAsync()
        {
            if (_permissionCatalog.Count > 0)
                return true;

            try
            {
                var result = await RoleApi.GetPermissionCatalogAsync();
                if (result.IsSuccess)
                {
                    _permissionCatalog = result.Data ?? new();
                    return true;
                }

                Snackbar.Add(result.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error);
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }

            return false;
        }

        private async Task OpenDeleteConfirm(RoleDetailResponse role)
        {
            _loading = true;

            try
            {
                await DeleteHelper.ExecuteDeleteOperation(
                    role.Id.ToString(),
                    role.Name,
                    RoleApi.DeleteRoleAsync,
                    LoadData);
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _loading = false;
                StateHasChanged();
            }
        }
    }
}
