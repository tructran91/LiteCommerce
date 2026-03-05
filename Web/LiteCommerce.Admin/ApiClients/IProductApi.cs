using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Product;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductApi
    {
        [Get(ApiRoutes.Product.GetAll)]
        Task<BaseResponse<List<BasicProductResponse>>> GetProductsAsync(int currentPage, int pageSize);

        [Get(ApiRoutes.Product.GetById)]
        Task<BaseResponse<ProductFormModel>> GetProductAsync(string id);

        [Post(ApiRoutes.Product.Create)]
        Task<BaseResponse<ProductFormModel>> CreateProductAsync([Body] MultipartFormDataContent content);

        [Put(ApiRoutes.Product.Update)]
        Task<BaseResponse<ProductFormModel>> UpdateProductAsync([Body] MultipartFormDataContent content);

        [Delete(ApiRoutes.Product.Delete)]
        Task<BaseResponse<bool>> DeleteProductAsync(string id);
    }
}
