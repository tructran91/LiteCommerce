using Catalog.Application.ViewModels;

namespace Catalog.Application.Responses
{
    public class ProductTemplateResponse
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public List<ProductAttributeOverviewViewModel> ProductAttributes { get; set; }
    }
}
