using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IBrandApi
    {
        [Get(ApiRoutes.Brand.GetAll)]
        Task<BaseResponse<List<BrandModel>>> GetBrandsAsync(int currentPage, int pageSize);

        [Get(ApiRoutes.Brand.GetById)]
        Task<BaseResponse<BrandModel>> GetBrandAsync(string id);

        [Post(ApiRoutes.Brand.Create)]
        Task<BaseResponse<BrandModel>> CreateBrandAsync([Body] BrandModel brand);

        [Put(ApiRoutes.Brand.Update)]
        Task<BaseResponse<BrandModel>> UpdateBrandAsync([Body] BrandModel brand);

        [Delete(ApiRoutes.Brand.Delete)]
        Task<BaseResponse<bool>> DeleteBrandAsync(string id);
    }
}
