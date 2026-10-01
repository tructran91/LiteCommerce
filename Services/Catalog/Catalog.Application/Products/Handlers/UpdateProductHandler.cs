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
            _logger.LogInformation("UpdateProductHandler: {ProductId} {ProductName}", request.Id, payload.Product.Name);

            var productId = Guid.Parse(request.Id);
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
            var slug = payload.Product.Name.Slugify();
            var isDuplicateSlug = await _productRepository.AnyAsync(p => p.Slug == slug && p.Id != productId);
            if (isDuplicateSlug)
            {
                return BaseResponse<ProductResponse>.Failure("A product with the same name already exists.", statusCode: HttpStatusCode.Conflict);
            }

            _mapper.Map(payload.Product, existingProduct);
            existingProduct.Slug = slug;

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

            var subFolder = existingProduct.Id.ToStoragePath(StorageFolder.Product);

            _logger.LogInformation("UpdateProductHandler => Step 6: Detach removed media");
            var obsoleteFiles = DetachRemovedMedias(payload.Product.DeletedMediaIds, existingProduct);

            _logger.LogInformation("UpdateProductHandler => Step 7: Upload new media");
            var uploadedFiles = new List<string>();
            try
            {
                await SaveProductMediasAsync(payload, existingProduct, subFolder, uploadedFiles, obsoleteFiles);

                _logger.LogInformation("UpdateProductHandler => Step 8: Save data");
                await _productRepository.UpdateAsync(existingProduct);
            }
            catch
            {
                await DeleteFilesAsync(uploadedFiles, subFolder);
                throw;
            }

            // Delete old files only after the DB no longer references them.
            _logger.LogInformation("UpdateProductHandler => Step 9: Delete obsolete files");
            await DeleteFilesAsync(obsoleteFiles, subFolder);

            var response = _mapper.Map<ProductResponse>(existingProduct);

            return BaseResponse<ProductResponse>.Success(response);
        }

        private static List<string> DetachRemovedMedias(IList<string>? deletedMediaIds, Product product)
        {
            var obsoleteFiles = new List<string>();
            if (deletedMediaIds == null || deletedMediaIds.Count == 0)
                return obsoleteFiles;

            foreach (var mediaId in deletedMediaIds)
            {
                var mediaGuid = Guid.Parse(mediaId);
                var productMedia = product.Medias.FirstOrDefault(m => m.Id == mediaGuid);
                if (productMedia != null)
                {
                    obsoleteFiles.Add(productMedia.Media.FileName);
                    product.Medias.Remove(productMedia);
                }
            }

            return obsoleteFiles;
        }

        private async Task DeleteFilesAsync(IEnumerable<string> fileNames, string subFolder)
        {
            foreach (var fileName in fileNames)
            {
                try
                {
                    await _mediaService.DeleteMediaAsync(fileName, subFolder);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "UpdateProductHandler => Could not delete file {FileName}", fileName);
                }
            }
        }

        private async Task SaveProductMediasAsync(UpdateProductRequest request, Product product, string subFolder,
            List<string> uploadedFiles, List<string> obsoleteFiles)
        {
            if (request.ThumbnailImage != null)
            {
                var fileName = await _mediaService.SaveMediaAsync(request.ThumbnailImage, subFolder);
                uploadedFiles.Add(fileName);

                if (product.ThumbnailImage != null)
                {
                    obsoleteFiles.Add(product.ThumbnailImage.FileName);
                    product.ThumbnailImage.FileName = fileName;
                    product.ThumbnailImage.Caption = request.ThumbnailImage.FileName;
                    product.ThumbnailImage.FileSize = request.ThumbnailImage.Length;
                }
                else
                {
                    product.ThumbnailImage = new Media
                    {
                        FileName = fileName,
                        MediaType = MediaType.Image,
                        Caption = request.ThumbnailImage.FileName,
                        FileSize = request.ThumbnailImage.Length
                    };
                }
            }

            foreach (var file in request.ProductImages ?? [])
            {
                var fileName = await _mediaService.SaveMediaAsync(file, subFolder);
                uploadedFiles.Add(fileName);
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
                uploadedFiles.Add(fileName);
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
