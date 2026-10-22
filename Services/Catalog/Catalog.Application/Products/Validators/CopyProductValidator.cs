using Catalog.Application.Products.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.Products.Validators
{
    public class CopyProductValidator : AbstractValidator<CopyProductCommand>
    {
        public CopyProductValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));
        }
    }
}
