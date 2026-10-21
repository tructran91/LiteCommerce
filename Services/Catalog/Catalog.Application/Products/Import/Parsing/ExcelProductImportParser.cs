using Catalog.Application.Extensions;
using Catalog.Application.Products.Import.Models;
using Catalog.Core.Constants;
using ClosedXML.Excel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Catalog.Application.Products.Import.Parsing
{
    // Reads the first worksheet of an .xlsx or .xls file into ParsedProductRow objects.
    // Cell-level problems become row errors; structural problems become file errors.
    public class ExcelProductImportParser
    {
        private static readonly string[] BooleanTrue = ["true", "1", "yes", "y"];
        private static readonly string[] BooleanFalse = ["false", "0", "no", "n"];
        private static readonly string[] DateFormats = ["yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss"];
        private static readonly Regex DotGroupedThousands = new(@"^-?\d{1,3}(\.\d{3})+$", RegexOptions.Compiled);

        public ParsedImportFile Parse(Stream stream)
        {
            var result = new ParsedImportFile();

            // Keep the buffer alive while the workbook is in use.
            using var buffer = new MemoryStream();
            stream.CopyTo(buffer);
            buffer.Position = 0;

            XLWorkbook workbook;
            try
            {
                workbook = ExcelWorkbookLoader.Load(buffer);
            }
            catch (Exception)
            {
                result.Errors.Add(new ProductImportError("File", "The file is not a valid Excel workbook (.xlsx or .xls)."));
                return result;
            }

            using (workbook)
            {
                var worksheet = workbook.Worksheets.FirstOrDefault();
                if (worksheet is null)
                {
                    result.Errors.Add(new ProductImportError("File", "The workbook does not contain any worksheet."));
                    return result;
                }

                var headerRow = worksheet.FirstRowUsed();
                if (headerRow is null)
                {
                    result.Errors.Add(new ProductImportError("File", "The worksheet is empty."));
                    return result;
                }

                var columns = ReadHeaders(headerRow, result);
                if (columns is null)
                {
                    return result;
                }

                var lastRowNumber = worksheet.LastRowUsed()?.RowNumber() ?? headerRow.RowNumber();
                for (var rowNumber = headerRow.RowNumber() + 1; rowNumber <= lastRowNumber; rowNumber++)
                {
                    var row = worksheet.Row(rowNumber);
                    if (IsBlank(row, columns))
                    {
                        continue;
                    }

                    result.Rows.Add(ReadRow(row, columns));
                }

                if (result.Rows.Count == 0)
                {
                    result.Errors.Add(new ProductImportError("File", "The worksheet does not contain any data rows."));
                }
            }

            return result;
        }

        private static Dictionary<string, int>? ReadHeaders(IXLRow headerRow, ParsedImportFile result)
        {
            var columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var cell in headerRow.CellsUsed())
            {
                var header = cell.GetString().Trim();
                // "_"-prefixed columns (_Status, _Errors) come from the error report, so a fixed report can be re-uploaded.
                if (string.IsNullOrEmpty(header) || header.StartsWith(ProductImportTemplate.ReservedColumnPrefix, StringComparison.Ordinal))
                {
                    continue;
                }

                if (!ProductImportTemplate.AllColumns.Contains(header))
                {
                    result.Errors.Add(new ProductImportError(header, $"Unknown column '{header}'."));
                    continue;
                }

                if (!columns.TryAdd(header, cell.Address.ColumnNumber))
                {
                    result.Errors.Add(new ProductImportError(header, $"Duplicate column '{header}'."));
                }
            }

            foreach (var required in ProductImportTemplate.RequiredColumns)
            {
                if (!columns.ContainsKey(required))
                {
                    result.Errors.Add(new ProductImportError(required, $"Required column '{required}' is missing."));
                }
            }

            return result.Errors.Count == 0 ? columns : null;
        }

        private static ParsedProductRow ReadRow(IXLRow row, Dictionary<string, int> columns)
        {
            var parsed = new ParsedProductRow { RowNumber = row.RowNumber() };

            parsed.Name = ReadString(row, columns, ProductImportTemplate.Name)?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(parsed.Name))
            {
                parsed.Errors.Add(new ProductImportError(ProductImportTemplate.Name, "Name is required."));
            }
            else if (parsed.Name.Length > FieldLength.ProductName)
            {
                parsed.Errors.Add(new ProductImportError(ProductImportTemplate.Name,
                    $"Name must not exceed {FieldLength.ProductName} characters."));
            }

            // Brand is optional: an empty cell means the product has no brand.
            parsed.Brand = ReadString(row, columns, ProductImportTemplate.Brand)?.Trim() ?? string.Empty;
            if (parsed.Brand.Length > FieldLength.BrandName)
            {
                parsed.Errors.Add(new ProductImportError(ProductImportTemplate.Brand,
                    $"Brand must not exceed {FieldLength.BrandName} characters."));
            }

            parsed.Category = ReadString(row, columns, ProductImportTemplate.Category);

            parsed.Price = ReadDecimal(row, columns, ProductImportTemplate.Price, parsed, required: true);

            parsed.Slug = ReadString(row, columns, ProductImportTemplate.Slug)?.Trim();
            if (!string.IsNullOrEmpty(parsed.Slug) && parsed.Slug.Length > FieldLength.ProductSlug)
            {
                parsed.Errors.Add(new ProductImportError(ProductImportTemplate.Slug,
                    $"Slug must not exceed {FieldLength.ProductSlug} characters."));
            }

            parsed.Sku = ReadString(row, columns, ProductImportTemplate.Sku)?.Trim();
            parsed.Gtin = ReadString(row, columns, ProductImportTemplate.Gtin)?.Trim();
            parsed.ShortDescription = ReadString(row, columns, ProductImportTemplate.ShortDescription);
            parsed.Description = ReadString(row, columns, ProductImportTemplate.Description);

            parsed.IsPublished = ReadBoolean(row, columns, ProductImportTemplate.IsPublished, parsed, defaultValue: true) ?? true;
            parsed.IsFeatured = ReadBoolean(row, columns, ProductImportTemplate.IsFeatured, parsed, defaultValue: false) ?? false;
            parsed.IsCallForPricing = ReadBoolean(row, columns, ProductImportTemplate.IsCallForPricing, parsed, defaultValue: false) ?? false;
            parsed.IsAllowToOrder = ReadBoolean(row, columns, ProductImportTemplate.IsAllowToOrder, parsed, defaultValue: false) ?? false;

            parsed.OldPrice = ReadDecimal(row, columns, ProductImportTemplate.OldPrice, parsed, required: false);
            parsed.SpecialPrice = ReadDecimal(row, columns, ProductImportTemplate.SpecialPrice, parsed, required: false);
            parsed.SpecialPriceStart = ReadDate(row, columns, ProductImportTemplate.SpecialPriceStart, parsed);
            parsed.SpecialPriceEnd = ReadDate(row, columns, ProductImportTemplate.SpecialPriceEnd, parsed);

            parsed.StockQuantity = ReadInt(row, columns, ProductImportTemplate.StockQuantity, parsed, allowNegative: false) ?? 0;
            parsed.DisplayOrder = ReadInt(row, columns, ProductImportTemplate.DisplayOrder, parsed, allowNegative: true) ?? 0;

            parsed.MetaTitle = ReadString(row, columns, ProductImportTemplate.MetaTitle);
            parsed.MetaKeywords = ReadString(row, columns, ProductImportTemplate.MetaKeywords);
            parsed.MetaDescription = ReadString(row, columns, ProductImportTemplate.MetaDescription);
            parsed.CanonicalUrl = ReadString(row, columns, ProductImportTemplate.CanonicalUrl);
            parsed.OgTitle = ReadString(row, columns, ProductImportTemplate.OgTitle);
            parsed.OgDescription = ReadString(row, columns, ProductImportTemplate.OgDescription);
            parsed.OgImage = ReadString(row, columns, ProductImportTemplate.OgImage);
            parsed.OgUrl = ReadString(row, columns, ProductImportTemplate.OgUrl);
            parsed.SchemaJson = ReadString(row, columns, ProductImportTemplate.SchemaJson);

            if (parsed.SpecialPriceStart.HasValue && parsed.SpecialPriceEnd.HasValue &&
                parsed.SpecialPriceStart > parsed.SpecialPriceEnd)
            {
                parsed.Errors.Add(new ProductImportError(ProductImportTemplate.SpecialPriceStart,
                    "SpecialPriceStart must not be after SpecialPriceEnd."));
            }

            return parsed;
        }

        // Only template columns count: a row holding just a leftover _Status/_Errors value is blank.
        private static bool IsBlank(IXLRow row, Dictionary<string, int> columns)
            => columns.Values.All(index => string.IsNullOrWhiteSpace(row.Cell(index).GetString()));

        private static IXLCell? GetCell(IXLRow row, Dictionary<string, int> columns, string column)
            => columns.TryGetValue(column, out var index) ? row.Cell(index) : null;

        private static string? ReadString(IXLRow row, Dictionary<string, int> columns, string column)
        {
            var cell = GetCell(row, columns, column);
            if (cell is null || cell.IsEmpty())
            {
                return null;
            }

            var value = cell.GetString().Trim();
            return string.IsNullOrEmpty(value) ? null : value;
        }

        private static decimal? ReadDecimal(IXLRow row, Dictionary<string, int> columns, string column, ParsedProductRow parsed, bool required)
        {
            var cell = GetCell(row, columns, column);
            if (cell is null || cell.IsEmpty())
            {
                if (required)
                {
                    parsed.Errors.Add(new ProductImportError(column, $"{column} is required."));
                }

                return null;
            }

            // Only a real numeric cell is read as a number; text cells go through the strict parse below.
            if (cell.DataType == XLDataType.Number)
            {
                return ValidateDecimal((decimal)cell.Value.GetNumber(), column, parsed);
            }

            var text = cell.GetString().Trim();

            // "1.000" typed as text is a thousands-grouped 1000 in vi-VN, not 1.0. Reject it instead of guessing.
            if (DotGroupedThousands.IsMatch(text))
            {
                parsed.Errors.Add(new ProductImportError(column,
                    $"{column} '{text}' looks like it uses '.' as a thousands separator. Enter the number without separators."));
                return null;
            }

            // No thousands separators: AllowThousands is intentionally excluded.
            const NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

            if (decimal.TryParse(text, styles, CultureInfo.InvariantCulture, out var value))
            {
                return ValidateDecimal(value, column, parsed);
            }

            parsed.Errors.Add(new ProductImportError(column, $"{column} must be a number without thousands separators."));
            return null;
        }

        private static decimal? ValidateDecimal(decimal value, string column, ParsedProductRow parsed)
        {
            if (value < 0)
            {
                parsed.Errors.Add(new ProductImportError(column, $"{column} must not be negative."));
                return null;
            }

            return value;
        }

        private static int? ReadInt(IXLRow row, Dictionary<string, int> columns, string column, ParsedProductRow parsed, bool allowNegative)
        {
            var cell = GetCell(row, columns, column);
            if (cell is null || cell.IsEmpty())
            {
                return null;
            }

            int? value = null;
            if (cell.DataType == XLDataType.Number)
            {
                // A fractional number (1.5) is rejected, not rounded.
                var number = cell.Value.GetNumber();
                if (number == Math.Floor(number) && number >= int.MinValue && number <= int.MaxValue)
                {
                    value = (int)number;
                }
            }
            else if (int.TryParse(cell.GetString().Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
            {
                value = parsedValue;
            }

            if (value is null)
            {
                parsed.Errors.Add(new ProductImportError(column, $"{column} must be an integer."));
                return null;
            }

            if (!allowNegative && value < 0)
            {
                parsed.Errors.Add(new ProductImportError(column, $"{column} must not be negative."));
                return null;
            }

            return value;
        }

        private static DateTime? ReadDate(IXLRow row, Dictionary<string, int> columns, string column, ParsedProductRow parsed)
        {
            var cell = GetCell(row, columns, column);
            if (cell is null || cell.IsEmpty())
            {
                return null;
            }

            // A real Excel date cell is unambiguous. Text must match DateFormats exactly: a lenient parse
            // would read "03/04/2026" as March 4 while a Vietnamese user means 3 April.
            if (cell.DataType == XLDataType.DateTime)
            {
                return cell.Value.GetDateTime();
            }

            var text = cell.GetString().Trim();
            if (DateTime.TryParseExact(text, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var exact))
            {
                return exact;
            }

            parsed.Errors.Add(new ProductImportError(column, $"{column} must be a date in yyyy-MM-dd format."));
            return null;
        }

        private static bool? ReadBoolean(IXLRow row, Dictionary<string, int> columns, string column, ParsedProductRow parsed, bool defaultValue)
        {
            var cell = GetCell(row, columns, column);
            if (cell is null || cell.IsEmpty())
            {
                return defaultValue;
            }

            if (cell.TryGetValue<bool>(out var boolean))
            {
                return boolean;
            }

            var text = cell.GetString().Trim();
            if (BooleanTrue.Contains(text, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if (BooleanFalse.Contains(text, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            parsed.Errors.Add(new ProductImportError(column, $"{column} must be true or false."));
            return null;
        }
    }
}
