namespace LiteCommerce.Admin.Models
{
    public class BreadcrumbItemModel
    {
        public string Label { get; set; } = string.Empty;

        public string? Url { get; set; } = null;

        public bool IsActive { get; set; } = false;
    }
}
