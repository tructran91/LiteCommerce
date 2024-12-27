using LiteCommerce.Admin.Enums;
using LiteCommerce.Admin.Models.Application;

namespace LiteCommerce.Admin.Services
{
    public interface IMenuService
    {
        List<MenuItem> GetMenuItems(PageType pageType);

        List<MenuItem> GetHeaderMenu();

        List<MenuItem> SetActiveMenuItems(List<MenuItem> menuItems, string currentUrl);
    }

    public class MenuService : IMenuService
    {
        public List<MenuItem> GetMenuItems(PageType pageType)
        {
            return pageType switch
            {
                PageType.Site => GetSiteMenu(),
                PageType.Catalog => GetCatalogMenu(),
                _ => new List<MenuItem>()
            };
        }

        public List<MenuItem> GetHeaderMenu()
        {
            return new List<MenuItem>
            {
                GetSiteMenu().First(),
                GetCatalogMenu().First()
            };
        }

        public List<MenuItem> SetActiveMenuItems(List<MenuItem> menuItems, string currentUrl)
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

        private List<MenuItem> GetSiteMenu()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Site</span>
                        </li>",
                    Url = string.Empty,
                    Title = "Site",
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Title = "Users",
                            Icon = "ti ti-users",
                            Url = "/users",
                        },
                        new MenuItem
                        {
                            Title = "Vendors",
                            Icon = "ti ti-building-store",
                            Url = "/vendors",
                        },
                        new MenuItem
                        {
                            Title = "Customer Groups",
                            Icon = "ti ti-user-circle",
                            Url = "/customer-groups",
                        },
                        new MenuItem
                        {
                            Title = "Reviews",
                            Icon = "ti ti-star",
                            Url = "/reviews",
                        },
                        new MenuItem
                        {
                            Title = "Review Replies",
                            Icon = "ti ti-message-chatbot",
                            Url = "/review-replies",
                        },
                        new MenuItem
                        {
                            Title = "Comments",
                            Icon = "ti ti-category",
                            Url = "/comments",
                        }
                    }
                }
            };
        }

        private List<MenuItem> GetDivisionsMenu()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Divisions</span>
                        </li>",
                    Title = "Divisions",
                    Url = string.Empty,
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Title = "Division Details",
                            Url = "/divisions/division-details"
                        },
                        new MenuItem
                        {
                            Title = "Location",
                            Url = "/divisions/location"
                        },
                        new MenuItem
                        {
                            Title = "Communities",
                            Url = "/divisions/communities"
                        }
                    }
                },
                new MenuItem
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Costing</span>
                        </li>",
                    Title = "Costing",
                    Url = string.Empty,
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Title = "Vendors",
                            Url = "/divisions/vendors"
                        },
                        new MenuItem
                        {
                            Title = "Community Taxes",
                            Url = "/divisions/community-taxes"
                        }
                    }
                }
            };
        }

        private List<MenuItem> GetCatalogMenu()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    SpecialMenu = @"
                        <li class=""nav-small-cap"">
                            <i class=""ti ti-dots nav-small-cap-icon fs-4""></i>
                            <span class=""hide-menu"">Catalog</span>
                        </li>",
                    Title = "Catalog",
                    Url = string.Empty,
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Title = "Products",
                            Icon = "ti ti-package",
                            Url = "/products",
                        },
                        new MenuItem
                        {
                            Title = "Product Prices",
                            Icon = "ti ti-currency-dollar",
                            Url = "/product-prices",
                        },
                        new MenuItem
                        {
                            Title = "Categories",
                            Icon = "ti ti-timeline-event",
                            Url = "/categories",
                        },
                        new MenuItem
                        {
                            Title = "Brands",
                            Icon = "ti ti-badge",
                            Url = "/brands",
                        },
                        new MenuItem
                        {
                            Title = "Product Options",
                            Icon = "ti ti-adjustments",
                            Url = "/product-options",
                        },
                        new MenuItem
                        {
                            Title = "Product Attribute Groups",
                            Icon = "ti ti-category",
                            Url = "/product-attribute-groups",
                        },
                        new MenuItem
                        {
                            Title = "Product Attributes",
                            Icon = "ti ti-tags",
                            Url = "/product-attributes",
                        },
                        new MenuItem
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
