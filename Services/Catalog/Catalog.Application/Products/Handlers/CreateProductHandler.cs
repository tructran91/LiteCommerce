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
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Catalog.Application.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, BaseResponse<ProductResponse>>
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IMediaService _mediaService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(IBaseRepository<Product> productRepository,
            IMediaService mediaService,
            IProductService productService,
            IMapper mapper,
            ILogger<CreateProductHandler> logger)
        {
            _productRepository = productRepository;
            _mediaService = mediaService;
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var payload = request.Payload;
            _logger.LogInformation($"CreateProductHandler: {JsonSerializer.Serialize(payload)}");

            try
            {
                _logger.LogInformation("CreateProductHandler => Step 1: Map data");
                var product = _mapper.Map<Product>(payload.Product);
                product.Slug = product.Name.Slugify();

                if (product.Id == Guid.Empty)
                {
                    product.Id = Guid.NewGuid();
                }

                _productService.AddOrDeleteOptions(payload.Product, product);
                _productService.AddOrDeleteAttributes(payload.Product, product);
                _productService.AddOrDeleteCategories(payload.Product, product);
                _productService.AddOrDeleteProductLinks(payload.Product, product);

                var productPriceHistory = _productService.CreatePriceHistory(product);
                product.PriceHistories.Add(productPriceHistory);

                _logger.LogInformation("CreateProductHandler => Step 2: Upload media and map media data");
                await SaveProductMediasAsync(payload, product);

                _logger.LogInformation("CreateProductHandler => Step 2.1: Move content images from temp to product folder");
                await MoveContentImagesFromTempAsync(payload, product);

                _logger.LogInformation("CreateProductHandler => Step 3: Save data");
                var createdProduct = await _productRepository.AddAsync(product);
                var response = _mapper.Map<ProductResponse>(createdProduct);

                return BaseResponse<ProductResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CreateProductHandler => Error: {ex.Message}");
                return BaseResponse<ProductResponse>.Failure(ex.Message);
            }
        }

        private async Task MoveContentImagesFromTempAsync(CreateProductRequest request, Product product)
        {
            if (string.IsNullOrEmpty(request.ContentTempId))
                return;

            var tempFolder = Path.Combine(StorageFolder.Product, StorageFolder.TempContent, request.ContentTempId);
            var productFolder = product.Id.ToStoragePath(StorageFolder.Product);

            await _mediaService.MoveContentImagesAsync(tempFolder, productFolder);

            // Replace temp URLs with product URLs in description
            if (!string.IsNullOrEmpty(product.Description))
            {
                var tempPathSegment = $"{StorageFolder.Product}/{StorageFolder.TempContent}/{request.ContentTempId}/";

                // Match URLs containing the temp path, capturing domain prefix, filename, and optional SAS query string
                var pattern = $@"(?<prefix>[^""\s]*?){Regex.Escape(tempPathSegment)}(?<fileName>[^""\s?]+)(?<suffix>\?[^""\s]*)?";

                product.Description = Regex.Replace(product.Description, pattern, match =>
                {
                    var fileName = match.Groups["fileName"].Value;
                    var prefix = match.Groups["prefix"].Value;
                    var productPath = $"{StorageFolder.Product}/{product.Id}/";

                    // Azure URLs have SAS tokens — regenerate entirely
                    // Local URLs are relative or have a domain prefix — preserve the prefix
                    return match.Groups["suffix"].Success
                        ? _mediaService.GetMediaUrl(fileName, productFolder)
                        : $"{prefix}{productPath}{fileName}";
                });
            }
        }

        private async Task SaveProductMediasAsync(CreateProductRequest request, Product product)
        {
            var subFolder = product.Id.ToStoragePath(StorageFolder.Product);

            if (request.ThumbnailImage != null)
            {
                var fileName = await _mediaService.SaveMediaAsync(request.ThumbnailImage, subFolder);
                if (product.ThumbnailImage != null)
                {
                    product.ThumbnailImage.FileName = fileName;
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

            foreach (var file in request.ProductImages)
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

            foreach (var file in request.ProductDocuments)
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
