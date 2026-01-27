using Blazored.LocalStorage;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Enums;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace LiteCommerce.Admin.Layout
{
    public partial class MainLayout
    {
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IMenuService MenuService { get; set; }

        private LayoutSettings settings { get; set; } = new();

        private List<MenuItem> menuItems { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetCurrentSettingsAsync();
            NavigationManager.LocationChanged += HandleLocationChanged;
            LoadMenu();
        }

        private async Task GetCurrentSettingsAsync()
        {
            var savedSettings = await LocalStorage.GetItemAsync<LayoutSettings>(AppConstants.LayoutSettingName);
            if (savedSettings != null)
            {
                settings = savedSettings;
            }
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            LoadMenu();
            StateHasChanged();
        }

        private void LoadMenu()
        {
            var currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var pageType = DeterminePageType(currentUrl);
            menuItems = MenuService.GetMenuItems(pageType);
            menuItems = MenuService.SetActiveMenuItems(menuItems, currentUrl);
        }

        private static readonly HashSet<string> SitePages = new(StringComparer.OrdinalIgnoreCase)
        {
            "users",
            "vendors"
        };

        private static readonly HashSet<string> CatalogPages = new(StringComparer.OrdinalIgnoreCase)
        {
            "products",
            "product-prices",
            "categories",
            "brands",
            "product-options",
            "product-attribute-groups",
            "product-attributes",
            "product-templates"
        };

        private PageType DeterminePageType(string url)
        {
            var urlParts = url.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (urlParts.Length > 0 && SitePages.Contains(urlParts[0]))
            {
                return PageType.Site;
            }
            else if (urlParts.Length > 0 && CatalogPages.Contains(urlParts[0]))
            {
                return PageType.Catalog;
            }

            return PageType.Site;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }
    }
}
