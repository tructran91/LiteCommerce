using LiteCommerce.Admin.Enums;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Resources;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace LiteCommerce.Admin.Services
{
    public class MenuService
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public MenuService(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Title = _localizer["Menu_Dashboard"],
                    Href = "/",
                    Icon = Icons.Material.Filled.Dashboard,
                    IconColor = Color.Primary,
                    Match = NavLinkMatch.All,
                    Type = MenuItemType.Link
                },

                new MenuItem { Title = _localizer["Menu_Catalog"], Type = MenuItemType.Section },
                new MenuItem { Title = _localizer["Menu_Categories"], Href = "/categories", Icon = Icons.Material.Filled.Category, Type = MenuItemType.Link },
                new MenuItem { Title = _localizer["Menu_Brands"], Href = "/brands", Icon = Icons.Material.Filled.Star, Type = MenuItemType.Link },
                new MenuItem { Title = _localizer["Menu_Products"], Href = "/products", Icon = Icons.Material.Filled.Inventory, Type = MenuItemType.Link },
                new MenuItem { Title = _localizer["Menu_ProductPrices"], Href = "/product-prices", Icon = Icons.Material.Filled.AttachMoney, Type = MenuItemType.Link },
                new MenuItem { Title = _localizer["Menu_ProductOptions"], Href = "/product-options", Icon = Icons.Material.Filled.Tune, Type = MenuItemType.Link },
                new MenuItem { Title = _localizer["Menu_ProductAttributeGroups"], Href = "/product-attribute-groups", Icon = Icons.Material.Filled.ViewModule, Type = MenuItemType.Link },
                new MenuItem { Title = _localizer["Menu_ProductAttributes"], Href = "/product-attributes", Icon = Icons.Material.Filled.Style, Type = MenuItemType.Link },
                new MenuItem { Title = _localizer["Menu_ProductTemplates"], Href = "/product-templates", Icon = Icons.Material.Filled.Description, Type = MenuItemType.Link },

                new MenuItem { Title = _localizer["Menu_SalesManagement"], Type = MenuItemType.Section },
                new MenuItem
                {
                    Title = _localizer["Menu_Orders"],
                    Icon = Icons.Material.Filled.Receipt,
                    Type = MenuItemType.Group,
                    ExpandByDefault = true,
                    Children = new List<MenuItem>
                    {
                        new MenuItem { Title = _localizer["Menu_AllOrders"], Href = "/orders" },
                        new MenuItem { Title = _localizer["Menu_PendingOrders"], Href = "/orders/pending" },
                        new MenuItem { Title = _localizer["Menu_ShippingOrders"], Href = "/orders/shipping" },
                        new MenuItem { Title = _localizer["Menu_CompletedOrders"], Href = "/orders/completed" }
                    }
                },

                new MenuItem { Title = _localizer["Menu_System"], Type = MenuItemType.Section },
                new MenuItem { Title = _localizer["Menu_Users"], Href = "/users", Icon = Icons.Material.Filled.ManageAccounts, Type = MenuItemType.Link },
                new MenuItem { Title = _localizer["Menu_ActivityLogs"], Href = "/activity-logs", Icon = Icons.Material.Filled.Timeline, Type = MenuItemType.Link },
                new MenuItem { Title = _localizer["Menu_AuditLogs"], Href = "/audit-logs", Icon = Icons.Material.Filled.History, Type = MenuItemType.Link }
            };
        }

        public List<MenuItem> GetMenuHorizontalItems()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Title = _localizer["Menu_Dashboard"],
                    Href = "/",
                    Icon = Icons.Material.Filled.Dashboard,
                    IconColor = Color.Primary,
                    Match = NavLinkMatch.All,
                    Type = MenuItemType.Link
                },

                new MenuItem
                {
                    Title = _localizer["Menu_Catalog"],
                    Icon = Icons.Material.Filled.Storefront,
                    Type = MenuItemType.Group,
                    Children = new List<MenuItem>
                    {
                        new MenuItem { Title = _localizer["Menu_Categories"], Href = "/categories" },
                        new MenuItem { Title = _localizer["Menu_Brands"], Href = "/brands" },
                        new MenuItem { Title = _localizer["Menu_Products"], Href = "/products" },
                        new MenuItem { Title = _localizer["Menu_ProductPrices"], Href = "/product-prices" },
                        new MenuItem { Title = _localizer["Menu_ProductOptions"], Href = "/product-options" },
                        new MenuItem { Title = _localizer["Menu_ProductAttributeGroups"], Href = "/product-attribute-groups" },
                        new MenuItem { Title = _localizer["Menu_ProductAttributes"], Href = "/product-attributes" },
                        new MenuItem { Title = _localizer["Menu_ProductTemplates"], Href = "/product-templates" },
                    }
                },

                new MenuItem
                {
                    Title = _localizer["Menu_System"],
                    Icon = Icons.Material.Filled.Settings,
                    Type = MenuItemType.Group,
                    Children = new List<MenuItem>
                    {
                        new MenuItem { Title = _localizer["Menu_Users"], Href = "/users" },
                        new MenuItem { Title = _localizer["Menu_ActivityLogs"], Href = "/activity-logs" },
                        new MenuItem { Title = _localizer["Menu_AuditLogs"], Href = "/audit-logs" },
                    }
                }
            };
        }
    }
}
