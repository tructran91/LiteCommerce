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
        private BaseResponse<List<CategoryResponse>>? _pagedResult;
        private bool _loading = false;
        private string? _errorMessage;
        private CategoryQuery _query = new();

        private async Task<TableData<CategoryResponse>> ServerReload(TableState state, CancellationToken ct)
        {
            await LoadData();

            return new TableData<CategoryResponse>
            {
                Items = _pagedResult?.Data ?? new(),
                TotalItems = _pagedResult?.Pagination.TotalRecords ?? 0,
            };
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            var result = await CategoryApi.GetCategoriesAsync(_query.Page, _query.PageSize);
            if (result.IsSuccess)
            {
                _pagedResult = result;
            }
            else
            {
                _errorMessage = result.Message;
                Snackbar.Add(result.Message ?? SystemMessages.ErrorOccurred, Severity.Error);
            }

            _loading = false;
            StateHasChanged();
        }

        private async Task ReloadTable() => await _table.ReloadServerData();

        private async Task OnPageChanged(int page)
        {
            _query.Page = page;
            await ReloadTable();
        }

        private async Task OnPageSizeChanged(int size)
        {
            _query.PageSize = size;
            _query.Page = 1;
            await ReloadTable();
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
                ReloadTable
            );

            _loading = false;
            StateHasChanged();
        }
    }
}
