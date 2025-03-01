﻿using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.Brand;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Pages.Catalog.Brands
{
    public partial class BrandManagement
    {
        [Inject]
        private IBrandApi BrandApi { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public SweetAlertService SweetAlertService { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Brands", Url = "/brands", IsActive = true }
        };

        private List<BrandModel> brands = new();

        private BrandModel addEditBrand = new BrandModel();

        private bool isLoading = true;

        private bool isEditMode = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetBrands();
        }

        private async Task ShowModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.showModal", "brandModal");
        }

        private async Task HideModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.hideModal", "brandModal");
        }

        private async Task GetBrands()
        {
            var response = await BrandApi.GetBrandsAsync(1, 100);
            if (response.IsSuccess)
            {
                brands = response.Data;
            }
            isLoading = false;
        }

        private async Task OpenAddModal()
        {
            isEditMode = false;
            addEditBrand = new BrandModel();
            await ShowModal();
        }

        private async Task OpenEditModal(string brandId)
        {
            isEditMode = true;
            var request = await BrandApi.GetBrandAsync(brandId);
            if (request.IsSuccess)
            {
                var brandToEdit = request.Data;
                addEditBrand = new BrandModel
                {
                    Id = brandToEdit.Id,
                    Name = brandToEdit.Name,
                    IsPublished = brandToEdit.IsPublished
                };
                await ShowModal();
            }
            else
            {
                ToastService.ShowError(request.Message);
            }
        }

        private async Task OpenDeleteModal(string brandId, string brandName)
        {
            var confirmationResult = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirm Brand Deletion",
                Html = $"Are you sure you want to delete the brand <strong>{brandName}</strong>? This action cannot be undone.",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Yes, delete it!",
                CancelButtonText = "Cancel"
            });

            if (confirmationResult.IsConfirmed)
            {
                var deletedResult = await BrandApi.DeleteBrandAsync(brandId);

                if (deletedResult.IsSuccess)
                {
                    ToastService.ShowSuccess(SystemMessages.DeleteDataSuccess);
                    await GetBrands();
                }
                else
                {
                    ToastService.ShowError(deletedResult.Message);
                }
            }
        }

        private async Task FormSubmitted()
        {
            if (isEditMode)
            {
                var response = await BrandApi.UpdateBrandAsync(addEditBrand);
                if (response.IsSuccess)
                {
                    await GetBrands();
                    await HideModal();
                    ToastService.ShowSuccess(SystemMessages.UpdateDataSuccess);
                }
                else
                {
                    ToastService.ShowError(response.Message);
                }
            }
            else
            {
                var request = await BrandApi.CreateBrandAsync(addEditBrand);
                if (request.IsSuccess)
                {
                    await GetBrands();
                    await HideModal();
                    ToastService.ShowSuccess(SystemMessages.AddDataSuccess);
                }
                else
                {
                    ToastService.ShowError(request.Message);
                }
            }
        }
    }
}
