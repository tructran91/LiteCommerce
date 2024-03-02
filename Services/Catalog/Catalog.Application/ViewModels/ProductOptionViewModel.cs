namespace Catalog.Application.ViewModels
{
    public class ProductOptionViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayType { get; set; }

        public IList<ProductOptionValueViewModel> Values { get; set; } = new List<ProductOptionValueViewModel>();
    }
}
