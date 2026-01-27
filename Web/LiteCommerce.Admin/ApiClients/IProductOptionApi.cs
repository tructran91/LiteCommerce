using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ProductOption;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductOptionApi
    {
        [Get(ApiRoutes.ProductOption.GetAll)]
        Task<BaseResponse<List<ProductOptionResponse>>> GetProductOptionsAsync(int currentPage, int pageSize);

        [Get(ApiRoutes.ProductOption.GetById)]
        Task<BaseResponse<ProductOptionResponse>> GetProductOptionAsync(string id);

        [Post(ApiRoutes.ProductOption.Create)]
        Task<BaseResponse<ProductOptionResponse>> CreateProductOptionAsync([Body] ProductOptionFormModel productOption);

        [Put(ApiRoutes.ProductOption.Update)]
        Task<BaseResponse<ProductOptionResponse>> UpdateProductOptionAsync([Body] ProductOptionFormModel productOption);

        [Delete(ApiRoutes.ProductOption.Delete)]
        Task<BaseResponse<bool>> DeleteProductOptionAsync(string id);
    }
}
