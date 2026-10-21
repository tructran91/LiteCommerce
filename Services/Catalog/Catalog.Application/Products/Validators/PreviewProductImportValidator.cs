using Catalog.Application.Products.Commands;
using Catalog.Application.Products.Import;
using Catalog.Application.Settings;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Catalog.Application.Products.Validators
{
    public class PreviewProductImportValidator : AbstractValidator<PreviewProductImportCommand>
    {
        public PreviewProductImportValidator(IOptions<FileUploadSettings> fileUploadOptions)
        {
            this.AddProductImportRules(
                x => x.Payload.File,
                x => x.Payload.Mode,
                fileUploadOptions.Value.MaxDocumentSizeMB);
        }
    }
}
