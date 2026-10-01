using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.ProductAttributeGroup
{
    public class ProductAttributeGroupFormModel
    {
        [Required]
        public string Name { get; set; }
    }
}
