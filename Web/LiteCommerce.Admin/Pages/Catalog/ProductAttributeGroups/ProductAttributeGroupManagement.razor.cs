using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.ProductAttributeGroups
{
    public partial class ProductAttributeGroupManagement
    {
        [Inject]
        private IProductAttributeGroupApi ProductAttributeGroupApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private DeleteOperationHelper _deleteHelper => new(DialogService, Snackbar, L);

        private List<BreadcrumbItem> _breadcrumbs = new();

        private MudTable<ProductAttributeGroupResponse> _table = null!;
        private List<ProductAttributeGroupResponse> _allGroups = new();
        private bool _loading = false;
        private string? _errorMessage;
        private ProductAttributeGroupQuery _query = new();

        private DialogOptions _dialogOptions = new()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true,
            BackdropClick = false,
        };

        protected override async Task OnInitializedAsync()
        {
            _breadcrumbs = new()
            {
                new(L["Common_Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(L["Menu_Catalog"], href: null, disabled: true),
                new(L["AttributeGroup_Title"], href: null, disabled: true),
            };

            await LoadData();
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            try
            {
                var result = await ProductAttributeGroupApi.GetProductAttributeGroupsAsync(_query.Page, _query.PageSize);
                if (result.IsSuccess)
                {
                    _allGroups = result.Data ?? new();
                }
                else
                {
                    var errorDetails = result.GetErrorMessage(L["Common_ErrorOccurred"]);
                    _errorMessage = errorDetails;
                    Snackbar.Add(errorDetails, Severity.Error);
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

        private void OnSortChanged(SortDirection direction)
        {
            _table.NavigateTo(Page.First);
        }

        private async Task OpenAttributeGroupDialog(bool isEdit, ProductAttributeGroupResponse? attributeGroup = null)
        {
            var parameters = new DialogParameters
            {
                [nameof(ProductAttributeGroupDialog.IsEdit)] = isEdit,
                [nameof(ProductAttributeGroupDialog.InitialId)] = isEdit ? attributeGroup?.Id : null,
                [nameof(ProductAttributeGroupDialog.InitialName)] = isEdit ? attributeGroup?.Name ?? "" : "",
            };

            var dialog = await DialogService.ShowAsync<ProductAttributeGroupDialog>("", parameters, _dialogOptions);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not ProductAttributeGroupDialog.ProductAttributeGroupDialogResult data)
                return;

            _loading = true;

            var attributeForm = new ProductAttributeGroupFormModel
            {
                Name = data.Name
            };

            try
            {
                var response = data.IsEdit
                    ? await ProductAttributeGroupApi.UpdateProductAttributeGroupAsync(data.Id!, attributeForm)
                    : await ProductAttributeGroupApi.CreateProductAttributeGroupAsync(attributeForm);

                if (response.IsSuccess)
                {
                    var successMessage = data.IsEdit
                        ? L["Common_UpdateDataSuccess"]
                        : L["Common_AddDataSuccess"];

                    Snackbar.Add(successMessage, Severity.Success);
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

        private async Task OpenDeleteConfirm(ProductAttributeGroupResponse attributeGroup)
        {
            _loading = true;

            try
            {
                await _deleteHelper.ExecuteDeleteOperation(
                    attributeGroup.Id,
                    attributeGroup.Name,
                    ProductAttributeGroupApi.DeleteProductAttributeGroupAsync,
                    async () => await LoadData()
                );
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
