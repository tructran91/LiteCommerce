using Catalog.Application.ProductAttributes.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.ProductAttributes.Validators
{
    public class UpdateProductAttributeValidator : AbstractValidator<UpdateProductAttributeCommand>
    {
        public UpdateProductAttributeValidator()
        {
            RuleFor(x => x.Payload.Id)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));

            RuleFor(x => x.Payload.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"));

            RuleFor(x => x.Payload.GroupId)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("GroupId"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("GroupId"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("GroupId"));
        }
    }
}
