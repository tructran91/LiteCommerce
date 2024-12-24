using LiteCommerce.Admin.Enums;
using LiteCommerce.Admin.Models;

namespace LiteCommerce.Admin.Services
{
    public interface IMenuService
    {
        List<MenuItemModel> GetMenuItems(PageType pageType);

        List<MenuItemModel> GetHeaderMenu();

        List<MenuItemModel> SetActiveMenuItems(List<MenuItemModel> menuItems, string currentUrl);
    }

    public class MenuService : IMenuService
    {
        public List<MenuItemModel> GetMenuItems(PageType pageType)
        {
            return pageType switch
            {
                PageType.Site => GetSiteMenu(),
                PageType.Catalog => GetCatalogMenu(),
                _ => new List<MenuItemModel>()
            };
        }

        public List<MenuItemModel> GetHeaderMenu()
        {
            return new List<MenuItemModel>
            {
                GetSiteMenu().First(),
                GetCatalogMenu().First()
            };
        }

        public List<MenuItemModel> SetActiveMenuItems(List<MenuItemModel> menuItems, string currentUrl)
        {
            foreach (var item in menuItems)
            {
                item.IsActive = IsUrlMatch(item.Url, currentUrl);

                if (item.Children != null && item.Children.Any())
                {
                    foreach (var childItem in item.Children)
                    {
                        childItem.IsActive = IsUrlMatch(childItem.Url, currentUrl);
                        if (childItem.IsActive)
                        {
                            item.IsActive = true;
                        }
                    }
                }
            }

            return menuItems;
        }

        private bool IsUrlMatch(string menuUrl, string currentUrl)
        {
            var currentUrlWithoutParams = currentUrl.Split('?')[0];

            menuUrl = menuUrl.Trim('/').ToLower();
            currentUrlWithoutParams = currentUrlWithoutParams.Trim('/').ToLower();

            if (menuUrl == currentUrlWithoutParams) return true;

            return currentUrlWithoutParams.StartsWith(menuUrl + "/");
        }

        private List<MenuItemModel> GetSiteMenu()
        {
            return new List<MenuItemModel>
            {
                new MenuItemModel
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Site</span>
                        </li>",
                    Url = string.Empty,
                    Title = "Site",
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel
                        {
                            Title = "Users",
                            Icon = "ti ti-users",
                            Url = "/users",
                        },
                        new MenuItemModel
                        {
                            Title = "Vendors",
                            Icon = "ti ti-building-store",
                            Url = "/vendors",
                        },
                        new MenuItemModel
                        {
                            Title = "Customer Groups",
                            Icon = "ti ti-user-circle",
                            Url = "/customer-groups",
                        },
                        new MenuItemModel
                        {
                            Title = "Reviews",
                            Icon = "ti ti-star",
                            Url = "/reviews",
                        },
                        new MenuItemModel
                        {
                            Title = "Review Replies",
                            Icon = "ti ti-message-chatbot",
                            Url = "/review-replies",
                        },
                        new MenuItemModel
                        {
                            Title = "Comments",
                            Icon = "ti ti-category",
                            Url = "/comments",
                        }
                    }
                }
            };
        }

        private List<MenuItemModel> GetDivisionsMenu()
        {
            return new List<MenuItemModel>
            {
                new MenuItemModel
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Divisions</span>
                        </li>",
                    Title = "Divisions",
                    Url = string.Empty,
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel
                        {
                            Title = "Division Details",
                            Url = "/divisions/division-details"
                        },
                        new MenuItemModel
                        {
                            Title = "Location",
                            Url = "/divisions/location"
                        },
                        new MenuItemModel
                        {
                            Title = "Communities",
                            Url = "/divisions/communities"
                        }
                    }
                },
                new MenuItemModel
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Costing</span>
                        </li>",
                    Title = "Costing",
                    Url = string.Empty,
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel
                        {
                            Title = "Vendors",
                            Url = "/divisions/vendors"
                        },
                        new MenuItemModel
                        {
                            Title = "Community Taxes",
                            Url = "/divisions/community-taxes"
                        }
                    }
                }
            };
        }

        private List<MenuItemModel> GetCatalogMenu()
        {
            return new List<MenuItemModel>
            {
                new MenuItemModel
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Catalog</span>
                        </li>",
                    Title = "Catalog",
                    Url = string.Empty,
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel
                        {
                            Title = "Products",
                            Icon = "ti ti-package",
                            Url = "/products",
                        },
                        new MenuItemModel
                        {
                            Title = "Product Prices",
                            Icon = "ti ti-currency-dollar",
                            Url = "/product-prices",
                        },
                        new MenuItemModel
                        {
                            Title = "Categories",
                            Icon = "ti ti-timeline-event",
                            Url = "/categories",
                        },
                        new MenuItemModel
                        {
                            Title = "Brands",
                            Icon = "ti ti-badge",
                            Url = "/brands",
                        },
                        new MenuItemModel
                        {
                            Title = "Product Options",
                            Icon = "ti ti-adjustments",
                            Url = "/product-options",
                        },
                        new MenuItemModel
                        {
                            Title = "Product Attribute Groups",
                            Icon = "ti ti-category",
                            Url = "/product-attribute-groups",
                        },
                        new MenuItemModel
                        {
                            Title = "Product Attributes",
                            Icon = "ti ti-tags",
                            Url = "/product-attributes",
                        },
                        new MenuItemModel
                        {
                            Title = "Product Templates",
                            Icon = "ti ti-template",
                            Url = "/product-templates",
                        }
                    }
                },
            };
        }
    }
}
