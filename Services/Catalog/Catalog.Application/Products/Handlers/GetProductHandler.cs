using AutoMapper;
using Catalog.Application.Products.Queries;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using Catalog.Application.ViewModels;
using Catalog.Core.Entities;
using Catalog.Core.Enums;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Catalog.Application.Products.Handlers
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, BaseResponse<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductHandler> _logger;

        public GetProductHandler(IProductRepository productRepository,
            IMediaService mediaService,
            IMapper mapper,
            ILogger<GetProductHandler> logger)
        {
            _productRepository = productRepository;
            _mediaService = mediaService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductAsync(request.Id);
            if (product is null)
            {
                return BaseResponse<ProductResponse>.Failure("Product does not exist.", null);
            }

            _logger.LogInformation($"GetProductHandler: product data: {JsonSerializer.Serialize(product)}");

            var productMapping = _mapper.Map<ProductResponse>(product);
            productMapping.CategoryIds = product.Categories.Select(c => c.Id.ToString()).ToList();
            productMapping.ThumbnailImageUrl = _mediaService.GetThumbnailUrl(product.ThumbnailImage);

            productMapping.Options = product.OptionValues.OrderBy(x => x.SortIndex).Select(x =>
                new ProductOptionViewModel
                {
                    Id = x.OptionId.ToString(),
                    Name = x.Option.Name,
                    DisplayType = x.DisplayType,
                    Values = JsonSerializer.Deserialize<IList<ProductOptionValueViewModel>>(x.Value)
                }).ToList();

            productMapping.Attributes = product.AttributeValues.Select(x => new ProductAttributeViewModel
            {
                AttributeValueId = x.Id.ToString(),
                Id = x.AttributeId.ToString(),
                Name = x.Attribute.Name,
                GroupName = x.Attribute.Group.Name,
                Value = x.Value
            }).ToList();

            foreach (var productMedia in product.Medias.Where(x => x.Media.MediaType == MediaType.Image))
            {
                productMapping.ProductImages.Add(new ProductMediaViewModel
                {
                    Id = productMedia.Id.ToString(),
                    MediaUrl = _mediaService.GetThumbnailUrl(productMedia.Media)
                });
            }

            foreach (var productMedia in product.Medias.Where(x => x.Media.MediaType == MediaType.File))
            {
                productMapping.ProductDocuments.Add(new ProductMediaViewModel
                {
                    Id = productMedia.Id.ToString(),
                    Caption = productMedia.Media.Caption,
                    MediaUrl = _mediaService.GetMediaUrl(productMedia.Media)
                });
            }

            foreach (var relatedProduct in product.ProductLinks.Where(x => x.LinkType == ProductLinkType.Related).Select(x => x.LinkedProduct).Where(x => !x.IsDeleted).OrderBy(x => x.Id))
            {
                productMapping.RelatedProducts.Add(new ProductLinkViewModel
                {
                    Id = relatedProduct.Id.ToString(),
                    Name = relatedProduct.Name,
                    IsPublished = relatedProduct.IsPublished
                });
            }

            foreach (var crossSellProduct in product.ProductLinks.Where(x => x.LinkType == ProductLinkType.CrossSell).Select(x => x.LinkedProduct).Where(x => !x.IsDeleted).OrderBy(x => x.Id))
            {
                productMapping.CrossSellProducts.Add(new ProductLinkViewModel
                {
                    Id = crossSellProduct.Id.ToString(),
                    Name = crossSellProduct.Name,
                    IsPublished = crossSellProduct.IsPublished
                });
            }

            return BaseResponse<ProductResponse>.Success(productMapping);
        }
    }
}
