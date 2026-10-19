using FluentValidation.Results;

namespace Identity.Infrastructure.Services
{
    // Identity has no ValidationBehavior, so services validate by hand and return the errors in a BaseResponse.
    internal static class ValidationErrors
    {
        // Same error shape as Catalog: keyed by camelCase field name. Null when the result is valid.
        public static Dictionary<string, List<string>>? From(ValidationResult result)
        {
            if (result.IsValid)
                return null;

            return result.Errors
                .GroupBy(x => LowerFirst(x.PropertyName))
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(e => e.ErrorMessage).ToList());
        }

        private static string LowerFirst(string value)
            => string.IsNullOrEmpty(value) ? value : char.ToLowerInvariant(value[0]) + value[1..];
    }
}
