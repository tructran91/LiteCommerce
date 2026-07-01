using AutoMapper;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Application.ViewModels;
using Catalog.Core.Entities;

namespace Catalog.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Brand, BrandResponse>();
            CreateMap<CreateBrandRequest, Brand>();
            CreateMap<UpdateBrandRequest, Brand>();

            CreateMap<Category, CategoryResponse>()
                .ForMember(prop => prop.DisplayName, opt => opt.MapFrom(o => o.Name))
                .ForMember(prop => prop.ThumbnailImageUrl, opt => opt.MapFrom(o => o.ThumbnailImage.FileName));
            CreateMap<CreateCategoryRequest, Category>()
                .ForMember(prop => prop.ThumbnailImage, opt => opt.Ignore())
                .ForMember(prop => prop.ParentId, opt => opt.MapFrom(o => (string.IsNullOrEmpty(o.ParentId)) ? (Guid?)null : Guid.Parse(o.ParentId)));
            CreateMap<UpdateCategoryRequest, Category>()
                .ForMember(prop => prop.ThumbnailImage, opt => opt.Ignore())
                .ForMember(prop => prop.ParentId, opt => opt.MapFrom(o => (string.IsNullOrEmpty(o.ParentId)) ? (Guid?)null : Guid.Parse(o.ParentId)));
            CreateMap<Category, BasicCategoryResponse>()
                .ForMember(prop => prop.DisplayName, opt => opt.MapFrom(o => o.Name));

            CreateMap<ProductAttributeGroup, ProductAttributeGroupResponse>();
            CreateMap<CreateProductAttributeGroupRequest, ProductAttributeGroup>();
            CreateMap<UpdateProductAttributeGroupRequest, ProductAttributeGroup>();

            CreateMap<ProductAttribute, ProductAttributeOverviewViewModel>();
            CreateMap<ProductAttribute, ProductAttributeResponse>();
            CreateMap<CreateProductAttributeRequest, ProductAttribute>();
            CreateMap<UpdateProductAttributeRequest, ProductAttribute>();

            CreateMap<ProductOption, ProductOptionResponse>();
            CreateMap<CreateProductOptionRequest, ProductOption>();
            CreateMap<UpdateProductOptionRequest, ProductOption>();

            CreateMap<ProductTemplate, ProductTemplateResponse>()
                .ForMember(prop => prop.ProductAttributes, opt => opt.Ignore());
            CreateMap<CreateProductTemplateRequest, ProductTemplate>()
                .ForMember(prop => prop.ProductAttributes, opt => opt.Ignore());
            CreateMap<UpdateProductTemplateRequest, ProductTemplate>();

            CreateMap<Product, ProductResponse>();
            CreateMap<ProductViewModel, Product>()
                .ForMember(prop => prop.Id, opt => opt.Ignore())
                .ForMember(prop => prop.BrandId, opt => opt.MapFrom(src => Guid.Parse(src.BrandId)));
            CreateMap<Product, ProductViewModel>();

            CreateMap<Product, ProductPricingResponse>();
        }
    }
}
