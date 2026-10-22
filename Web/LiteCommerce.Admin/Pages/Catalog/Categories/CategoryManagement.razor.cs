using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business.Category;
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

        [Inject]
        private DeleteOperationHelper DeleteHelper { get; set; }

        private List<BreadcrumbItem> _breadcrumbs = new();

        private MudTable<CategoryResponse> _table = null!;
        private List<CategoryResponse> _allCategories = new();
        private bool _loading = false;
        private string? _errorMessage;
        private CategoryQuery _query = new();

        protected override async Task OnInitializedAsync()
        {
            _breadcrumbs = new()
            {
                new(L["Common_Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(L["Menu_Catalog"], href: null, disabled: true),
                new(L["Category_Title"], href: null, disabled: true),
            };

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

            try
            {
                await DeleteHelper.ExecuteDeleteOperation(
                    category.Id,
                    category.Name,
                    CategoryApi.DeleteCategoryAsync,
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
