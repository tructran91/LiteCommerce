using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.Products.Commands;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using Catalog.Core.Entities;
using Catalog.Core.Enums;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.Products.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, BaseResponse<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediaService _mediaService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(IProductRepository productRepository,
            IMediaService mediaService,
            IProductService productService,
            IMapper mapper,
            ILogger<UpdateProductHandler> logger)
        {
            _productRepository = productRepository;
            _mediaService = mediaService;
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var payload = request.Payload;
            _logger.LogInformation("UpdateProductHandler: {Payload}", JsonSerializer.Serialize(payload));

            try
            {
                var productId = Guid.Parse(payload.Product.Id);
                var existingProduct = await _productRepository.GetProductAsync(productId);
                if (existingProduct is null)
                {
                    return BaseResponse<ProductResponse>.Failure("Product does not exist.", statusCode: HttpStatusCode.NotFound);
                }

                // Capture original prices BEFORE mapping
                var originalPrice = existingProduct.Price;
                var originalOldPrice = existingProduct.OldPrice;
                var originalSpecialPrice = existingProduct.SpecialPrice;
                var originalSpecialPriceStart = existingProduct.SpecialPriceStart;
                var originalSpecialPriceEnd = existingProduct.SpecialPriceEnd;

                _logger.LogInformation("UpdateProductHandler => Step 1: Update basic info");
                _mapper.Map(payload.Product, existingProduct);
                existingProduct.Slug = existingProduct.Name.Slugify();

                // Compare with original values (before mapping overwrote them)
                var hasPriceChanged = originalPrice != existingProduct.Price
                    || originalOldPrice != existingProduct.OldPrice
                    || originalSpecialPrice != existingProduct.SpecialPrice
                    || originalSpecialPriceStart != existingProduct.SpecialPriceStart
                    || originalSpecialPriceEnd != existingProduct.SpecialPriceEnd;

                if (hasPriceChanged)
                {
                    var priceHistory = _productService.CreatePriceHistory(existingProduct);
                    existingProduct.PriceHistories.Add(priceHistory);
                }

                _logger.LogInformation("UpdateProductHandler => Step 2: Update options");
                _productService.AddOrDeleteOptions(payload.Product, existingProduct);

                _logger.LogInformation("UpdateProductHandler => Step 3: Update attributes");
                _productService.AddOrDeleteAttributes(payload.Product, existingProduct);

                _logger.LogInformation("UpdateProductHandler => Step 4: Update categories");
                _productService.AddOrDeleteCategories(payload.Product, existingProduct);

                _logger.LogInformation("UpdateProductHandler => Step 5: Update product links");
                _productService.AddOrDeleteProductLinks(payload.Product, existingProduct);

                _logger.LogInformation("UpdateProductHandler => Step 6: Delete removed media");
                await DeleteRemovedMediasAsync(payload.Product.DeletedMediaIds, existingProduct);

                _logger.LogInformation("UpdateProductHandler => Step 7: Upload new media");
                await SaveProductMediasAsync(payload, existingProduct);

                _logger.LogInformation("UpdateProductHandler => Step 8: Save data");
                await _productRepository.UpdateAsync(existingProduct);
                var response = _mapper.Map<ProductResponse>(existingProduct);

                return BaseResponse<ProductResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateProductHandler => Error: {Message}", ex.Message);
                return BaseResponse<ProductResponse>.Failure(ex.Message);
            }
        }

        private async Task DeleteRemovedMediasAsync(IList<string>? deletedMediaIds, Product product)
        {
            if (deletedMediaIds == null || deletedMediaIds.Count == 0)
                return;

            var subFolder = product.Id.ToStoragePath(StorageFolder.Product);

            foreach (var mediaId in deletedMediaIds)
            {
                var mediaGuid = Guid.Parse(mediaId);
                var productMedia = product.Medias.FirstOrDefault(m => m.Id == mediaGuid);
                if (productMedia != null)
                {
                    await _mediaService.DeleteMediaAsync(productMedia.Media.FileName);
                    product.Medias.Remove(productMedia);
                }
            }
        }

        private async Task SaveProductMediasAsync(UpdateProductRequest request, Product product)
        {
            var subFolder = product.Id.ToStoragePath(StorageFolder.Product);

            if (request.ThumbnailImage != null)
            {
                // Delete old thumbnail if exists
                if (product.ThumbnailImage != null)
                {
                    await _mediaService.DeleteMediaAsync(product.ThumbnailImage.FileName);
                }

                var fileName = await _mediaService.SaveMediaAsync(request.ThumbnailImage, subFolder);
                if (product.ThumbnailImage != null)
                {
                    product.ThumbnailImage.FileName = fileName;
                    product.ThumbnailImage.Caption = request.ThumbnailImage.FileName;
                    product.ThumbnailImage.FileSize = request.ThumbnailImage.Length;
                    product.ThumbnailImage.LastModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    product.ThumbnailImage = new Media
                    {
                        FileName = fileName,
                        MediaType = MediaType.Image,
                        Caption = request.ThumbnailImage.FileName,
                        FileSize = request.ThumbnailImage.Length,
                        CreatedDate = DateTime.UtcNow
                    };
                }
            }

            foreach (var file in request.ProductImages ?? [])
            {
                var fileName = await _mediaService.SaveMediaAsync(file, subFolder);
                var productMedia = new ProductMedia
                {
                    Product = product,
                    Media = new Media
                    {
                        FileName = fileName,
                        MediaType = MediaType.Image,
                        Caption = file.FileName,
                        FileSize = file.Length,
                        CreatedDate = DateTime.UtcNow
                    },
                    CreatedDate = DateTime.UtcNow
                };
                product.AddMedia(productMedia);
            }

            foreach (var file in request.ProductDocuments ?? [])
            {
                var fileName = await _mediaService.SaveMediaAsync(file, subFolder);
                var productMedia = new ProductMedia
                {
                    Product = product,
                    Media = new Media
                    {
                        FileName = fileName,
                        MediaType = MediaType.File,
                        Caption = file.FileName,
                        FileSize = file.Length,
                        CreatedDate = DateTime.UtcNow
                    },
                    CreatedDate = DateTime.UtcNow
                };
                product.AddMedia(productMedia);
            }
        }
    }
}
