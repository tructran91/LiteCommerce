using AutoMapper;
using Catalog.Application.ActivityLogs.Queries;
using Catalog.Application.AuditLogs.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using Catalog.Application.ViewModels;
using Catalog.Core.DTOs;
using Catalog.Core.Entities;
using System.Text.Json;

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
                .ForMember(prop => prop.ThumbnailImageUrl, opt => opt.Ignore());
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

            CreateMap<SeedResult, SeedDatabaseResponse>();

            // Log timestamps are stored as UTC but read back as Unspecified; mark them so the JSON carries the Z suffix.
            CreateMap<GetAllAuditLogsQuery, AuditLogFilter>();
            CreateMap<AuditLog, AuditLogResponse>()
                .ForMember(prop => prop.Action, opt => opt.MapFrom(src => src.Action.ToString()))
                .ForMember(prop => prop.Changes, opt => opt.MapFrom(src => ParseAuditChanges(src.Changes)))
                .ForMember(prop => prop.Timestamp, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.Timestamp, DateTimeKind.Utc)));

            CreateMap<GetAllActivityLogsQuery, ActivityLogFilter>();
            CreateMap<ActivityLog, ActivityLogResponse>()
                .ForMember(prop => prop.Action, opt => opt.MapFrom(src => src.Action.ToString()))
                .ForMember(prop => prop.Timestamp, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.Timestamp, DateTimeKind.Utc)));
        }

        // AuditLog.Changes is the JSON written by AuditLogInterceptor; values keep their JSON type there.
        private static List<AuditPropertyChangeResponse> ParseAuditChanges(string changes)
        {
            if (string.IsNullOrWhiteSpace(changes)) return new();

            var items = JsonSerializer.Deserialize<List<AuditChangeJson>>(changes) ?? new();
            return items
                .Select(x => new AuditPropertyChangeResponse
                {
                    Property = x.Property,
                    OldValue = ToDisplayString(x.OldValue),
                    NewValue = ToDisplayString(x.NewValue)
                })
                .ToList();
        }

        private static string? ToDisplayString(JsonElement? value)
        {
            return value?.ValueKind switch
            {
                null or JsonValueKind.Null or JsonValueKind.Undefined => null,
                JsonValueKind.String => value.Value.GetString(),
                _ => value.Value.GetRawText()
            };
        }

        private sealed record AuditChangeJson(string Property, JsonElement? OldValue, JsonElement? NewValue);
    }
}
