using Microsoft.AspNetCore.Components.Forms;

namespace LiteCommerce.Admin.Models.Business.Product
{
    public class CreateProductFormModel
    {
        public ProductFormModel Product { get; set; } = new();

        public IBrowserFile? ThumbnailImage { get; set; }

        public List<IBrowserFile> ProductImages { get; set; } = new();

        public List<IBrowserFile> ProductDocuments { get; set; } = new();
    }
}
