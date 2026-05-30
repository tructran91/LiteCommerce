using LiteCommerce.Admin.Enums;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace LiteCommerce.Admin.Models.Application
{
    public class MenuItem
    {
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public Color IconColor { get; set; } = Color.Default;
        public string Href { get; set; } = string.Empty;
        public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;
        public MenuItemType Type { get; set; } = MenuItemType.Link;

        public bool ExpandByDefault { get; set; } = false;
        public List<MenuItem> Children { get; set; } = new();
    }
}
