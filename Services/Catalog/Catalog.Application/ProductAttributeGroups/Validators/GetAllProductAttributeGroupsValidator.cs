using Catalog.Application.ProductAttributeGroups.Queries;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.ProductAttributeGroups.Validators
{
    public class GetAllProductAttributeGroupsValidator : AbstractValidator<GetAllProductAttributeGroupsQuery>
    {
        public GetAllProductAttributeGroupsValidator()
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
