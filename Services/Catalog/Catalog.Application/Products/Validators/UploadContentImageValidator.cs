using Catalog.Application.Products.Commands;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.Products.Validators
{
    public class UploadContentImageValidator : AbstractValidator<UploadContentImageCommand>
    {
        public UploadContentImageValidator()
        {
            RuleFor(x => x.Payload.File)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("File"))
                .Must(file => file == null || file.Length > 0).WithMessage(ValidationMessages.NotNullOrEmpty("File"));

            RuleFor(x => x.Payload.ProductId)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("ProductId"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("ProductId"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("ProductId"));
        }
    }
}
