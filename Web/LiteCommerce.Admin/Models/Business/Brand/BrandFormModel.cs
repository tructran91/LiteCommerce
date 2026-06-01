using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.Brand
{
    public class BrandFormModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Brand name is required")]
        public string Name { get; set; }

        public bool IsPublished { get; set; } = true;
    }
}
