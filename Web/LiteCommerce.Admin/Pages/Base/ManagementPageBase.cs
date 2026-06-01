using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Shared.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Base
{
    public class DeleteOperationHelper
    {
        private readonly IDialogService _dialogService;
        private readonly ISnackbar _snackbar;

        public DeleteOperationHelper(IDialogService dialogService, ISnackbar snackbar)
        {
            _dialogService = dialogService;
            _snackbar = snackbar;
        }

        public async Task<bool> ShowDeleteConfirmDialog(string itemName)
        {
            var parameters = new DialogParameters
            {
                [nameof(ConfirmDeleteDialog.ItemName)] = itemName,
            };
            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true,
                CloseOnEscapeKey = true,
            };

            var dialog = await _dialogService.ShowAsync<ConfirmDeleteDialog>("", parameters, options);
            var result = await dialog.Result;

            return result is not null && !result.Canceled;
        }

        public async Task<bool> ExecuteDeleteOperation<TId, TResult>(
            TId id,
            string itemName,
            Func<TId, Task<BaseResponse<TResult>>> deleteApiCall,
            Func<Task> onSuccess,
            Action? setLoading = null)
        {
            if (!await ShowDeleteConfirmDialog(itemName))
                return false;

            setLoading?.Invoke();

            var deletedResult = await deleteApiCall(id);

            if (deletedResult.IsSuccess)
            {
                await onSuccess();
                _snackbar.Add($"Đã xóa \"{itemName}\"", Severity.Success);
            }
            else
            {
                var msg = deletedResult.Message ?? SystemMessages.ErrorOccurred;
                _snackbar.Add(msg, Severity.Error);
            }

            return deletedResult.IsSuccess;
        }
    }
}
