using Catalog.Application.Brands.Queries;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.Categories.Validators
{
    public class GetCategoryValidator : AbstractValidator<GetCategoryQuery>
    {
        public GetCategoryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));
        }
    }
}
