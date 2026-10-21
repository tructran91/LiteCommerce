namespace LiteCommerce.Admin.Constants
{
    public static class FileUploadConstants
    {
        public const string SpreadsheetContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string LegacySpreadsheetContentType = "application/vnd.ms-excel";

        // Allowed File Types
        public static readonly string[] AllowedImageTypes = new[]
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };

        public static readonly string[] AllowedDocumentTypes = new[]
        {
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            LegacySpreadsheetContentType,
            SpreadsheetContentType
        };

        // Error Messages
        public const string InvalidImageTypeMessage = "Only image files are allowed";
        public const string InvalidDocumentTypeMessage = "Only PDF, Word, and Excel files are allowed";
    }
}