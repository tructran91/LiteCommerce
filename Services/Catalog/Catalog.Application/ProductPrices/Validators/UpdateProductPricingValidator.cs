using Catalog.Application.ProductPrices.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.ProductPrices.Validators
{
    public class UpdateProductPricingValidator : AbstractValidator<UpdateProductPricingCommand>
    {
        public UpdateProductPricingValidator()
        {
            RuleForEach(x => x.Payload.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.Id)
                    .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                    .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                    .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));
            });
        }
    }
}
