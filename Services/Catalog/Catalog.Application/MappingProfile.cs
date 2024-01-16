using AutoMapper;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Brand, BrandResponse>();
            CreateMap<CreateBrandRequest, Brand>()
                .ForMember(prop => prop.CreatedOn, opt => opt.MapFrom(o => DateTime.UtcNow));
            CreateMap<UpdateBrandRequest, Brand>()
                .ForMember(prop => prop.UpdatedOn, opt => opt.MapFrom(o => DateTime.UtcNow));

            //CreateMap<Category, CategoryDto>();
            //CreateMap<CategoryDto, Category>()
            //    .ForMember(prop => prop.Id, opt => opt.MapFrom(o => ((o.Id == null || o.Id == Guid.Empty) ? Guid.NewGuid() : o.Id)))
            //    .ForMember(prop => prop.ParentId, opt => opt.MapFrom(o => ((o.ParentId == null || o.ParentId == Guid.Empty) ? (Guid?)null : o.ParentId)))
            //    .ForMember(prop => prop.IsDeleted, opt => opt.MapFrom(o => false))
            //    .ForMember(prop => prop.CreatedOn, opt => opt.MapFrom(o => DateTime.UtcNow))
            //    .AfterMap((src, dest) => dest.LatestUpdatedOn = dest.CreatedOn);

            //CreateMap<Product, ProductDto>();
            //CreateMap<ProductDto, Product>()
            //    .ForMember(prop => prop.Id, opt => opt.MapFrom(o => ((o.Id == null || o.Id == Guid.Empty) ? Guid.NewGuid() : o.Id)))
            //    .ForMember(prop => prop.IsDeleted, opt => opt.MapFrom(o => false))
            //    .ForMember(prop => prop.CreatedOn, opt => opt.MapFrom(o => DateTime.UtcNow))
            //    .AfterMap((src, dest) => dest.LatestUpdatedOn = dest.CreatedOn);
        }
    }
}
