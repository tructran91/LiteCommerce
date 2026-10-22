using Catalog.Application.Products.Queries;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.Products.Validators
{
    public class GetAllProductsValidator : AbstractValidator<GetAllProductsQuery>
    {
        private static readonly string[] AllowedSortFields =
        [
            "name",
            "isFeatured",
            "isAllowToOrder",
            "isCallForPricing",
            "isPublished"
        ];

        public GetAllProductsValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(PaginationSetting.MinPageSize).WithMessage(ValidationMessages.MustBeGreaterThan("PageSize", PaginationSetting.MinPageSize))
                .LessThanOrEqualTo(PaginationSetting.MaxPageSize).WithMessage(ValidationMessages.MustBeLessThanOrEqual("PageSize", PaginationSetting.MaxPageSize));

            RuleFor(x => x.CurrentPage)
                .GreaterThan(PaginationSetting.MinCurrentPage).WithMessage(ValidationMessages.MustBeGreaterThan("CurrentPage", PaginationSetting.MinCurrentPage))
                .LessThanOrEqualTo(PaginationSetting.MaxCurrentPage).WithMessage(ValidationMessages.MustBeLessThanOrEqual("CurrentPage", PaginationSetting.MaxCurrentPage));

            RuleFor(x => x.SortBy)
                .Must(value => value is null || AllowedSortFields.Contains(value, StringComparer.OrdinalIgnoreCase))
                .WithMessage($"SortBy must be one of: {string.Join(", ", AllowedSortFields)}.");

            RuleFor(x => x.SortDirection)
                .Must(value => value is null || value.Equals("asc", StringComparison.OrdinalIgnoreCase) || value.Equals("desc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortDirection must be asc or desc.");

            RuleFor(x => x.SortDirection)
                .Empty()
                .When(x => string.IsNullOrWhiteSpace(x.SortBy))
                .WithMessage("SortDirection requires SortBy.");
        }
    }
}
