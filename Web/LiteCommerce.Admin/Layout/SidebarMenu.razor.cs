using LiteCommerce.Admin.Models.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Layout
{
    public partial class SidebarMenu
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public List<MenuItem> MenuItems { get; set; } = new();

        private void ToggleMenu(MenuItem item)
        {
            // item.IsActive = !item.IsActive;

            if (item.IsActive)
            {
                CloseAllMenus(new List<MenuItem> { item });
            }
            else
            {
                CloseAllMenus(MenuItems);
                item.IsActive = true;
            }
        }

        private void CloseAllMenus(List<MenuItem> items)
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
