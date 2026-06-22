using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Product;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.Products
{
    public partial class ProductManagement
    {
        [Inject]
        private IProductApi ProductApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private DeleteOperationHelper _deleteHelper => new(DialogService, Snackbar);

        private List<BreadcrumbItem> _breadcrumbs = new()
        {
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Catalog", href: null, disabled: true),
            new("Products", href: null, disabled: true),
        };

        private MudTable<BasicProductResponse> _table = null!;
        private List<BasicProductResponse> _allProducts = new();
        private bool _loading = false;
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            try
            {
                var result = await ProductApi.GetProductsAsync(1, int.MaxValue);
                if (result.IsSuccess)
                {
                    _allProducts = result.Data ?? new();
                }
                else
                {
                    _errorMessage = result.Message;
                    Snackbar.Add(result.Message ?? SystemMessages.ErrorOccurred, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }

            _loading = false;
            StateHasChanged();
        }

        private void OnSortChanged(SortDirection direction)
        {
            _table.NavigateTo(Page.First);
        }

        private void NavigateToAdd()
        {
            NavigationManager.NavigateTo("/products/add");
        }

        private void NavigateToEdit(string id)
        {
            NavigationManager.NavigateTo($"/products/edit/{id}");
        }

        private async Task OpenDeleteConfirm(BasicProductResponse product)
        {
            _loading = true;

            await _deleteHelper.ExecuteDeleteOperation(
                product.Id.ToString(),
                product.Name,
                ProductApi.DeleteProductAsync,
                async () => await LoadData()
            );

            _loading = false;
            StateHasChanged();
        }
    }
}
