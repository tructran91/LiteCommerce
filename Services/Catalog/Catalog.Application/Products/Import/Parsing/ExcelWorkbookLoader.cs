using ClosedXML.Excel;
using ExcelDataReader;
using System.Text;

namespace Catalog.Application.Products.Import.Parsing
{
    // Opens an uploaded workbook as a ClosedXML workbook, whatever its format.
    // ClosedXML reads only .xlsx, so a legacy .xls (BIFF) file is read with ExcelDataReader and its
    // first worksheet is copied, cell by cell, into an in-memory .xlsx workbook. The parser and the
    // error report then work the same way for both formats.
    public static class ExcelWorkbookLoader
    {
        // OLE2 compound file header: every .xls workbook starts with these bytes (.xlsx is a zip, "PK").
        private static readonly byte[] Ole2Signature = [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1];

        static ExcelWorkbookLoader()
        {
            // .xls stores text in legacy code pages (e.g. windows-1252), which .NET does not load by default.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        // The stream must be seekable. Throws when the content is not a readable workbook.
        public static XLWorkbook Load(Stream stream)
        {
            if (!IsLegacyXls(stream))
            {
                return new XLWorkbook(stream);
            }

            return ConvertXls(stream);
        }

        private static bool IsLegacyXls(Stream stream)
        {
            var header = new byte[Ole2Signature.Length];
            var read = stream.Read(header, 0, header.Length);
            stream.Position = 0;

            return read == header.Length && header.AsSpan().SequenceEqual(Ole2Signature);
        }

        private static XLWorkbook ConvertXls(Stream stream)
        {
            using var reader = ExcelReaderFactory.CreateBinaryReader(stream);

            var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet(string.IsNullOrWhiteSpace(reader.Name) ? "Sheet1" : reader.Name);

            // Only the first sheet is imported, so only the first sheet is copied.
            var rowNumber = 0;
            while (reader.Read())
            {
                rowNumber++;
                for (var column = 0; column < reader.FieldCount; column++)
                {
                    var value = reader.GetValue(column);
                    if (value is not null)
                    {
                        worksheet.Cell(rowNumber, column + 1).Value = ToCellValue(value);
                    }
                }
            }

            return workbook;
        }

        // Keep the cell type (number, date, boolean, text): the parser treats a numeric cell
        // differently from the same digits typed as text.
        private static XLCellValue ToCellValue(object value) => value switch
        {
            double number => number,
            int number => number,
            DateTime date => date,
            TimeSpan time => time,
            bool boolean => boolean,
            _ => Convert.ToString(value) ?? string.Empty
        };
    }
}
