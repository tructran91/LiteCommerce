using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.Category;
using LiteCommerce.Admin.Shared.Components;
using Microsoft.AspNetCore.Components;

namespace LiteCommerce.Admin.Pages.Catalog.Categories
{
    public partial class AddCategory
    {
        [Inject]
        private ICategoryApi CategoryApi { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Categories", Url = "/categories", IsActive = false },
            new BreadcrumbItem { Label = "Add Category", Url = "/categories/add-category", IsActive = true }
        };

        private List<BasicCategoryModel> basicCategories = new();

        private CategoryFormModel initCategory = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetBasicCategories();
        }

        private async Task GetBasicCategories()
        {
            var response = await CategoryApi.GetBasicCategoriesAsync();
            if (response.IsSuccess)
            {
                basicCategories = response.Data;
            }
        }

        private async Task FormSubmitted()
        {
            //var response = await CategoryApi.CreateCategoryAsync(initCategory);
        }
    }
}
