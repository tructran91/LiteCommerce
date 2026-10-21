using Catalog.Application.Products.Import;
using Catalog.Application.Products.Queries;
using Catalog.Application.Settings;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Catalog.Application.Products.Validators
{
    public class BuildProductImportReportValidator : AbstractValidator<BuildProductImportReportQuery>
    {
        public BuildProductImportReportValidator(IOptions<FileUploadSettings> fileUploadOptions)
        {
            this.AddProductImportRules(
                x => x.Payload.File,
                x => x.Payload.Mode,
                fileUploadOptions.Value.MaxDocumentSizeMB);
        }
    }
}
