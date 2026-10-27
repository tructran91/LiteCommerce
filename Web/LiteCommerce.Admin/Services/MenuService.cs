using LiteCommerce.Admin.Enums;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Resources;
using LiteCommerce.Shared.Authorization;
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
                new MenuItem { Title = _localizer["Menu_Categories"], Href = "/categories", Icon = Icons.Material.Filled.Category, Type = MenuItemType.Link, RequiredPermission = AppPermissions.Category.View },
                new MenuItem { Title = _localizer["Menu_Brands"], Href = "/brands", Icon = Icons.Material.Filled.Star, Type = MenuItemType.Link, RequiredPermission = AppPermissions.Brand.View },
                new MenuItem { Title = _localizer["Menu_Products"], Href = "/products", Icon = Icons.Material.Filled.Inventory, Type = MenuItemType.Link, RequiredPermission = AppPermissions.Product.View },
                new MenuItem { Title = _localizer["Menu_ProductPrices"], Href = "/product-prices", Icon = Icons.Material.Filled.AttachMoney, Type = MenuItemType.Link, RequiredPermission = AppPermissions.ProductPrice.View },
                new MenuItem { Title = _localizer["Menu_ProductOptions"], Href = "/product-options", Icon = Icons.Material.Filled.Tune, Type = MenuItemType.Link, RequiredPermission = AppPermissions.ProductOption.View },
                new MenuItem { Title = _localizer["Menu_ProductAttributeGroups"], Href = "/product-attribute-groups", Icon = Icons.Material.Filled.ViewModule, Type = MenuItemType.Link, RequiredPermission = AppPermissions.ProductAttributeGroup.View },
                new MenuItem { Title = _localizer["Menu_ProductAttributes"], Href = "/product-attributes", Icon = Icons.Material.Filled.Style, Type = MenuItemType.Link, RequiredPermission = AppPermissions.ProductAttribute.View },
                new MenuItem { Title = _localizer["Menu_ProductTemplates"], Href = "/product-templates", Icon = Icons.Material.Filled.Description, Type = MenuItemType.Link, RequiredPermission = AppPermissions.ProductTemplate.View },

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
                new MenuItem { Title = _localizer["Menu_Users"], Href = "/users", Icon = Icons.Material.Filled.ManageAccounts, Type = MenuItemType.Link, RequiredPermission = AppPermissions.User.View },
                new MenuItem { Title = _localizer["Menu_Roles"], Href = "/roles", Icon = Icons.Material.Filled.AdminPanelSettings, Type = MenuItemType.Link, RequiredPermission = AppPermissions.Role.View },
                new MenuItem { Title = _localizer["Menu_ActivityLogs"], Href = "/activity-logs", Icon = Icons.Material.Filled.Timeline, Type = MenuItemType.Link, RequiredPermission = AppPermissions.CatalogLog.ViewActivity },
                new MenuItem { Title = _localizer["Menu_AuditLogs"], Href = "/audit-logs", Icon = Icons.Material.Filled.History, Type = MenuItemType.Link, RequiredPermission = AppPermissions.CatalogLog.ViewAudit }
            };
        }

        // Keeps a section only when at least one item under it survived, so a restricted
        // user never sees a heading with nothing beneath it.
        public async Task<List<MenuItem>> GetVisibleMenuItemsAsync(PermissionService permissions)
            => Filter(GetMenuItems(), await permissions.GetGrantedAsync());

        public async Task<List<MenuItem>> GetVisibleMenuHorizontalItemsAsync(PermissionService permissions)
            => Filter(GetMenuHorizontalItems(), await permissions.GetGrantedAsync());

        private static List<MenuItem> Filter(List<MenuItem> source, string[] granted)
        {
            var kept = new List<MenuItem>();

            foreach (var item in source)
            {
                switch (item.Type)
                {
                    case MenuItemType.Section:
                        kept.Add(item);
                        break;

                    case MenuItemType.Group:
                        var children = item.Children.Where(c => IsVisible(c, granted)).ToList();
                        if (children.Count > 0)
                            kept.Add(item.WithChildren(children));
                        break;

                    default:
                        if (IsVisible(item, granted))
                            kept.Add(item);
                        break;
                }
            }

            return RemoveEmptySections(kept);
        }

        private static bool IsVisible(MenuItem item, string[] granted)
            => item.RequiredPermission is null
               || PermissionMatcher.IsGranted(granted, item.RequiredPermission);

        private static List<MenuItem> RemoveEmptySections(List<MenuItem> items)
        {
            var result = new List<MenuItem>();

            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Type == MenuItemType.Section)
                {
                    var next = items.Skip(i + 1).FirstOrDefault();
                    if (next is null || next.Type == MenuItemType.Section)
                        continue;
                }

                result.Add(items[i]);
            }

            return result;
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
                        new MenuItem { Title = _localizer["Menu_Categories"], Href = "/categories" , RequiredPermission = AppPermissions.Category.View },
                        new MenuItem { Title = _localizer["Menu_Brands"], Href = "/brands" , RequiredPermission = AppPermissions.Brand.View },
                        new MenuItem { Title = _localizer["Menu_Products"], Href = "/products" , RequiredPermission = AppPermissions.Product.View },
                        new MenuItem { Title = _localizer["Menu_ProductPrices"], Href = "/product-prices" , RequiredPermission = AppPermissions.ProductPrice.View },
                        new MenuItem { Title = _localizer["Menu_ProductOptions"], Href = "/product-options" , RequiredPermission = AppPermissions.ProductOption.View },
                        new MenuItem { Title = _localizer["Menu_ProductAttributeGroups"], Href = "/product-attribute-groups" , RequiredPermission = AppPermissions.ProductAttributeGroup.View },
                        new MenuItem { Title = _localizer["Menu_ProductAttributes"], Href = "/product-attributes" , RequiredPermission = AppPermissions.ProductAttribute.View },
                        new MenuItem { Title = _localizer["Menu_ProductTemplates"], Href = "/product-templates" , RequiredPermission = AppPermissions.ProductTemplate.View },
                    }
                },

                new MenuItem
                {
                    Title = _localizer["Menu_System"],
                    Icon = Icons.Material.Filled.Settings,
                    Type = MenuItemType.Group,
                    Children = new List<MenuItem>
                    {
                        new MenuItem { Title = _localizer["Menu_Users"], Href = "/users" , RequiredPermission = AppPermissions.User.View },
                        new MenuItem { Title = _localizer["Menu_Roles"], Href = "/roles", RequiredPermission = AppPermissions.Role.View },
                        new MenuItem { Title = _localizer["Menu_ActivityLogs"], Href = "/activity-logs" , RequiredPermission = AppPermissions.CatalogLog.ViewActivity },
                        new MenuItem { Title = _localizer["Menu_AuditLogs"], Href = "/audit-logs" , RequiredPermission = AppPermissions.CatalogLog.ViewAudit },
                    }
                }
            };
        }
    }
}
