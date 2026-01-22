using Catalog.Application.ProductOptions.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.ProductOptions.Validators
{
    public class CreateProductOptionValidator : AbstractValidator<CreateProductOptionCommand>
    {
        public CreateProductOptionValidator()
        {
            RuleFor(x => x.Payload.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"));
        }
    }
}
