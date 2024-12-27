using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business
{
    public class Brand
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Slug { get; set; }

        public bool IsPublished { get; set; }
    }
}
