using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.ProductAttribute
{
    public class ProductAttributeFormModel
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string GroupId { get; set; }
    }
}
