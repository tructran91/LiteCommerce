using Catalog.Application.Products.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.Products.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {

            RuleFor(x => x.Payload.Product.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"));

            RuleFor(x => x.Payload.Product.BrandId)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Brand"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Brand"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));
        }
    }
}
