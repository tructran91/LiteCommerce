using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.AuditLog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Logs.AuditLogs
{
    public partial class AuditLogManagement
    {
        [Inject]
        private IAuditLogApi AuditLogApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private List<BreadcrumbItem> _breadcrumbs = new()
        {
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("System", href: null, disabled: true),
            new("Audit Logs", href: null, disabled: true),
        };

        // Logs only grow, so paging happens on the server (unlike the Catalog pages, which page in memory).
        private readonly int[] _pageSizeOptions = { 10, AppConstants.PageSize, 50, 100 };

        // Bound two-way: a constant RowsPerPage would be re-applied on every render and undo the user's choice.
        private int _rowsPerPage = AppConstants.PageSize;

        private MudTable<AuditLogResponse> _table = null!;
        private string? _errorMessage;
        private AuditLogQuery _query = new();

        private readonly DialogOptions _dialogOptions = new()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true,
        };

        private async Task<TableData<AuditLogResponse>> LoadServerData(TableState state, CancellationToken cancellationToken)
        {
            _errorMessage = null;

            try
            {
                var result = await AuditLogApi.GetAuditLogsAsync(
                    state.Page + 1,
                    state.PageSize,
                    entityName: _query.EntityName,
                    action: _query.Action,
                    correlationId: _query.CorrelationId,
                    fromDate: LogConstants.ToUtcStartOfDay(_query.FromDate),
                    toDate: LogConstants.ToUtcEndOfDay(_query.ToDate));

                if (result.IsSuccess)
                {
                    return new TableData<AuditLogResponse>
                    {
                        Items = result.Data ?? new(),
                        TotalItems = result.Pagination?.TotalRecords ?? 0
                    };
                }

                var errorDetails = result.GetErrorMessage(SystemMessages.ErrorOccurred);
                _errorMessage = errorDetails;
                Snackbar.Add(errorDetails, Severity.Error);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }

            return new TableData<AuditLogResponse> { Items = new List<AuditLogResponse>(), TotalItems = 0 };
        }

        private async Task ApplyFilter()
        {
            // Back to page 1, otherwise a narrower filter can leave the table on an empty page.
            // NavigateTo reloads server data itself, but does nothing when already on the first page.
            if (_table.CurrentPage == 0)
            {
                await _table.ReloadServerData();
            }
            else
            {
                _table.NavigateTo(Page.First);
            }
        }

        private async Task ClearFilter()
        {
            _query = new();
            await ApplyFilter();
        }

        private async Task FilterByCorrelation(string correlationId)
        {
            _query = new() { CorrelationId = correlationId };
            await ApplyFilter();
        }

        private async Task ClearCorrelationFilter()
        {
            _query.CorrelationId = null;
            await ApplyFilter();
        }

        private static string GetChangedProperties(AuditLogResponse auditLog)
        {
            // Created / Deleted rows list every column; only Modified and SoftDeleted rows have a meaningful diff.
            if (auditLog.Action is "Created" or "Deleted")
            {
                return $"{auditLog.Changes.Count} values";
            }

            var changed = auditLog.Changes.Where(c => c.IsChanged).Select(c => c.Property).ToList();
            return changed.Count == 0 ? "—" : string.Join(", ", changed);
        }

        private async Task OpenDetailDialog(AuditLogResponse auditLog)
        {
            var parameters = new DialogParameters
            {
                [nameof(AuditChangesDialog.Title)] = $"{auditLog.Action} {auditLog.EntityName}",
                [nameof(AuditChangesDialog.AuditLog)] = auditLog,
            };

            await DialogService.ShowAsync<AuditChangesDialog>("", parameters, _dialogOptions);
        }
    }
}
