using Catalog.Application.ViewModels;

namespace Catalog.Application.Requests
{
    public class CreateProductTemplateRequest
    {
        public string Name { get; set; }

        public List<ProductAttributeOverviewViewModel> ProductAttributes { get; set; }
    }
}
