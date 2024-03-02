namespace Catalog.Application.ViewModels
{
    public class ProductViewModel
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public string? Slug { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        public bool IsPublished { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public string? Sku { get; set; }

        public string ThumbnailImageUrl { get; set; }

        public string BrandId { get; set; }

        public IList<string> CategoryIds { get; set; } = new List<string>();

        public IList<ProductAttributeViewModel> Attributes { get; set; } = new List<ProductAttributeViewModel>();

        public IList<ProductOptionViewModel> Options { get; set; } = new List<ProductOptionViewModel>();

        public IList<ProductMediaViewModel> ProductImages { get; set; } = new List<ProductMediaViewModel>();

        public IList<ProductMediaViewModel> ProductDocuments { get; set; } = new List<ProductMediaViewModel>();

        public IList<string> DeletedMediaIds { get; set; } = new List<string>();

        public List<ProductLinkViewModel> RelatedProducts { get; set; } = new List<ProductLinkViewModel>();

        public List<ProductLinkViewModel> CrossSellProducts { get; set; } = new List<ProductLinkViewModel>();
    }
}
