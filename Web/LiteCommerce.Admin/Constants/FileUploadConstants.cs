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

        // Error Messages
        public const string ImageSizeExceededMessage = "File size must not exceed 5MB";
        public const string InvalidImageTypeMessage = "Only image files are allowed";
    }
}