using LiteCommerce.Admin.Enums;
using LiteCommerce.Admin.Models.Application;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace LiteCommerce.Admin.Services
{
    public class MenuService
    {
        public List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Title = "Dashboard",
                    Href = "/",
                    Icon = Icons.Material.Filled.Dashboard,
                    IconColor = Color.Primary,
                    Match = NavLinkMatch.All,
                    Type = MenuItemType.Link
                },

                new MenuItem { Title = "Catalog", Type = MenuItemType.Section },
                new MenuItem { Title = "Categories", Href = "/categories", Icon = Icons.Material.Filled.Category, Type = MenuItemType.Link },
                new MenuItem { Title = "Brands", Href = "/brands", Icon = Icons.Material.Filled.Star, Type = MenuItemType.Link },
                new MenuItem { Title = "Products", Href = "/products", Icon = Icons.Material.Filled.Inventory, Type = MenuItemType.Link },
                new MenuItem { Title = "Product Prices", Href = "/product-prices", Icon = Icons.Material.Filled.AttachMoney, Type = MenuItemType.Link },
                new MenuItem { Title = "Product Options", Href = "/product-options", Icon = Icons.Material.Filled.Tune, Type = MenuItemType.Link },
                new MenuItem { Title = "Product Attribute Groups", Href = "/product-attribute-groups", Icon = Icons.Material.Filled.ViewModule, Type = MenuItemType.Link },
                new MenuItem { Title = "Product Attributes", Href = "/product-attributes", Icon = Icons.Material.Filled.Style, Type = MenuItemType.Link },
                new MenuItem { Title = "Product Templates", Href = "/product-templates", Icon = Icons.Material.Filled.Description, Type = MenuItemType.Link },

                new MenuItem { Title = "Quản lý bán hàng", Type = MenuItemType.Section },
                new MenuItem
                {
                    Title = "Đơn hàng",
                    Icon = Icons.Material.Filled.Receipt,
                    Type = MenuItemType.Group,
                    ExpandByDefault = true, // Tương đương Expanded="@DrawerOpen" trong code cũ
                    Children = new List<MenuItem>
                    {
                        new MenuItem { Title = "Tất cả đơn hàng", Href = "/orders" },
                        new MenuItem { Title = "Chờ xử lý", Href = "/orders/pending" },
                        new MenuItem { Title = "Đang giao", Href = "/orders/shipping" },
                        new MenuItem { Title = "Hoàn thành", Href = "/orders/completed" }
                    }
                },

                new MenuItem { Title = "Hệ thống", Type = MenuItemType.Section },
                new MenuItem { Title = "Nhân viên", Href = "/staff", Icon = Icons.Material.Filled.ManageAccounts, Type = MenuItemType.Link },
                new MenuItem { Title = "Cài đặt cửa hàng", Href = "/system-settings", Icon = Icons.Material.Filled.Tune, Type = MenuItemType.Link }
            };
        }

        public List<MenuItem> GetMenuHorizontalItems()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Title = "Dashboard",
                    Href = "/",
                    Icon = Icons.Material.Filled.Dashboard,
                    IconColor = Color.Primary,
                    Match = NavLinkMatch.All,
                    Type = MenuItemType.Link
                },
                                
                new MenuItem
                {
                    Title = "Catalog",
                    Icon = Icons.Material.Filled.Storefront,
                    Type = MenuItemType.Group,
                    Children = new List<MenuItem>
                    {
                        new MenuItem { Title = "Categories", Href = "/categories" },
                        new MenuItem { Title = "Brands", Href = "/brands" },
                        new MenuItem { Title = "Products", Href = "/products" },
                        new MenuItem { Title = "Product Prices", Href = "/product-prices" },
                        new MenuItem { Title = "Product Options", Href = "/product-options" },
                        new MenuItem { Title = "Product Attribute Groups", Href = "/product-attribute-groups" },
                        new MenuItem { Title = "Product Attributes", Href = "/product-attributes" },
                        new MenuItem { Title = "Product Templates", Href = "/product-templates" },
                    }
                }
            };
        }
    }
}
