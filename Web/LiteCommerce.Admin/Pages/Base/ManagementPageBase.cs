using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Resources;
using LiteCommerce.Admin.Shared.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Base
{
    public class DeleteOperationHelper
    {
        private readonly IDialogService _dialogService;
        private readonly ISnackbar _snackbar;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DeleteOperationHelper(
            IDialogService dialogService,
            ISnackbar snackbar,
            IStringLocalizer<SharedResource> localizer)
        {
            _dialogService = dialogService;
            _snackbar = snackbar;
            _localizer = localizer;
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
                _snackbar.Add(_localizer["Common_DeletedMessage", itemName], Severity.Success);
            }
            else
            {
                var msg = deletedResult.Message ?? _localizer["Common_ErrorOccurred"];
                _snackbar.Add(msg, Severity.Error);
            }

            return deletedResult.IsSuccess;
        }
    }
}
