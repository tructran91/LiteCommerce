using Catalog.Application.Extensions;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace Catalog.Application.Products.Import
{
    public static class ProductImportValidationExtensions
    {
        // File and mode are validated identically by preview, commit and error report.
        public static void AddProductImportRules<T>(
            this AbstractValidator<T> validator,
            Expression<Func<T, IFormFile?>> file,
            Expression<Func<T, string?>> mode,
            int maxFileSizeMB)
        {
            validator.RuleFor(file)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("File"))
                .MustBeValidExcel("File", maxFileSizeMB);

            validator.RuleFor(mode)
                .Must(BeAValidMode)
                .WithMessage($"Mode must be '{ProductImportModes.AllOrNothing}' or '{ProductImportModes.SkipInvalid}'.");
        }

        private static bool BeAValidMode(string? mode)
            => ProductImportModes.TryParse(mode, out _);
    }
}
