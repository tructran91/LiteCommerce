namespace LiteCommerce.Admin.Models.Application
{
    public class BreadcrumbItem
    {
        public string Label { get; set; } = string.Empty;

        public string? Url { get; set; } = null;

        public bool IsActive { get; set; } = false;
    }
}
