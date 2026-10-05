using Catalog.Application.Products.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Settings;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;
using Microsoft.Extensions.Options;

namespace Catalog.Application.Products.Validators
{
    public class UploadContentImageValidator : AbstractValidator<UploadContentImageCommand>
    {
        public UploadContentImageValidator(IOptions<FileUploadSettings> fileUploadOptions)
        {
            RuleFor(x => x.Payload.File)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("File"))
                .MustBeValidImage("File", fileUploadOptions.Value.MaxImageSizeMB);

            RuleFor(x => x.Payload.ProductId)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("ProductId"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("ProductId"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("ProductId"));
        }
    }
}
