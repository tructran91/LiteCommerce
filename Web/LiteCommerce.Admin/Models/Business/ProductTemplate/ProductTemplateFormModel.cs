using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.ProductTemplate
{
    public class ProductTemplateFormModel
    {
        [Required]
        public string Name { get; set; }

        public List<ProductAttributeItem> ProductAttributes { get; set; } = new();
    }
}
