namespace Catalog.Core.DTOs
{
    public record ProductListFilter
    {
        public string? Name { get; set; }

        public bool? IsFeatured { get; set; }

        public bool? IsAllowToOrder { get; set; }

        public bool? IsCallForPricing { get; set; }

        public bool? IsPublished { get; set; }

        public string? SortBy { get; set; }

        public string? SortDirection { get; set; }
    }
}
