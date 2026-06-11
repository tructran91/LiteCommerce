using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Category;
using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.Categories
{
    public partial class CategoryManagement
    {
        [Inject]
        private ICategoryApi CategoryApi { get; set; }

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
            new("Categories", href: null, disabled: true),
        };

        private MudTable<CategoryResponse> _table = null!;
        private List<CategoryResponse> _allCategories = new();
        private bool _loading = false;
        private string? _errorMessage;
        private CategoryQuery _query = new();

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
                var result = await CategoryApi.GetCategoriesAsync(_query.Page, _query.PageSize);
                if (result.IsSuccess)
                {
                    _allCategories = result.Data ?? new();
                }
                else
                {
                    var errorDetails = result.Errors != null && result.Errors.Any()
                        ? string.Join(", ", result.Errors.SelectMany(e => e.Value.Select(msg => $"{e.Key}: {msg}")))
                        : result.Message ?? SystemMessages.ErrorOccurred;

                    _errorMessage = errorDetails;
                    Snackbar.Add(errorDetails, Severity.Error);
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
            NavigationManager.NavigateTo("/categories/add");
        }

        private void NavigateToEdit(string id)
        {
            NavigationManager.NavigateTo($"/categories/edit/{id}");
        }

        private async Task OpenDeleteConfirm(CategoryResponse category)
        {
            _loading = true;

            await _deleteHelper.ExecuteDeleteOperation(
                category.Id,
                category.Name,
                CategoryApi.DeleteCategoryAsync,
                async () => await LoadData()
            );

            _loading = false;
            StateHasChanged();
        }
    }
}
