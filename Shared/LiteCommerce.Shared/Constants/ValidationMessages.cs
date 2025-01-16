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
    }
}
