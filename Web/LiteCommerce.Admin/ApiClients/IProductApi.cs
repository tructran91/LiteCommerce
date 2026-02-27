using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Product;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductApi
    {
        [Get(ApiRoutes.Product.GetById)]
        Task<BaseResponse<ProductFormModel>> GetProductAsync(string id);

        [Post(ApiRoutes.Product.Create)]
        [Multipart]
        Task<BaseResponse<ProductFormModel>> CreateProductAsync(MultipartFormDataContent content);

        [Put(ApiRoutes.Product.Update)]
        [Multipart]
        Task<BaseResponse<ProductFormModel>> UpdateProductAsync(MultipartFormDataContent content);

        [Delete(ApiRoutes.Product.Delete)]
        Task<BaseResponse<bool>> DeleteProductAsync(string id);
    }
}
