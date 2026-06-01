using LiteCommerce.Admin.Constants;

namespace LiteCommerce.Admin.Models.Business.ProductAttribute
{
    public class ProductAttributeQuery
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = AppConstants.PageSize;
    }
}
