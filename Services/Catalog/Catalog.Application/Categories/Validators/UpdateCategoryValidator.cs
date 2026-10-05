using Catalog.Application.Categories.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Settings;
using Catalog.Core.Constants;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;
using Microsoft.Extensions.Options;

namespace Catalog.Application.Categories.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator(IOptions<FileUploadSettings> fileUploadOptions)
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));

            RuleFor(x => x.Payload.Name)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Name"))
                .MaximumLength(FieldLength.CategoryName).WithMessage(ValidationMessages.MaximumLength("Name", FieldLength.CategoryName));


            RuleFor(x => x.Payload.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.MustBeGreaterThanOrEqual("DisplayOrder", 0));

            RuleFor(x => x.Payload.ParentId)
                .Must(GuidValidator.IsValidGuid).WithMessage(ValidationMessages.MustBeAValidGuid("ParentId"))
                .When(x => !string.IsNullOrEmpty(x.Payload.ParentId?.ToString()));

            RuleFor(x => x.Payload.ThumbnailImage)
                .MustBeValidImage("ThumbnailImage", fileUploadOptions.Value.MaxImageSizeMB);
        }
    }
}
