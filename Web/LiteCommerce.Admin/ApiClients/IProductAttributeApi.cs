using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductAttributeApi
    {
        [Get("/api/admin/product-attribute/GetAllProductAttributes")]
        Task<BaseResponse<List<ProductAttributeResponse>>> GetProductAttributesAsync(int currentPage, int pageSize);

        [Get("/api/admin/product-attribute/GetProductAttributeById/{id}")]
        Task<BaseResponse<ProductAttributeResponse>> GetProductAttributeAsync(string id);

        [Post("/api/admin/product-attribute/CreateProductAttribute")]
        Task<BaseResponse<ProductAttributeResponse>> CreateProductAttributeAsync([Body] ProductAttributeFormModel ProductAttribute);

        [Put("/api/admin/product-attribute/UpdateProductAttribute")]
        Task<BaseResponse<ProductAttributeResponse>> UpdateProductAttributeAsync([Body] ProductAttributeFormModel ProductAttribute);

        [Delete("/api/admin/product-attribute/DeleteProductAttribute/{id}")]
        Task<BaseResponse<bool>> DeleteProductAttributeAsync(string id);
    }
}
