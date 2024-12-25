using Blazored.LocalStorage;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection.Metadata;

namespace LiteCommerce.Admin.Layout
{
    public partial class LayoutSettings
    {
        [Parameter]
        public LayoutSettingsModel CurrentSettings { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                await LoadLayoutSettingsAsync();
            }
        }

        // Theme mode Dark or Light
        private async Task SetThemeMode(string theme)
        {
            CurrentSettings.Theme = theme;

            if (theme == "dark")
            {
                await JSRuntime.InvokeVoidAsync("layoutInterop.setThemeMode",
                    "dark",      // theme
                    "dark-logo", // darkDisplay
                    "light-logo", // lightDisplay
                    "moon",      // sunDisplay
                    "sun"        // moonDisplay
                );
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("layoutInterop.setThemeMode",
                    "light",     // theme
                    "light-logo", // darkDisplay
                    "dark-logo",  // lightDisplay
                    "sun",       // sunDisplay
                    "moon"       // moonDisplay
                );
            }

            await SaveSettingsAsync();
        }

        // Theme Direction RTL LTR
        private async Task SetLayoutDirection(string direction)
        {
            CurrentSettings.Direction = direction;
            await JSRuntime.InvokeVoidAsync("layoutInterop.setLayoutDirection", direction);
            await SaveSettingsAsync();
        }

        // Theme Layout Color
        private async Task SetColorTheme(string colorTheme)
        {
            CurrentSettings.ColorTheme = colorTheme;
            await JSRuntime.InvokeVoidAsync("layoutInterop.setColorTheme", colorTheme);
            await SaveSettingsAsync();
        }

        // Theme Layout Vertical or Horizontal
        private async Task SetLayoutType(string layoutType)
        {
            CurrentSettings.Layout = layoutType;
            await JSRuntime.InvokeVoidAsync("layoutInterop.setLayoutType", layoutType);
            await SaveSettingsAsync();
        }

        // Theme Layout Box or Full
        private async Task SetContainerOption(string containerOption)
        {
            CurrentSettings.ContainerOption = containerOption;
            await JSRuntime.InvokeVoidAsync("layoutInterop.setContainerOption", containerOption);
            await SaveSettingsAsync();
        }

        // Theme Sidebar Full or Collapse
        private async Task SetSidebarType(string sidebarType)
        {
            CurrentSettings.SidebarType = sidebarType;
            await JSRuntime.InvokeVoidAsync("layoutInterop.setSidebarType", sidebarType);
            await SaveSettingsAsync();
        }

        private async Task ManageSidebarType()
        {
            await JSRuntime.InvokeVoidAsync("layoutInterop.manageSidebarType", CurrentSettings.SidebarType);
        }

        // Theme card with Border or Shadow
        private async Task SetCardLayout(string cardLayout)
        {
            CurrentSettings.CardLayout = cardLayout;
            await JSRuntime.InvokeVoidAsync("layoutInterop.setCardLayout", cardLayout);
            await SaveSettingsAsync();
        }

        private async Task LoadLayoutSettingsAsync()
        {
            await SetThemeMode(CurrentSettings.Theme);
            await SetLayoutDirection(CurrentSettings.Direction);
            await SetColorTheme(CurrentSettings.ColorTheme);
            await SetLayoutType(CurrentSettings.Layout);
            await SetContainerOption(CurrentSettings.ContainerOption);
            await SetSidebarType(CurrentSettings.SidebarType);
            await SetCardLayout(CurrentSettings.CardLayout);
            await ManageSidebarType();
        }

        private async Task SaveSettingsAsync()
        {
            await LocalStorage.SetItemAsync(LayoutConstant.LayoutSettingName, CurrentSettings);
        }
    }
}
