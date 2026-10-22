using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Product;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProductApi
    {
        [Get(ApiRoutes.Product.GetAll)]
        Task<BaseResponse<List<BasicProductResponse>>> GetProductsAsync(
            int currentPage,
            int pageSize,
            string? name = null,
            bool? isFeatured = null,
            bool? isAllowToOrder = null,
            bool? isCallForPricing = null,
            bool? isPublished = null,
            string? sortBy = null,
            string? sortDirection = null);

        [Get(ApiRoutes.Product.GetById)]
        Task<BaseResponse<ProductFormModel>> GetProductAsync(string id);

        [Post(ApiRoutes.Product.Create)]
        Task<BaseResponse<ProductFormModel>> CreateProductAsync([Body] MultipartFormDataContent content);

        [Put(ApiRoutes.Product.Update)]
        Task<BaseResponse<ProductFormModel>> UpdateProductAsync(string id, [Body] MultipartFormDataContent content);

        [Delete(ApiRoutes.Product.Delete)]
        Task<BaseResponse<bool>> DeleteProductAsync(string id);

        [Post(ApiRoutes.Product.Copy)]
        Task<BaseResponse<ProductFormModel>> CopyProductAsync(string id);

        [Post(ApiRoutes.Product.UploadContentImage)]
        Task<BaseResponse<string>> UploadContentImageAsync([Body] MultipartFormDataContent content);

        [Post(ApiRoutes.Product.ImportPreview)]
        Task<BaseResponse<ProductImportPreviewResponse>> PreviewProductImportAsync([Body] MultipartFormDataContent content);

        [Post(ApiRoutes.Product.ImportCommit)]
        Task<BaseResponse<ProductImportResultResponse>> CommitProductImportAsync([Body] MultipartFormDataContent content);

        [Post(ApiRoutes.Product.ImportErrorReport)]
        Task<HttpResponseMessage> GetProductImportErrorReportAsync([Body] MultipartFormDataContent content);
    }
}
