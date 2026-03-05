namespace LiteCommerce.Admin.Helpers
{
    public static class NumberFormatHelper
    {
        /// <summary>
        /// Format decimal - remove trailing .00 if it's a whole number
        /// </summary>
        /// <param name="value">Decimal value to format</param>
        /// <returns>Formatted string: "100" for whole numbers, "99.5" or "99.99" for decimals</returns>
        public static string FormatDecimal(decimal? value)
        {
            if (!value.HasValue) 
                return string.Empty;

            // If it's a whole number, don't display .00
            if (value.Value == Math.Floor(value.Value))
                return value.Value.ToString("0");

            // If it has decimal part, display maximum 2 digits
            return value.Value.ToString("0.##");
        }

        /// <summary>
        /// Parse string to nullable decimal
        /// </summary>
        /// <param name="value">String value to parse</param>
        /// <returns>Parsed decimal or null if invalid</returns>
        public static decimal? ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (decimal.TryParse(value, out var result))
                return result;

            return null;
        }

        /// <summary>
        /// Format decimal with currency symbol (e.g.: $100, $99.99)
        /// </summary>
        public static string FormatCurrency(decimal? value, string currencySymbol = "$")
        {
            if (!value.HasValue) 
                return string.Empty;

            var formatted = FormatDecimal(value);
            return $"{currencySymbol}{formatted}";
        }

        /// <summary>
        /// Format with thousands separator (e.g.: 1,000 or 1,000.50)
        /// </summary>
        public static string FormatDecimalWithSeparator(decimal? value)
        {
            if (!value.HasValue) 
                return string.Empty;

            if (value.Value == Math.Floor(value.Value))
                return value.Value.ToString("N0"); // 1,000

            return value.Value.ToString("N2").TrimEnd('0').TrimEnd('.'); // 1,000.5
        }
    }
}
