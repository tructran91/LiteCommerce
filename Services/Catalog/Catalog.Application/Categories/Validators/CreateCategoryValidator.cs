using Catalog.Application.Categories.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.Categories.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {

            RuleFor(x => x.Payload.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"));

            RuleFor(x => x.Payload.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.MustBeGreaterThanOrEqual("DisplayOrder", PaginationSetting.MinCurrentPage));

            RuleFor(x => x.Payload.ParentId)
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("ParentId"))
                .When(x => !string.IsNullOrEmpty(x.Payload.ParentId?.ToString()));
        }
    }
}
