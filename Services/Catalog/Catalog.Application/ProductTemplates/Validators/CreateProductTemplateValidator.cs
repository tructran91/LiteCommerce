using Catalog.Application.ProductTemplates.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.ProductTemplates.Validators
{
    public class CreateProductTemplateValidator : AbstractValidator<CreateProductTemplateCommand>
    {
        public CreateProductTemplateValidator()
        {
            RuleFor(x => x.Payload.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"));
        }
    }
}
