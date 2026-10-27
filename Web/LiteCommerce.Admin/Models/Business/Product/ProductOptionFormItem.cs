namespace LiteCommerce.Admin.Models.Business.Product
{
    public class ProductOptionFormItem
    {
        // The ProductOption picked from the global option list.
        public string Id { get; set; }

        public string Name { get; set; }

        // "text", "color" or "image".
        public string DisplayType { get; set; } = "text";

        public IList<ProductOptionValueFormItem> Values { get; set; } = new List<ProductOptionValueFormItem>();
    }
}
