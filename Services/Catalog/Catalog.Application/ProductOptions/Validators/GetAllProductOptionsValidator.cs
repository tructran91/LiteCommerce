using Catalog.Application.ProductOptions.Queries;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.ProductOptions.Validators
{
    public class GetAllProductOptionsValidator : AbstractValidator<GetAllProductOptionsQuery>
    {
        public GetAllProductOptionsValidator()
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
