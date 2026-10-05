# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

Scope: `Web/LiteCommerce.Admin`, a Blazor WebAssembly admin SPA (net10.0) built on MudBlazor 9, Refit, and Blazored.TextEditor (rich text). Repo-wide conventions are in the root `AGENTS.md` and `CLAUDE.md`.

## Commands (run from repo root)

```bash
dotnet run --project Web/LiteCommerce.Admin     # http://localhost:5027
dotnet watch --project Web/LiteCommerce.Admin
```

The Catalog API must be running too. The client calls it directly at `ApiSettings:CatalogUrl` in `wwwroot/appsettings.json` (currently `http://localhost:53090/`, which matches the Catalog.API launch profile). There is no gateway and no auth.

## How the client talks to the API

- Each backend controller has a Refit interface in `ApiClients/I{Entity}Api.cs`. Route strings live in `Constants/ApiRoutes.cs` and must match the controller's `[Route]` and verb attributes exactly. Register every new interface with `AddRefitClient<T>()` in `Program.cs`.
- **The client does not share types with the backend.** `Models/Common/BaseResponse<T>` and `Pagination` are hand-written copies of `LiteCommerce.Shared.Models`. Each entity's DTOs live in `Models/Business/{Entity}/`: `*Response` (what the API returns), `*FormModel` (what the form binds to and sends), and `*Query` (paging state). When you change a backend DTO, update the matching client model by hand.
- **Error handling**: the API returns a `BaseResponse` JSON body for every status (400/404/409/500). Every Refit client is registered with `ApiClientSettings.Create()`, whose `ExceptionFactory` deserializes those error bodies instead of throwing `ApiException`. So:
  - Check `response.IsSuccess`, and show `response.GetErrorMessage(SystemMessages.ErrorOccurred)`, which joins field errors or falls back to `Message`.
  - Still wrap calls in `try/catch/finally`. Network failures and non-JSON responses still throw, and `_loading = false` belongs in `finally`. `BrandManagement.razor.cs` is the reference.
  - Every Refit method returns `BaseResponse<T>`. Keep it that way: a method with any other return type receives a default object on error instead of an exception.
- Update calls pass the id as the first argument (`UpdateBrandAsync(id, form)`, route `{Base}/{id}`). The id is never sent in the body or the multipart fields.
- Product create and update send a hand-built `MultipartFormDataContent` (`UpsertProduct.razor.cs` → `CreateMultipartFormDataContent`). The field names must match the backend's model binding: `Product.Name`, `Product.CategoryIds[i]`, `ThumbnailImage`, `ProductImages`, `ProductDocuments`, and so on. When you add a product field, add it to both the form model and this method.
- Category create and update build their multipart body the same way (`UpsertCategory.razor.cs`). Removing the thumbnail in edit mode sends `RemoveThumbnail=True`. Clearing the preview alone does not remove the image on the server.
- The content-image upload is called by the rich-text editor before the product exists. The page generates a temp id and sends `productId`, `isNewProduct` and `file`. It then passes that id as `ContentTempId` on create, so the backend can move the images.

## Page patterns

- There are two CRUD layouts:
  - **Simple entities** (Brand, ProductOption, ProductAttribute, ProductAttributeGroup, ProductTemplate) use a `{Entity}Management` page with a `MudTable` plus a `{Entity}Dialog.razor` opened via `IDialogService`. The dialog returns a result record, and the management page calls the API.
  - **Rich entities** (Category, Product) use `{Entity}Management` plus a separate `Upsert{Entity}` page with two routes, `/x/add` and `/x/edit/{id}`.
- For deletes, use `DeleteOperationHelper` (`Pages/Base/ManagementPageBase.cs`, named differently from its class). It shows `ConfirmDeleteDialog`, calls the API, and shows a snackbar.
- User-facing message text comes from `Constants/SystemMessages.cs`. Allowed upload content types and their messages come from `Constants/FileUploadConstants.cs`. Size limits come from the `FileUpload` section of `wwwroot/appsettings.json`, through the `FileUploadSettings` singleton (`Models/Common`, injected with `[Inject]`). Keep those limits in step with the Catalog API's own `FileUpload` section, which is what actually enforces them. Some UI text is in Vietnamese. Match the language of the surrounding page.
- Navigation comes from `Services/MenuService.GetMenuItems()`, which feeds the vertical and horizontal layouts. A new page needs an entry there to appear in the menu. Entries under the "Quản lý bán hàng" section are placeholders for services that don't exist yet.
- `AppSettingsService` saves UI preferences such as layout and theme to `localStorage` (`app-settings`). `SettingsDrawer` edits them, and layouts subscribe to `OnSettingsChanged`.
