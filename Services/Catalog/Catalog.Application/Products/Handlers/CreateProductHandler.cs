using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.Products.Commands;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using Catalog.Core.Entities;
using Catalog.Core.Enums;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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

                var optionIndex = 0;
                foreach (var option in payload.Product.Options)
                {
                    var productOption = new ProductOptionValue
                    {
                        OptionId = Guid.Parse(option.Id),
                        DisplayType = option.DisplayType,
                        Value = JsonSerializer.Serialize(option.Values),
                        SortIndex = optionIndex,
                        CreatedDate = DateTime.UtcNow
                    };
                    product.AddOptionValue(productOption);

                    optionIndex++;
                }

                foreach (var attribute in payload.Product.Attributes)
                {
                    var attributeValue = new ProductAttributeValue
                    {
                        AttributeId = Guid.Parse(attribute.Id),
                        Value = attribute.Value
                    };
                    product.AddAttributeValue(attributeValue);
                }

                foreach (var categoryId in payload.Product.CategoryIds)
                {
                    var productCategory = new ProductCategory
                    {
                        CategoryId = Guid.Parse(categoryId),
                        CreatedDate = DateTime.UtcNow
                    };
                    product.AddCategory(productCategory);
                }

                _productService.MapProductLinkToProduct(payload.Product, product);

                var productPriceHistory = _productService.CreatePriceHistory(product);
                product.PriceHistories.Add(productPriceHistory);

                _logger.LogInformation("CreateProductHandler => Step 2: Upload media and map media data");
                await SaveProductMediasAsync(payload, product);

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

        private async Task SaveProductMediasAsync(CreateProductRequest request, Product product)
        {
            if (request.ThumbnailImage != null)
            {
                var fileName = await _mediaService.SaveMediaAsync(request.ThumbnailImage);
                if (product.ThumbnailImage != null)
                {
                    product.ThumbnailImage.FileName = fileName;
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
                var fileName = await _mediaService.SaveMediaAsync(file);
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
                var fileName = await _mediaService.SaveMediaAsync(file);
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
