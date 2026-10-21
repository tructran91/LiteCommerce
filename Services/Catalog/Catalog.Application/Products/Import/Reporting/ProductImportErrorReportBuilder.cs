using Catalog.Application.Products.Import;
using Catalog.Application.Products.Import.Models;
using Catalog.Application.Products.Import.Parsing;
using ClosedXML.Excel;

namespace Catalog.Application.Products.Import.Reporting
{
    // Produces a copy of the uploaded workbook with _Status/_Errors columns so the user can fix rows.
    public class ProductImportErrorReportBuilder
    {
        public const string StatusColumn = "_Status";
        public const string ErrorsColumn = "_Errors";

        public byte[] Build(Stream source, ParsedImportFile file, IReadOnlyDictionary<int, ResolvedProductRow> rowsByNumber)
        {
            // An .xls upload comes back as .xlsx: the report is always written in the modern format.
            using var workbook = ExcelWorkbookLoader.Load(source);
            var worksheet = workbook.Worksheets.First();

            var headerRow = worksheet.FirstRowUsed()!;
            var lastColumn = headerRow.LastCellUsed()!.Address.ColumnNumber;

            // A re-uploaded report already has these columns: reuse them instead of appending a second pair.
            var statusColumn = FindCell(headerRow, StatusColumn)?.Address.ColumnNumber ?? ++lastColumn;
            var errorsColumn = FindCell(headerRow, ErrorsColumn)?.Address.ColumnNumber ?? ++lastColumn;

            headerRow.Cell(statusColumn).Value = StatusColumn;
            headerRow.Cell(errorsColumn).Value = ErrorsColumn;
            headerRow.Cell(statusColumn).Style.Font.Bold = true;
            headerRow.Cell(errorsColumn).Style.Font.Bold = true;

            var redFill = XLColor.FromHtml("#FFC7CE");

            foreach (var parsedRow in file.Rows)
            {
                var row = worksheet.Row(parsedRow.RowNumber);
                var resolved = rowsByNumber.GetValueOrDefault(parsedRow.RowNumber);
                var isValid = resolved?.IsValid == true;

                row.Cell(statusColumn).Value = isValid ? ProductImportRowStatus.Valid : ProductImportRowStatus.Invalid;
                row.Cell(errorsColumn).Value = isValid
                    ? string.Empty
                    : string.Join("; ", resolved?.Errors.Select(e => $"{e.Column}: {e.Message}") ?? []);

                if (isValid)
                {
                    // Drop the red fill a previous report may have left on this row.
                    row.Cell(statusColumn).Style.Fill.BackgroundColor = XLColor.NoColor;
                    row.Cell(errorsColumn).Style.Fill.BackgroundColor = XLColor.NoColor;
                    continue;
                }

                row.Cell(statusColumn).Style.Fill.BackgroundColor = redFill;
                row.Cell(errorsColumn).Style.Fill.BackgroundColor = redFill;

                // Highlight the offending cells so they are easy to spot.
                foreach (var error in resolved?.Errors ?? [])
                {
                    var cell = FindCell(headerRow, error.Column);
                    if (cell is not null)
                    {
                        row.Cell(cell.Address.ColumnNumber).Style.Fill.BackgroundColor = redFill;
                    }
                }
            }

            worksheet.Column(statusColumn).AdjustToContents();
            worksheet.Column(errorsColumn).AdjustToContents();

            using var output = new MemoryStream();
            workbook.SaveAs(output);
            return output.ToArray();
        }

        private static IXLCell? FindCell(IXLRow headerRow, string column)
        {
            return headerRow.CellsUsed()
                .FirstOrDefault(c => string.Equals(c.GetString().Trim(), column, StringComparison.OrdinalIgnoreCase));
        }
    }
}
