using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductAttributeGroupApi
    {
        [Get(ApiRoutes.ProductAttributeGroup.GetAll)]
        Task<BaseResponse<List<ProductAttributeGroupResponse>>> GetProductAttributeGroupsAsync(int currentPage, int pageSize);

        [Get(ApiRoutes.ProductAttributeGroup.GetById)]
        Task<BaseResponse<ProductAttributeGroupResponse>> GetProductAttributeGroupAsync(string id);

        [Post(ApiRoutes.ProductAttributeGroup.Create)]
        Task<BaseResponse<ProductAttributeGroupResponse>> CreateProductAttributeGroupAsync([Body] ProductAttributeGroupFormModel productAttributeGroup);

        [Put(ApiRoutes.ProductAttributeGroup.Update)]
        Task<BaseResponse<ProductAttributeGroupResponse>> UpdateProductAttributeGroupAsync([Body] ProductAttributeGroupFormModel productAttributeGroup);

        [Delete(ApiRoutes.ProductAttributeGroup.Delete)]
        Task<BaseResponse<bool>> DeleteProductAttributeGroupAsync(string id);
    }
}
