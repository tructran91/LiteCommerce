using LiteCommerce.Admin.Constants;

namespace LiteCommerce.Admin.Models.Business.ProductAttributeGroup
{
    public class ProductAttributeGroupQuery
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = AppConstants.PageSize;
    }
}
