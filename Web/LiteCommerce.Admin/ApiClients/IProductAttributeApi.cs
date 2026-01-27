using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductAttributeApi
    {
        [Get(ApiRoutes.ProductAttribute.GetAll)]
        Task<BaseResponse<List<ProductAttributeResponse>>> GetProductAttributesAsync(int currentPage, int pageSize);

        [Get(ApiRoutes.ProductAttribute.GetById)]
        Task<BaseResponse<ProductAttributeResponse>> GetProductAttributeAsync(string id);

        [Post(ApiRoutes.ProductAttribute.Create)]
        Task<BaseResponse<ProductAttributeResponse>> CreateProductAttributeAsync([Body] ProductAttributeFormModel productAttribute);

        [Put(ApiRoutes.ProductAttribute.Update)]
        Task<BaseResponse<ProductAttributeResponse>> UpdateProductAttributeAsync([Body] ProductAttributeFormModel productAttribute);

        [Delete(ApiRoutes.ProductAttribute.Delete)]
        Task<BaseResponse<bool>> DeleteProductAttributeAsync(string id);
    }
}
