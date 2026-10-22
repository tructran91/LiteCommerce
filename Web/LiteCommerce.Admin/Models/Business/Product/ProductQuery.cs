namespace LiteCommerce.Admin.Models.Business.Product
{
    public class ProductQuery
    {
        public string? Name { get; set; }

        public bool? IsFeatured { get; set; }

        public bool? IsAllowToOrder { get; set; }

        public bool? IsCallForPricing { get; set; }

        public bool? IsPublished { get; set; }
    }
}
