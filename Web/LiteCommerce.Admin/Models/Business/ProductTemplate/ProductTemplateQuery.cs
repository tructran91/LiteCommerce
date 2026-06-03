using LiteCommerce.Admin.Constants;

namespace LiteCommerce.Admin.Models.Business.ProductTemplate
{
    public class ProductTemplateQuery
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = AppConstants.PageSize;
    }
}
