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
                // Dashboard
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

                // --- QUẢN LÝ BÁN HÀNG ---
                new MenuItem { Title = "Quản lý bán hàng", Type = MenuItemType.Section },
                
                // Group: Đơn hàng
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

                // Group: Sản phẩm
                new MenuItem
                {
                    Title = "Sản phẩm",
                    Icon = Icons.Material.Filled.Inventory,
                    Type = MenuItemType.Group,
                    ExpandByDefault = false, // Tương đương Expanded="false" trong code cũ
                    Children = new List<MenuItem>
                    {
                        new MenuItem { Title = "Danh sách", Href = "/products" },
                        new MenuItem { Title = "Thêm mới", Href = "/products/add" },
                        new MenuItem { Title = "Danh mục", Href = "/categories" },
                        new MenuItem { Title = "Tồn kho", Href = "/products/inventory" }
                    }
                },

                // Khách hàng
                new MenuItem
                {
                    Title = "Khách hàng",
                    Href = "/customers",
                    Icon = Icons.Material.Filled.People,
                    Type = MenuItemType.Link
                },

                // --- HỆ THỐNG ---
                new MenuItem { Title = "Hệ thống", Type = MenuItemType.Section },

                new MenuItem { Title = "Nhân viên", Href = "/staff", Icon = Icons.Material.Filled.ManageAccounts, Type = MenuItemType.Link },
                new MenuItem { Title = "Cài đặt cửa hàng", Href = "/system-settings", Icon = Icons.Material.Filled.Tune, Type = MenuItemType.Link }
            };
        }
    }
}
