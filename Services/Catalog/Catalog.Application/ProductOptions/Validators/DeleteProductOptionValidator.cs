using Catalog.Application.ProductOptions.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.ProductOptions.Validators
{
    public class DeleteProductOptionValidator : AbstractValidator<DeleteProductOptionCommand>
    {
        public DeleteProductOptionValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));
        }
    }
}
