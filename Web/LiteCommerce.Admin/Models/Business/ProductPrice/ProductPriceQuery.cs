using LiteCommerce.Admin.Constants;

namespace LiteCommerce.Admin.Models.Business.ProductPrice
{
    public class ProductPriceQuery
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = AppConstants.PageSize;
    }
}
