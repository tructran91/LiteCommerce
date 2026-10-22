using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business.ProductAttribute;
using LiteCommerce.Admin.Models.Business.ProductTemplate;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.ProductTemplates
{
    public partial class ProductTemplateManagement
    {
        [Inject]
        private IProductTemplateApi ProductTemplateApi { get; set; }

        [Inject]
        private IProductAttributeApi ProductAttributeApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private DeleteOperationHelper DeleteHelper { get; set; }

        private List<BreadcrumbItem> _breadcrumbs = new();

        private MudTable<ProductTemplateResponse> _table = null!;
        private List<ProductTemplateResponse> _allTemplates = new();
        private List<ProductAttributeResponse> _productAttributes = new();
        private bool _loading = false;
        private string? _errorMessage;
        private ProductTemplateQuery _query = new();

        private DialogOptions _dialogOptions = new()
        {
            MaxWidth = MaxWidth.Medium,
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
                new(L["Template_Title"], href: null, disabled: true),
            };

            await LoadProductAttributes();
            await LoadData();
        }

        private async Task LoadProductAttributes()
        {
            var response = await ProductAttributeApi.GetProductAttributesAsync(1, 100);
            if (response.IsSuccess)
            {
                _productAttributes = response.Data;
            }
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            try
            {
                var result = await ProductTemplateApi.GetProductTemplatesAsync(_query.Page, _query.PageSize);
                if (result.IsSuccess)
                {
                    _allTemplates = result.Data ?? new();
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

        private async Task OpenTemplateDialog(bool isEdit, ProductTemplateResponse? productTemplate = null)
        {
            var parameters = new DialogParameters
            {
                [nameof(ProductTemplateDialog.IsEdit)] = isEdit,
                [nameof(ProductTemplateDialog.InitialId)] = isEdit ? productTemplate?.Id : null,
                [nameof(ProductTemplateDialog.InitialName)] = isEdit ? productTemplate?.Name ?? "" : "",
                [nameof(ProductTemplateDialog.AllAttributes)] = _productAttributes,
                [nameof(ProductTemplateDialog.InitialSelectedAttributes)] = isEdit ? productTemplate?.ProductAttributes : null,
            };

            var dialog = await DialogService.ShowAsync<ProductTemplateDialog>("", parameters, _dialogOptions);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not ProductTemplateDialog.TemplateDialogResult data)
                return;

            _loading = true;

            var templateForm = new ProductTemplateFormModel
            {
                Name = data.Name,
                ProductAttributes = data.ProductAttributes
            };

            try
            {
                var response = data.IsEdit
                    ? await ProductTemplateApi.UpdateProductTemplateAsync(data.Id!, templateForm)
                    : await ProductTemplateApi.CreateProductTemplateAsync(templateForm);

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

        private async Task OpenDeleteConfirm(ProductTemplateResponse productTemplate)
        {
            _loading = true;

            try
            {
                await DeleteHelper.ExecuteDeleteOperation(
                    productTemplate.Id,
                    productTemplate.Name,
                    ProductTemplateApi.DeleteProductTemplateAsync,
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
