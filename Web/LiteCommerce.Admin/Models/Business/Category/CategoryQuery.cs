using LiteCommerce.Admin.Constants;

namespace LiteCommerce.Admin.Models.Business.Category
{
    public class CategoryQuery
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = AppConstants.PageSize;
    }
}
