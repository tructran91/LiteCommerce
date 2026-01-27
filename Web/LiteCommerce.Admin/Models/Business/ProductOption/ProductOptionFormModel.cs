using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.ProductOption
{
    public class ProductOptionFormModel
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
