using Catalog.Application.Categories.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.Categories.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Payload.Id)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));

            RuleFor(x => x.Payload.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"));


            RuleFor(x => x.Payload.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.MustBeGreaterThanOrEqual("DisplayOrder", 0));

            RuleFor(x => x.Payload.ParentId)
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("ParentId"))
                .When(x => !string.IsNullOrEmpty(x.Payload.ParentId?.ToString()));
        }
    }
}
