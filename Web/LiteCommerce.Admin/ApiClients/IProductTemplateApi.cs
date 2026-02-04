using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ProductTemplate;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductTemplateApi
    {
        [Get(ApiRoutes.ProductTemplate.GetAll)]
        Task<BaseResponse<List<ProductTemplateResponse>>> GetProductTemplatesAsync(int currentPage, int pageSize);

        [Get(ApiRoutes.ProductTemplate.GetById)]
        Task<BaseResponse<ProductTemplateResponse>> GetProductTemplateAsync(string id);

        [Post(ApiRoutes.ProductTemplate.Create)]
        Task<BaseResponse<ProductTemplateResponse>> CreateProductTemplateAsync([Body] ProductTemplateFormModel productTemplate);

        [Put(ApiRoutes.ProductTemplate.Update)]
        Task<BaseResponse<ProductTemplateResponse>> UpdateProductTemplateAsync([Body] ProductTemplateFormModel productTemplate);

        [Delete(ApiRoutes.ProductTemplate.Delete)]
        Task<BaseResponse<bool>> DeleteProductTemplateAsync(string id);
    }
}
