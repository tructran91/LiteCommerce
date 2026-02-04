namespace LiteCommerce.Admin.Models.Business.ProductTemplate
{
    public class ProductTemplateResponse
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public List<ProductAttributeItem> ProductAttributes { get; set; } = new();
    }
}
