using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Product;
using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Headers;

namespace LiteCommerce.Admin.Pages.Catalog.Products
{
    public partial class ProductImportDialog
    {
        private const string DefaultReportFileName = "product-import-review.xlsx";

        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        private IProductApi ProductApi { get; set; }

        [Inject]
        private FileDownloadService FileDownloadService { get; set; }

        [Inject]
        private FileUploadSettings FileUploadSettings { get; set; }

        private MudFileUpload<IBrowserFile> _fileUpload = null!;

        private int _step = 1;
        private bool _busy;
        private bool _onlyInvalid;
        private string _mode = ProductImportModes.AllOrNothing;
        // Off by default: a typo in a brand name should be an error, not a new brand.
        private bool _createMissingBrands;
        private string? _error;
        private string? _fileName;
        private long _fileSize;
        private byte[]? _fileBytes;
        private ProductImportPreviewResponse? _preview;
        private ProductImportResultResponse? _result;

        // Totals across every import done in this dialog ("Import another file" starts a new one),
        // so closing it any way still tells the page to reload.
        private int _importedTotal;
        private int _skippedTotal;

        private IEnumerable<ProductImportRowResult> VisibleRows =>
            _preview is null
                ? Enumerable.Empty<ProductImportRowResult>()
                : _onlyInvalid
                    ? _preview.Rows.Where(r => !r.IsValid)
                    : _preview.Rows;

        private Color StepColor(int step) => step <= _step ? Color.Primary : Color.Default;

        private Variant StepVariant(int step) => step == _step ? Variant.Filled : Variant.Outlined;

        private string StepTextClass(int step) => step == _step ? "font-weight-bold" : "mud-text-secondary";

        private async Task OnFileSelected(IBrowserFile? file)
        {
            // MudFileUpload raises FilesChanged with null when the selection is cleared.
            if (file is null)
            {
                ClearSelection();
                return;
            }

            _error = null;
            ClearSelection();

            var extension = Path.GetExtension(file.Name);
            if (!string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(extension, ".xls", StringComparison.OrdinalIgnoreCase))
            {
                _error = "Only .xlsx or .xls files are supported.";
                await ClearFileAsync();
                return;
            }

            if (file.Size > FileUploadSettings.MaxDocumentSize)
            {
                _error = FileUploadSettings.DocumentSizeExceededMessage;
                await ClearFileAsync();
                return;
            }

            try
            {
                await using var stream = file.OpenReadStream(FileUploadSettings.MaxDocumentSize);
                using var buffer = new MemoryStream();
                await stream.CopyToAsync(buffer);

                _fileBytes = buffer.ToArray();
                _fileName = file.Name;
                _fileSize = file.Size;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                await ClearFileAsync();
            }
        }

        private void ClearSelection()
        {
            _fileBytes = null;
            _fileName = null;
            _fileSize = 0;
            _preview = null;
            _result = null;
        }

        private async Task ClearFileAsync()
        {
            ClearSelection();

            if (_fileUpload is not null)
            {
                await _fileUpload.ClearAsync();
            }
        }

        private async Task PreviewAsync()
        {
            if (_fileBytes is null)
            {
                return;
            }

            _busy = true;
            _error = null;

            try
            {
                using var content = BuildContent();
                var result = await ProductApi.PreviewProductImportAsync(content);

                if (result.Data is not null)
                {
                    _preview = result.Data;
                    _onlyInvalid = false;
                    _step = 2;
                }
                else
                {
                    _error = result.GetErrorMessage(SystemMessages.ErrorOccurred);
                }
            }
            catch (Exception ex)
            {
                _error = $"Error: {ex.Message}";
            }
            finally
            {
                _busy = false;
                StateHasChanged();
            }
        }

        private async Task CommitAsync()
        {
            if (_fileBytes is null || _preview is null)
            {
                return;
            }

            _busy = true;
            _error = null;

            try
            {
                using var content = BuildContent();
                var result = await ProductApi.CommitProductImportAsync(content);

                if (result.IsSuccess && result.Data is not null)
                {
                    _result = result.Data;
                    _importedTotal += result.Data.Imported;
                    _skippedTotal += result.Data.Skipped;
                    _step = 3;
                }
                else
                {
                    _error = result.GetErrorMessage(SystemMessages.ErrorOccurred);
                }
            }
            catch (Exception ex)
            {
                _error = $"Error: {ex.Message}";
            }
            finally
            {
                _busy = false;
                StateHasChanged();
            }
        }

        private async Task DownloadReportAsync()
        {
            if (_fileBytes is null)
            {
                return;
            }

            _busy = true;
            _error = null;

            try
            {
                using var content = BuildContent();
                using var response = await ProductApi.GetProductImportErrorReportAsync(content);

                if (!response.IsSuccessStatusCode)
                {
                    _error = "Could not generate the error report. Check that the file can be parsed.";
                    return;
                }

                await FileDownloadService.DownloadAsync(response, DefaultReportFileName);
            }
            catch (Exception ex)
            {
                _error = $"Error: {ex.Message}";
            }
            finally
            {
                _busy = false;
                StateHasChanged();
            }
        }

        private async Task SwitchToSkipInvalidAsync()
        {
            _mode = ProductImportModes.SkipInvalid;
            await PreviewAsync();
        }

        private void BackToUpload()
        {
            _step = 1;
            _error = null;
            _preview = null;
            _onlyInvalid = false;
        }

        private async Task ResetAsync()
        {
            _step = 1;
            _error = null;
            _preview = null;
            _result = null;
            _onlyInvalid = false;

            await ClearFileAsync();
        }

        // Every way out (X, Cancel, View products) goes through here; Escape is disabled by the caller.
        private void CloseDialog()
        {
            if (_importedTotal > 0)
            {
                CloseAfterImport();
                return;
            }

            MudDialog.Cancel();
        }

        private void CloseAfterImport()
            => MudDialog.Close(DialogResult.Ok(new ProductImportDialogResult
            {
                Imported = _importedTotal,
                Skipped = _skippedTotal
            }));

        private MultipartFormDataContent BuildContent()
        {
            var content = new MultipartFormDataContent();

            var fileContent = new ByteArrayContent(_fileBytes!);
            var isLegacyXls = string.Equals(Path.GetExtension(_fileName), ".xls", StringComparison.OrdinalIgnoreCase);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(isLegacyXls
                ? FileUploadConstants.LegacySpreadsheetContentType
                : FileUploadConstants.SpreadsheetContentType);
            content.Add(fileContent, "file", _fileName ?? "products.xlsx");

            content.Add(new StringContent(_mode), "mode");
            content.Add(new StringContent(_createMissingBrands ? "true" : "false"), "createMissingBrands");

            return content;
        }
    }
}
