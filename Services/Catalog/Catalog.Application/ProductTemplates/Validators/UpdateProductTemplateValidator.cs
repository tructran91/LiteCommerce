using Catalog.Application.ProductTemplates.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.ProductTemplates.Validators
{
    public class UpdateProductTemplateValidator : AbstractValidator<UpdateProductTemplateCommand>
    {
        public UpdateProductTemplateValidator()
        {
            RuleFor(x => x.Payload.Id)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));

            RuleFor(x => x.Payload.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"));
        }
    }
}
