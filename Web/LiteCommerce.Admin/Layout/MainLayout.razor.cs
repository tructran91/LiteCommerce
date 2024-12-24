﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Enums;
using LiteCommerce.Admin.Models;
using LiteCommerce.Admin.Services;

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

        private LayoutSettingsModel Settings { get; set; }

        public List<MenuItemModel> MenuItems { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetCurrentSettingsAsync();
            NavigationManager.LocationChanged += HandleLocationChanged;
            LoadMenu();
        }

        private async Task GetCurrentSettingsAsync()
        {
            Settings = await LocalStorage.GetItemAsync<LayoutSettingsModel>(LayoutConstant.LayoutSettingName);
            if (Settings == null)
            {
                Settings = new LayoutSettingsModel();
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
            MenuItems = MenuService.GetMenuItems(pageType);
            MenuItems = MenuService.SetActiveMenuItems(MenuItems, currentUrl);
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