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
            CreateMap<CreateBrandRequest, Brand>()
                .ForMember(prop => prop.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow));
            CreateMap<UpdateBrandRequest, Brand>()
                .ForMember(prop => prop.LastModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow));

            CreateMap<Category, CategoryResponse>()
                .ForMember(prop => prop.DisplayName, opt => opt.MapFrom(o => o.Name));
            CreateMap<CreateCategoryRequest, Category>()
                .ForMember(prop => prop.ParentId, opt => opt.MapFrom(o => (string.IsNullOrEmpty(o.ParentId)) ? (Guid?)null : Guid.Parse(o.ParentId)))
                .ForMember(prop => prop.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow));
            CreateMap<UpdateCategoryRequest, Category>()
                .ForMember(prop => prop.ParentId, opt => opt.MapFrom(o => (string.IsNullOrEmpty(o.ParentId)) ? (Guid?)null : Guid.Parse(o.ParentId)))
                .ForMember(prop => prop.LastModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow));
            CreateMap<Category, BasicCategoryResponse>()
                .ForMember(prop => prop.DisplayName, opt => opt.MapFrom(o => o.Name));

            CreateMap<Product, ProductResponse>();
            CreateMap<ProductViewModel, Product>()
                .ForMember(prop => prop.Id, opt => opt.Ignore())
                .ForMember(prop => prop.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow));
            CreateMap<Product, ProductViewModel>();
        }
    }
}
