using Catalog.Application.Brands.Queries;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.Categories.Validators
{
    public class GetAllCategoriesValidator : AbstractValidator<GetAllCategoriesQuery>
    {
        public GetAllCategoriesValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(PaginationSetting.MinPageSize).WithMessage(ValidationMessages.MustBeGreaterThan("PageSize", PaginationSetting.MinPageSize))
                .LessThanOrEqualTo(PaginationSetting.MaxPageSize).WithMessage(ValidationMessages.MustBeLessThanOrEqual("PageSize", PaginationSetting.MaxPageSize));

            RuleFor(x => x.CurrentPage)
                .GreaterThan(PaginationSetting.MinPageSize).WithMessage(ValidationMessages.MustBeGreaterThan("CurrentPage", PaginationSetting.MinCurrentPage))
                .LessThanOrEqualTo(PaginationSetting.MaxCurrentPage).WithMessage(ValidationMessages.MustBeLessThanOrEqual("CurrentPage", PaginationSetting.MaxCurrentPage));
        }
    }
}
