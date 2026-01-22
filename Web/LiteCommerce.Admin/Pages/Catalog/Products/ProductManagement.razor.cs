using LiteCommerce.Admin.Models.Application;

namespace LiteCommerce.Admin.Pages.Catalog.Products
{
    public partial class ProductManagement
    {
        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Products", Url = "/products", IsActive = true }
        };

        private bool isLoading = true;
    }
}
