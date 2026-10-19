# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

Scope: `Web/LiteCommerce.Admin`, a Blazor WebAssembly admin SPA (net10.0) built on MudBlazor 9, Refit, and Blazored.TextEditor (rich text). Repo-wide conventions are in the root `AGENTS.md` and `CLAUDE.md`.

## Commands (run from repo root)

```bash
dotnet run --project Web/LiteCommerce.Admin     # http://localhost:5027
dotnet watch --project Web/LiteCommerce.Admin
```

The Catalog API and the Identity API must both be running. The client calls them directly, with no gateway, at `ApiSettings:CatalogUrl` and `ApiSettings:IdentityUrl` in `wwwroot/appsettings.json`. These currently point at `http://localhost:8081/` and `http://localhost:8082/` (IIS). The commented-out pair `53090` / `5131` matches the `dotnet run` launch profiles. Catalog's own `Identity:Authority` must point at the same Identity instance, or every token is rejected.

## Authentication

- **Every page requires login.** `_Imports.razor` applies `@attribute [Authorize]`. `Pages/Auth/Login.razor` opts out with `[AllowAnonymous]` and uses `BlankLayout`. `App.razor` uses `AuthorizeRouteView`: an anonymous user is sent to `/login?returnUrl=...` by `Components/RedirectToLogin.razor`, and an authenticated user without access sees an inline "no access" view.
- **Login flow** (`Services/AuthService.LoginAsync`): Identity `POST api/auth/login`, then Catalog `GET api/admin/session` with that token, and only then are the tokens stored. The app is Admin-only, so if Catalog rejects the session (for example a Customer account), the refresh token is revoked (best effort) and Catalog's message is shown. If Catalog is unreachable, nothing is stored.
- **Tokens** live in `localStorage` (`auth-tokens`) through `TokenStorageService`, which also caches them in memory. `JwtAuthenticationStateProvider` builds the `ClaimsPrincipal` from the JWT payload (`JwtParser`, no signature check; the API does that). The claim names are the short ones Identity writes: `email` (name), `role`, `sub`. If the access token has expired, the provider calls `AuthService.RefreshAsync()` before deciding, so reopening the app after the access token lifetime does not bounce a user with a live refresh token to `/login`.
- **Auth-state notifications** go through one event: `TokenStorageService.TokensChanged` fires on every save and clear, and `JwtAuthenticationStateProvider` subscribes to it. `AuthService` never references the provider (the provider depends on `AuthService`, so the reverse would be a cycle). Anything that changes tokens must go through `TokenStorageService`.
- **Attaching the token**: every Catalog Refit client is registered with `.AddHttpMessageHandler<AuthHeaderHandler>()`. The handler adds `Authorization: Bearer` and refreshes an access token that is about to expire. On a 401 it calls `RefreshAsync(rejectedToken)`, which forces a refresh even for an unexpired token, then retries once. A 401 on the retry clears the tokens. A new Catalog client needs that `.AddHttpMessageHandler<AuthHeaderHandler>()` call too, or its requests go out anonymous and get 401.
- `IIdentityApi` and `ISessionApi` deliberately have **no** `AuthHeaderHandler`. `AuthService` depends on them, so adding it would create a cycle. Methods that need a token take it as a `[Authorize("Bearer")] string token` argument.
- `IIdentityAuditLogApi` and `IIdentityActivityLogApi` point at `IdentityUrl` and **do** use `AuthHeaderHandler` (the log endpoints are AdminOnly, and `AuthService` does not depend on them, so there is no cycle).

## Log pages

- `/activity-logs` and `/audit-logs` (`Pages/Logs/`) read either service, chosen by the **Source** selector (`Enums/LogSource`). Switching source clears the filters, because entity names, events and correlation ids belong to one service.
- Both services serve the same routes (`ApiRoutes.AuditLog` / `ApiRoutes.ActivityLog`) on different base URLs. `IIdentityAuditLogApi` just inherits `IAuditLogApi` to get a second Refit registration. The activity endpoints have different filters, so `IIdentityActivityLogApi` is its own interface.
- `ActivityLogResponse` is one model for both: Catalog fills the entity fields, Identity fills `IsSuccess`, `FailureReason`, `Email`, `IpAddress`, `UserAgent`. The Activity page swaps its columns and filters on `IsIdentity`.
- `AuditChangesDialog` takes a `Source` parameter, so a correlation-id lookup asks the right service. Correlation ids never span services.
- Paging and filtering are server-side (`MudTable ServerData`), with `@bind-RowsPerPage` so a filter change does not reset the page size.
- **The auth services are singletons, and must stay that way.** `TokenStorageService`, `JwtAuthenticationStateProvider`, `AuthenticationStateProvider` and `AuthService` are singletons, and `AuthHeaderHandler` is transient. `IHttpClientFactory` builds each client's handler chain in its own DI scope. If these were scoped, every handler would get its own token cache, refresh lock and state provider. The results would be stale tokens after logout, parallel refreshes that Identity treats as token theft (revoking every session), and 401 logouts the UI never hears about.
- Refresh is serialized by a `SemaphoreSlim` in `AuthService`. Identity rotates refresh tokens and revokes all sessions when one is reused, so never refresh outside `AuthService.RefreshAsync`. It clears the tokens only when Identity answers 401/400 (refresh token expired, revoked or reused). A 429, a 5xx or a network error keeps them for a later retry.
- Change password (`Pages/Auth/ChangePassword`) calls Identity, which revokes every refresh token. The client then clears its tokens and goes to `/login`. `Components/UserMenu.razor` shows the signed-in email and handles logout (best-effort revoke, then clear).

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
