using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IBrandApi
    {
        [Get(ApiRoutes.Brand.GetAll)]
        Task<BaseResponse<List<BrandResponse>>> GetBrandsAsync(int currentPage, int pageSize);

        [Get(ApiRoutes.Brand.GetById)]
        Task<BaseResponse<BrandResponse>> GetBrandAsync(string id);

        [Post(ApiRoutes.Brand.Create)]
        Task<BaseResponse<BrandResponse>> CreateBrandAsync([Body] BrandFormModel brand);

        [Put(ApiRoutes.Brand.Update)]
        Task<BaseResponse<BrandResponse>> UpdateBrandAsync([Body] BrandFormModel brand);

        [Delete(ApiRoutes.Brand.Delete)]
        Task<BaseResponse<bool>> DeleteBrandAsync(string id);
    }
}
