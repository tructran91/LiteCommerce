using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.Product
{
    public class ProductFormModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a brand")]
        public string BrandId { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        public bool IsPublished { get; set; } = true;

        public decimal? Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public string? Sku { get; set; }

        public string? Gtin { get; set; }

        public string? ThumbnailImageUrl { get; set; }

        public IList<string> CategoryIds { get; set; } = new List<string>();

        public IList<ProductAttributeFormItem> Attributes { get; set; } = new List<ProductAttributeFormItem>();
    }
}
