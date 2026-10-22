using Catalog.Application.Extensions;
using Catalog.Application.Products.Commands;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using Catalog.Application.ViewModels;
using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Catalog.Core.Enums;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Catalog.Application.Products.Handlers
{
    public class CopyProductHandler : IRequestHandler<CopyProductCommand, BaseResponse<ProductResponse>>
    {
        private const string CopyNameDateFormat = "dd-MM-yyyy-HH-mm-ss";

        private readonly IProductRepository _productRepository;
        private readonly IMediaService _mediaService;
        private readonly IProductService _productService;
        private readonly ILogger<CopyProductHandler> _logger;

        public CopyProductHandler(
            IProductRepository productRepository,
            IMediaService mediaService,
            IProductService productService,
            ILogger<CopyProductHandler> logger)
        {
            _productRepository = productRepository;
            _mediaService = mediaService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductResponse>> Handle(CopyProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CopyProductHandler: {Request}", JsonSerializer.Serialize(request));

            var source = await _productRepository.GetProductAsync(Guid.Parse(request.Id));
            if (source is null)
            {
                return BaseResponse<ProductResponse>.Failure("Product does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            foreach (var option in source.OptionValues)
            {
                JsonSerializer.Deserialize<IList<ProductOptionValueViewModel>>(option.Value);
            }

            var copiedProduct = CreateCopy(source);
            request.EntityDisplayName = copiedProduct.Name;
            request.LogDescription = $"Copied Product '{source.Name}' ({source.Id}) to '{copiedProduct.Name}'";

            if (await _productRepository.AnyAsync(p => p.Slug == copiedProduct.Slug))
            {
                return BaseResponse<ProductResponse>.Failure("A product with the same name already exists.", statusCode: HttpStatusCode.Conflict);
            }

            var sourceFolder = source.Id.ToStoragePath(StorageFolder.Product);
            var destinationFolder = copiedProduct.Id.ToStoragePath(StorageFolder.Product);

            try
            {
                await _mediaService.CopyFolderContentAsync(sourceFolder, destinationFolder);
                copiedProduct.Description = RewriteSourceUrls(source.Description, source.Id, copiedProduct.Id, destinationFolder);
                copiedProduct.OgImage = RewriteSourceUrls(source.OgImage, source.Id, copiedProduct.Id, destinationFolder);
                await _productRepository.AddAsync(copiedProduct);
            }
            catch
            {
                await DeleteDestinationFolderAsync(destinationFolder);
                throw;
            }

            return BaseResponse<ProductResponse>.Success(_productService.BuildProductResponse(copiedProduct), statusCode: HttpStatusCode.Created);
        }

        private Product CreateCopy(Product source)
        {
            var suffix = $" - {GetVietnamTime().ToString(CopyNameDateFormat)}";
            var sourceNameLength = Math.Min(source.Name.Length, FieldLength.ProductName - suffix.Length);
            var name = source.Name[..sourceNameLength] + suffix;

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Slug = name.Slugify(),
                ShortDescription = source.ShortDescription,
                Description = source.Description,
                IsPublished = false,
                Price = source.Price,
                OldPrice = source.OldPrice,
                SpecialPrice = source.SpecialPrice,
                SpecialPriceStart = source.SpecialPriceStart,
                SpecialPriceEnd = source.SpecialPriceEnd,
                IsFeatured = source.IsFeatured,
                IsCallForPricing = source.IsCallForPricing,
                IsAllowToOrder = source.IsAllowToOrder,
                StockQuantity = 0,
                Sku = null,
                Gtin = null,
                DisplayOrder = source.DisplayOrder,
                MetaTitle = source.MetaTitle,
                MetaKeywords = source.MetaKeywords,
                MetaDescription = source.MetaDescription,
                CanonicalUrl = null,
                OgTitle = source.OgTitle,
                OgDescription = source.OgDescription,
                OgImage = source.OgImage,
                OgUrl = null,
                SchemaJson = null,
                BrandId = source.BrandId,
                ThumbnailImage = CloneMedia(source.ThumbnailImage)
            };

            foreach (var sourceMedia in source.Medias.Where(x => x.Media != null))
            {
                product.AddMedia(new ProductMedia { Media = CloneMedia(sourceMedia.Media)! });
            }

            foreach (var category in source.Categories)
            {
                product.AddCategory(new ProductCategory
                {
                    CategoryId = category.CategoryId,
                    DisplayOrder = category.DisplayOrder,
                    IsFeaturedProduct = category.IsFeaturedProduct
                });
            }

            foreach (var attribute in source.AttributeValues)
            {
                product.AddAttributeValue(new ProductAttributeValue
                {
                    AttributeId = attribute.AttributeId,
                    Attribute = attribute.Attribute,
                    Value = attribute.Value
                });
            }

            foreach (var option in source.OptionValues)
            {
                product.AddOptionValue(new ProductOptionValue
                {
                    OptionId = option.OptionId,
                    Option = option.Option,
                    Value = option.Value,
                    DisplayType = option.DisplayType,
                    SortIndex = option.SortIndex
                });
            }

            foreach (var link in source.ProductLinks.Where(x => x.LinkedProduct != null && !x.LinkedProduct.IsDeleted))
            {
                product.AddProductLinks(new ProductLink
                {
                    LinkedProductId = link.LinkedProductId,
                    LinkedProduct = link.LinkedProduct,
                    LinkType = link.LinkType
                });
            }

            product.PriceHistories.Add(_productService.CreatePriceHistory(product));
            return product;
        }

        private string? RewriteSourceUrls(string? value, Guid sourceId, Guid destinationId, string destinationFolder)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var sourcePath = $"{StorageFolder.Product}/{sourceId}/";
            var destinationPath = $"{StorageFolder.Product}/{destinationId}/";
            var pattern = $@"(?<prefix>[^""'\s]*?){Regex.Escape(sourcePath)}(?<fileName>[^""'\s?]+)(?<suffix>\?[^""'\s]*)?";

            return Regex.Replace(value, pattern, match =>
            {
                var fileName = match.Groups["fileName"].Value;
                return match.Groups["suffix"].Success
                    ? _mediaService.GetMediaUrl(fileName, destinationFolder)
                    : $"{match.Groups["prefix"].Value}{destinationPath}{fileName}";
            });
        }

        private async Task DeleteDestinationFolderAsync(string destinationFolder)
        {
            try
            {
                await _mediaService.DeleteFolderAsync(destinationFolder);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "CopyProductHandler => Could not clean up folder {Folder}", destinationFolder);
            }
        }

        private static Media? CloneMedia(Media? source)
        {
            return source is null
                ? null
                : new Media
                {
                    Caption = source.Caption,
                    FileSize = source.FileSize,
                    FileName = source.FileName,
                    MediaType = source.MediaType
                };
        }

        private static DateTime GetVietnamTime()
        {
            var timeZoneId = OperatingSystem.IsWindows() ? "SE Asia Standard Time" : "Asia/Ho_Chi_Minh";
            return TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out var timeZone)
                ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone)
                : DateTime.UtcNow;
        }
    }
}
