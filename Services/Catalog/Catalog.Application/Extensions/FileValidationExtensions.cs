using FluentValidation;
using LiteCommerce.Shared.Constants;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Extensions
{
    public static class FileValidationExtensions
    {
        private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        private static readonly string[] DocumentExtensions = [".pdf", ".doc", ".docx", ".xls", ".xlsx"];

        public static IRuleBuilderOptions<T, IFormFile?> MustBeValidImage<T>(this IRuleBuilder<T, IFormFile?> ruleBuilder, string fieldName, int maxSizeMB)
        {
            return ruleBuilder.MustBeValidFile(fieldName, maxSizeMB, ImageExtensions);
        }

        public static IRuleBuilderOptions<T, IFormFile?> MustBeValidDocument<T>(this IRuleBuilder<T, IFormFile?> ruleBuilder, string fieldName, int maxSizeMB)
        {
            return ruleBuilder.MustBeValidFile(fieldName, maxSizeMB, DocumentExtensions);
        }

        private static IRuleBuilderOptions<T, IFormFile?> MustBeValidFile<T>(
            this IRuleBuilder<T, IFormFile?> ruleBuilder,
            string fieldName,
            int maxSizeMB,
            string[] allowedExtensions)
        {
            var maxSize = maxSizeMB * 1024L * 1024L;

            return ruleBuilder
                .Must(file => file is null || file.Length > 0)
                    .WithMessage(ValidationMessages.NotNullOrEmpty(fieldName))
                .Must(file => file is null || file.Length <= maxSize)
                    .WithMessage(ValidationMessages.FileSizeExceeded(fieldName, maxSizeMB))
                .Must(file => file is null || allowedExtensions.Contains(Path.GetExtension(file.FileName), StringComparer.OrdinalIgnoreCase))
                    .WithMessage(ValidationMessages.InvalidFileType(fieldName, string.Join(", ", allowedExtensions)));
        }
    }
}
