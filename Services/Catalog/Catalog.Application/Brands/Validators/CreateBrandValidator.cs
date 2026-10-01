using Catalog.Application.Brands.Commands;
using Catalog.Core.Constants;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.Brands.Validators
{
    public class CreateBrandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandValidator()
        {
            RuleFor(x => x.Payload.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .MaximumLength(FieldLength.BrandName).WithMessage(ValidationMessages.MaximumLength("Name", FieldLength.BrandName));
        }
    }
}
