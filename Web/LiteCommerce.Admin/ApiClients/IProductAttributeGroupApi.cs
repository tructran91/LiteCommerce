using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductAttributeGroupApi
    {
        [Get("/api/admin/product-attribute-group/GetAllProductAttributeGroups")]
        Task<BaseResponse<List<ProductAttributeGroupResponse>>> GetProductAttributeGroupsAsync(int currentPage, int pageSize);

        [Get("/api/admin/product-attribute-group/GetProductAttributeGroupById/{id}")]
        Task<BaseResponse<ProductAttributeGroupResponse>> GetProductAttributeGroupAsync(string id);

        [Post("/api/admin/product-attribute-group/CreateProductAttributeGroup")]
        Task<BaseResponse<ProductAttributeGroupResponse>> CreateProductAttributeGroupAsync([Body] ProductAttributeGroupFormModel ProductAttributeGroup);

        [Put("/api/admin/product-attribute-group/UpdateProductAttributeGroup")]
        Task<BaseResponse<ProductAttributeGroupResponse>> UpdateProductAttributeGroupAsync([Body] ProductAttributeGroupFormModel ProductAttributeGroup);

        [Delete("/api/admin/product-attribute-group/DeleteProductAttributeGroup/{id}")]
        Task<BaseResponse<bool>> DeleteProductAttributeGroupAsync(string id);
    }
}
