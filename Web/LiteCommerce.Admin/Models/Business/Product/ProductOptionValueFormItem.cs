namespace LiteCommerce.Admin.Models.Business.Product
{
    public class ProductOptionValueFormItem
    {
        // Feeds the variant NormalizedName on the server, so changing it re-identifies
        // every variant using this value. Display is only a label.
        public string Key { get; set; }

        public string Display { get; set; }
    }
}
