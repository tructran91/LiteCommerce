namespace LiteCommerce.Shared.Constants
{
    public static class ValidationMessages
    {
        public static string NotNullOrEmpty(string fieldName)
        {
            return $"{fieldName} cannot be null or empty.";
        }

        public static string MustBeAValidGuid(string fieldName)
        {
            return $"{fieldName} must be a valid GUID.";
        }

        public static string MustBePositiveNumber(string fieldName)
        {
            return $"{fieldName} must be a positive number.";
        }

        public static string MustBeGreaterThan(string fieldName, int min)
        {
            return $"{fieldName} must be greater than {min}.";
        }

        public static string MustBeGreaterThanOrEqual(string fieldName, int min)
        {
            return $"{fieldName} must be greater than or equal to {min}.";
        }

        public static string MustBeLessThanOrEqual(string fieldName, int max)
        {
            return $"{fieldName} must be less than or equal to {max}.";
        }

        public static string MaximumLength(string fieldName, int max)
        {
            return $"{fieldName} must not exceed {max} characters.";
        }

        public static string MustNotBeAfter(string fieldName, string otherFieldName)
        {
            return $"{fieldName} must not be after {otherFieldName}.";
        }

        public static string FileSizeExceeded(string fieldName, int maxSizeMB)
        {
            return $"{fieldName} must not exceed {maxSizeMB}MB.";
        }

        public static string InvalidFileType(string fieldName, string allowedExtensions)
        {
            return $"{fieldName} must be one of the following file types: {allowedExtensions}.";
        }
    }
}
