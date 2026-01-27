using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.ProductAttributeGroup
{
    public class ProductAttributeFormModel
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string GroupId { get; set; }
    }
}
