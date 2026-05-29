using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ProductPrice;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductPriceApi
    {
        [Get(ApiRoutes.ProductPrice.GetProductPricing)]
        Task<BaseResponse<List<ProductPricingResponse>>> GetProductPricingAsync(int currentPage, int pageSize);

        [Put(ApiRoutes.ProductPrice.UpdateProductPricing)]
        Task<BaseResponse<List<ProductPricingResponse>>> UpdateProductPricingAsync([Body] List<UpdateProductPricingRequest> requests);
    }
}
