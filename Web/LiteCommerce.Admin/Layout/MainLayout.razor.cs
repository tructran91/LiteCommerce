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

        public List<MenuItem> menuItems { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetCurrentSettingsAsync();
            NavigationManager.LocationChanged += HandleLocationChanged;
            LoadMenu();
        }

        private async Task GetCurrentSettingsAsync()
        {
            var savedSettings = await LocalStorage.GetItemAsync<LayoutSettings>(LayoutConstant.LayoutSettingName);
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

        private PageType DeterminePageType(string url)
        {
            var urlParts = url.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (urlParts.Length > 0)
            {
                switch (urlParts[0].ToLower())
                {
                    case "users":
                        return PageType.Site;

                    case "vendors":
                        return PageType.Site;

                    case "products":
                        return PageType.Catalog;

                    case "product-prices":
                        return PageType.Catalog;

                    case "categories":
                        return PageType.Catalog;

                    case "brands":
                        return PageType.Catalog;

                    default:
                        return PageType.Site;
                }
            }

            return PageType.Site;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }
    }
}
