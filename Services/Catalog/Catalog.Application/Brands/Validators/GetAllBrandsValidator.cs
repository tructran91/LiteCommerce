using Catalog.Application.Brands.Queries;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.Brands.Validators
{
    public class GetAllBrandsValidator : AbstractValidator<GetAllBrandsQuery>
    {
        public GetAllBrandsValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(Pagination.MinPageSize).WithMessage(ValidationMessages.MustBeGreaterThan("PageSize", Pagination.MinPageSize))
                .LessThanOrEqualTo(Pagination.MaxPageSize).WithMessage(ValidationMessages.MustBeLessThanOrEqual("PageSize", Pagination.MaxPageSize));

            RuleFor(x => x.CurrentPage)
                .GreaterThan(Pagination.MinPageSize).WithMessage(ValidationMessages.MustBeGreaterThan("CurrentPage", Pagination.MinCurrentPage))
                .LessThanOrEqualTo(Pagination.MaxCurrentPage).WithMessage(ValidationMessages.MustBeLessThanOrEqual("CurrentPage", Pagination.MaxCurrentPage));
        }
    }
}
