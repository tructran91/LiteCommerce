using LiteCommerce.Admin.Constants;

namespace LiteCommerce.Admin.Models.Business.Brand
{
    public class BrandQuery
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = AppConstants.PageSize;
    }
}
