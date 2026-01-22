using Catalog.Application.ProductAttributeGroups.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.ProductAttributeGroups.Validators
{
    public class CreateProductAttributeGroupValidator : AbstractValidator<CreateProductAttributeGroupCommand>
    {
        public CreateProductAttributeGroupValidator()
        {
            RuleFor(x => x.Payload.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"));
        }
    }
}
