using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using LiteCommerce.Admin.Models;

namespace LiteCommerce.Admin.Layout
{
    public partial class SidebarMenu
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public List<MenuItemModel> MenuItems { get; set; } = new();

        private void ToggleMenu(MenuItemModel item)
        {
            // item.IsActive = !item.IsActive;

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
