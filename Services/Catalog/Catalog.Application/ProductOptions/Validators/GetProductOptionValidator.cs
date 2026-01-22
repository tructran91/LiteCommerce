using Catalog.Application.ProductOptions.Queries;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.ProductOptions.Validators
{
    public class GetProductOptionValidator : AbstractValidator<GetProductOptionQuery>
    {
        public GetProductOptionValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));
        }
    }
}
