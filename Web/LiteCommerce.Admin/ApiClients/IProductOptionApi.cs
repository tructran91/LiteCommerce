using LiteCommerce.Admin.Models.Business.ProductOption;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductOptionApi
    {
        [Get("/api/admin/product-option/GetAllProductOptions")]
        Task<BaseResponse<List<ProductOptionResponse>>> GetProductOptionsAsync(int currentPage, int pageSize);

        [Get("/api/admin/product-option/GetProductOptionById/{id}")]
        Task<BaseResponse<ProductOptionResponse>> GetProductOptionAsync(string id);

        [Post("/api/admin/product-option/CreateProductOption")]
        Task<BaseResponse<ProductOptionResponse>> CreateProductOptionAsync([Body] ProductOptionFormModel ProductOption);

        [Put("/api/admin/product-option/UpdateProductOption")]
        Task<BaseResponse<ProductOptionResponse>> UpdateProductOptionAsync([Body] ProductOptionFormModel ProductOption);

        [Delete("/api/admin/product-option/DeleteProductOption/{id}")]
        Task<BaseResponse<bool>> DeleteProductOptionAsync(string id);
    }
}
