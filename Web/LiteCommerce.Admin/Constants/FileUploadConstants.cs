namespace LiteCommerce.Admin.Constants
{
    public static class FileUploadConstants
    {
        // File Size Limits
        public const long MaxImageSize = 5 * 1024 * 1024; // 5MB
        public const long MaxDocumentSize = 10 * 1024 * 1024; // 10MB

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
            "application/vnd.ms-excel",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };

        // Error Messages
        public const string ImageSizeExceededMessage = "File size must not exceed 5MB";
        public const string InvalidImageTypeMessage = "Only image files are allowed";
        public const string DocumentSizeExceededMessage = "File size must not exceed 10MB";
        public const string InvalidDocumentTypeMessage = "Only PDF, Word, and Excel files are allowed";
    }
}