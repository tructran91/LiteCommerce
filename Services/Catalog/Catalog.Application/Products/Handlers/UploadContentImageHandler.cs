using Catalog.Application.Extensions;
using Catalog.Application.Products.Commands;
using Catalog.Application.Services;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Catalog.Application.Products.Handlers
{
    public class UploadContentImageHandler : IRequestHandler<UploadContentImageCommand, BaseResponse<string>>
    {
        private readonly IMediaService _mediaService;
        private readonly ILogger<UploadContentImageHandler> _logger;

        public UploadContentImageHandler(IMediaService mediaService, ILogger<UploadContentImageHandler> logger)
        {
            _mediaService = mediaService;
            _logger = logger;
        }

        public async Task<BaseResponse<string>> Handle(UploadContentImageCommand request, CancellationToken cancellationToken)
        {
            var payload = request.Payload;
            _logger.LogInformation($"UploadContentImageHandler: {JsonSerializer.Serialize(payload)}");

            try
            {
                var id = Guid.Parse(payload.ProductId);
                string subFolder;

                if (payload.IsNewProduct)
                {
                    subFolder = Path.Combine(StorageFolder.Product, StorageFolder.TempContent, id.ToString());
                }
                else
                {
                    subFolder = id.ToStoragePath(StorageFolder.Product);
                }

                var fileName = await _mediaService.SaveMediaAsync(payload.File, subFolder);
                var url = _mediaService.GetMediaUrl(fileName, subFolder);

                return BaseResponse<string>.Success(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UploadContentImageHandler => Error: {Message}", ex.Message);
                return BaseResponse<string>.Failure(ex.Message);
            }
        }
    }
}
