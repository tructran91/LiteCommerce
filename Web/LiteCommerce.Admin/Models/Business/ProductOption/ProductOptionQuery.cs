using LiteCommerce.Admin.Constants;

namespace LiteCommerce.Admin.Models.Business.ProductOption
{
    public class ProductOptionQuery
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = AppConstants.PageSize;
    }
}
