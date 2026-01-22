using Catalog.Application.Products.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.Products.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {

            RuleFor(x => x.Payload.Product.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"));
        }
    }
}
