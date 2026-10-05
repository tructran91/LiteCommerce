using Catalog.Application.Products.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Settings;
using Catalog.Core.Constants;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;
using Microsoft.Extensions.Options;

namespace Catalog.Application.Products.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator(IOptions<FileUploadSettings> fileUploadOptions)
        {
            var fileUpload = fileUploadOptions.Value;

            RuleFor(x => x.Payload.Product.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .MaximumLength(FieldLength.ProductName).WithMessage(ValidationMessages.MaximumLength("Name", FieldLength.ProductName));

            RuleFor(x => x.Payload.Product.BrandId)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Brand"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Brand"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));

            RuleFor(x => x.Payload.ThumbnailImage)
                .MustBeValidImage("ThumbnailImage", fileUpload.MaxImageSizeMB);

            RuleForEach(x => x.Payload.ProductImages)
                .MustBeValidImage("ProductImages", fileUpload.MaxImageSizeMB);

            RuleForEach(x => x.Payload.ProductDocuments)
                .MustBeValidDocument("ProductDocuments", fileUpload.MaxDocumentSizeMB);
        }
    }
}
