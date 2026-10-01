using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ActivityLog;
using LiteCommerce.Admin.Pages.Logs.AuditLogs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Logs.ActivityLogs
{
    public partial class ActivityLogManagement
    {
        [Inject]
        private IActivityLogApi ActivityLogApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private List<BreadcrumbItem> _breadcrumbs = new()
        {
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("System", href: null, disabled: true),
            new("Activity Logs", href: null, disabled: true),
        };

        // Logs only grow, so paging happens on the server (unlike the Catalog pages, which page in memory).
        private readonly int[] _pageSizeOptions = { 10, AppConstants.PageSize, 50, 100 };

        // Bound two-way: a constant RowsPerPage would be re-applied on every render and undo the user's choice.
        private int _rowsPerPage = AppConstants.PageSize;

        private MudTable<ActivityLogResponse> _table = null!;
        private string? _errorMessage;
        private ActivityLogQuery _query = new();

        private readonly DialogOptions _dialogOptions = new()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true,
        };

        private async Task<TableData<ActivityLogResponse>> LoadServerData(TableState state, CancellationToken cancellationToken)
        {
            _errorMessage = null;

            try
            {
                var result = await ActivityLogApi.GetActivityLogsAsync(
                    state.Page + 1,
                    state.PageSize,
                    search: string.IsNullOrWhiteSpace(_query.Search) ? null : _query.Search.Trim(),
                    entityName: _query.EntityName,
                    action: _query.Action,
                    fromDate: LogConstants.ToUtcStartOfDay(_query.FromDate),
                    toDate: LogConstants.ToUtcEndOfDay(_query.ToDate));

                if (result.IsSuccess)
                {
                    return new TableData<ActivityLogResponse>
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

            return new TableData<ActivityLogResponse> { Items = new List<ActivityLogResponse>(), TotalItems = 0 };
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

        private async Task OnSearchKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await ApplyFilter();
            }
        }

        private async Task OpenChangesDialog(ActivityLogResponse activityLog)
        {
            var parameters = new DialogParameters
            {
                [nameof(AuditChangesDialog.Title)] = activityLog.Description,
                [nameof(AuditChangesDialog.CorrelationId)] = activityLog.CorrelationId,
            };

            await DialogService.ShowAsync<AuditChangesDialog>("", parameters, _dialogOptions);
        }
    }
}
