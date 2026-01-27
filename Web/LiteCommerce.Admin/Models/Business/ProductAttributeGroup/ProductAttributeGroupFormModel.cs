using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.ProductAttributeGroup
{
    public class ProductAttributeGroupFormModel
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
