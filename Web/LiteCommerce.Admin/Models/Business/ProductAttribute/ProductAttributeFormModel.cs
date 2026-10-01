using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.ProductAttribute
{
    public class ProductAttributeFormModel
    {
        [Required]
        public string Name { get; set; }

        public string GroupId { get; set; }
    }
}
