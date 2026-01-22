using LiteCommerce.Admin.Models.Application;
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

        public List<MenuItem> menuItems { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            menuItems = MenuService.GetHeaderMenu();
        }

        private void ToggleMenu(MenuItem item)
        {
            if (item.IsActive)
            {
                CloseAllMenus(new List<MenuItem> { item });
            }
            else
            {
                CloseAllMenus(menuItems);
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
