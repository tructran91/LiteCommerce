using LiteCommerce.Admin.Models;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Layout
{
    public partial class HeaderMenu
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private IMenuService MenuService { get; set; }

        public List<MenuItemModel> MenuItems { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            MenuItems = MenuService.GetHeaderMenu();
        }

        private void ToggleMenu(MenuItemModel item)
        {
            if (item.IsActive)
            {
                CloseAllMenus(new List<MenuItemModel> { item });
            }
            else
            {
                CloseAllMenus(MenuItems);
                item.IsActive = true;
            }
        }

        private void CloseAllMenus(List<MenuItemModel> items)
        {
            foreach (var item in items)
            {
                item.IsActive = false;
                if (item.HasChildren)
                {
                    CloseAllMenus(item.Children);
                }
            }
        }

        private async Task HandleSidebar()
        {
            await JSRuntime.InvokeVoidAsync("appInterop.toggleSidebarMenu");
        }
    }
}
