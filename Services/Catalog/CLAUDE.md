# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

Scope: the Catalog microservice (`Services/Catalog`). Repo-wide conventions are in the root `AGENTS.md` and `CLAUDE.md`. This file covers how the Catalog code fits together.

## Commands (run from repo root)

```bash
dotnet build Services/Catalog/Catalog.API
dotnet run --project Services/Catalog/Catalog.API          # http://localhost:53090, Scalar UI at /scalar/v1, OpenAPI at /openapi/v1.json
```

`dotnet-ef` is a **local tool** whose manifest is in `Catalog.API/.config`, so run EF commands from `Services/Catalog/Catalog.API` (run `dotnet tool restore` once):

```bash
dotnet ef migrations add <Name> --project ../Catalog.Infrastructure --startup-project . --output-dir Data/Migrations
dotnet ef database update --project ../Catalog.Infrastructure --startup-project .
```

There are no tests. Migrations live in `Catalog.Infrastructure/Data/Migrations` (not the EF default `Migrations/`), so pass `--output-dir Data/Migrations` when you add one. Startup **does not migrate**. `ApplySeedAsync` only seeds, and only when the `Brands` table is empty. A fresh database needs `database update` first. Seed errors (including an unknown `Seeding:Profile`) are logged at Error level and swallowed, so the app still starts.

## Request pipeline

`Controller → IMediator.Send → ValidationBehavior → ActivityLogBehavior → Handler → repository / services → BaseResponse<T>`

- Pipeline behaviors run in registration order (`AddInfrastructureServices`). `ActivityLogBehavior` sits after validation, so rejected requests are never logged, and it only writes when the response `IsSuccess`. A failed log write is logged and swallowed.
- `CatalogContext` has two `SaveChangesInterceptor`s, registered in this order: `AuditableEntityInterceptor` (dates) then `AuditLogInterceptor`. The audit interceptor skips `Modified` entries whose values did not actually change (`UpdateAsync` marks every column modified), records `IsDeleted` false → true as `SoftDeleted`, and does nothing while `CatalogContext.IsAuditEnabled` is false (the seed turns it off). `ExecuteUpdate/ExecuteDelete` bypass the change tracker and would not be audited.

- **Composition root**: `Catalog.API/Extensions/ServiceExtension.cs`. MediatR handlers, validators (`AddValidatorsFromAssembly`) and AutoMapper profiles are scanned from the `Catalog.Application` assembly, using `AssemblyReference` as the marker. New handlers and validators need no registration, but new services and repositories do.
- **Repositories**: entity CRUD goes through the generic `IBaseRepository<T>` (`BaseRepository<T>` requires `T : BaseEntity`). Only `Product` has a dedicated `IProductRepository`, for the eager-loaded aggregate (`GetProductAsync`) and the projected list and pricing queries that return `Catalog.Core/DTOs`. `Add/Update/DeleteAsync` each call `SaveChangesAsync` right away. There is no unit-of-work, so a handler that touches several aggregates saves them separately.
- `GetAsync` always pages (`Skip/Take`, default page size 10) and orders by `CreatedDate` unless you pass `orderBy`. Use `Query()` when you need an unpaged or custom query.

## Response and error convention

**Every response body is a `BaseResponse<T>`, and the HTTP status equals `BaseResponse.StatusCode`.**

| Outcome | Produced by | HTTP |
|---|---|---|
| Success | handler `BaseResponse.Success(data)` | 200 |
| Created | handler `Success(data, statusCode: HttpStatusCode.Created)` + controller `ToCreatedResult` (adds `Location`) | 201 |
| Unknown route / wrong method / wrong media type | `UseBaseResponseStatusCodePages` (`SuppressMapClientErrors = true`) | 404 / 405 / 415 |
| Invalid input | `ValidationBehavior` throws `ValidationException` → middleware | 400, `errors` keyed by client field name (`name`, `product.categoryIds[0]`) |
| Malformed body / binding error | `InvalidModelStateResponseFactory` (`ConfigureBaseResponseErrors`) | 400 |
| Not found | handler `return Failure(..., HttpStatusCode.NotFound)` | 404 |
| Duplicate / entity still in use | handler `return Failure(..., HttpStatusCode.Conflict)` | 409 |
| Unique index violation that slipped past the handler's check (race) | middleware (`DbUpdateException` + SQL 2601/2627) | 409 |
| Unexpected exception | middleware, generic message, details only in the log | 500 |

- Expected failures are **returned** from the handler. Only unexpected ones are **thrown**. Never wrap a handler body in `catch (Exception) → Failure(ex.Message)`: that turns a 500 into a 400 and leaks internals.
- Controllers must `return ToActionResult(result)` (from `BaseApiController`), never `Ok(result)`, and declare a `[ProducesResponseType(typeof(BaseResponse<T>), code)]` for each status the action can return. `AdminBrandController` is the reference implementation.
- Verbs:
  - Update is `PUT /{resource}/{id}`. The command carries `Id` from the route next to `Payload`, and the request DTO has no `Id` (see `UpdateBrandCommand`).
  - A partial bulk update is `PATCH` on the collection (`PATCH /product-price`).
  - An upload that creates a file returns 201 with `Location` set to the file URL.
- Error bodies written outside controllers (middleware, status pages, model binding) use `ErrorJson.Options`, which omits nulls. A `"data": null` would fail to deserialize on clients typed `BaseResponse<bool>`.
- Ad-hoc SQL against tables with filtered indexes (`Brands`, `Categories`, `Products`) needs `SET QUOTED_IDENTIFIER ON`. `sqlcmd` needs `-I`; SSMS has it on by default.

## Uniqueness and delete guards

- Unique, filtered (`[IsDeleted] = 0`) indexes:
  - `Brand.Name`, `Brand.Slug`
  - `Category (ParentId, Name)`. Names are unique per parent only, and the seed data has "Gaming" under two parents. Category slugs are **not** unique.
  - `Product.Slug`

  Lengths come from `Catalog.Core/Constants/FieldLength.cs`, which both the EF configurations and the validators use (SQL Server cannot index `nvarchar(max)`).
- Handlers check for duplicates first and return 409. Brand checks name **or** slug, because different names can slugify to the same value. The index is the backstop for races.
- Category update also checks the new parent: 404 if it does not exist, 400 if it is the category itself or one of its descendants (`UpdateCategoryHandler.ValidateParentAsync`).
- Delete guards count the dependents and return 409 with `ErrorMessages.CannotDeleteInUse(...)` (`LiteCommerce.Shared.Constants`, next to `ValidationMessages`), so every message reads `Cannot delete brand "Apple" because 3 products are still linked to it.`. Don't hand-write these messages. The guarded cases are:
  - Brand used by products
  - Category with subcategories or assigned products
  - Option or Attribute used by products
  - Attribute used by templates
  - Attribute Group that still has attributes

## Validators

The query and command classes are validated, not the request DTOs. Id fields are `string` and checked with `GuidValidator.IsValidGuid` (from `LiteCommerce.Shared.Validators`). Messages come from `LiteCommerce.Shared.Constants.ValidationMessages`. Handlers then call `Guid.Parse` directly and trust the validator. Paging is bounded by `PaginationSetting` (max page size 100).

## Product aggregate

The `Product` aggregate is the complex part. Everything else is simple single-table CRUD.

- Create and update are **multipart/form-data** (`[FromForm]`). `CreateProductRequest`/`UpdateProductRequest` wrap a `ProductViewModel Product` plus `IFormFile` thumbnail, images and documents. The client sends fields as `Product.Name`, `Product.CategoryIds[0]`, and so on.
- `UpdateProductHandler` works in a fixed order. First it captures the original prices and runs `_mapper.Map(viewModel, product)`. Then it re-slugifies, adds a `ProductPriceHistory` if any price field changed, and syncs collections through `IProductService.AddOrDelete{Options,Attributes,Categories,ProductLinks}`. Last it deletes the media in `DeletedMediaIds`, saves new files, and calls `UpdateAsync`. Keep price capture **before** the map.
- **Rich-text content images**: `upload-content-image` saves into `Product/temp/{tempId}/` for a product that doesn't exist yet, or into `Product/{productId}/` for an existing one. On create, `CreateProductHandler` moves the temp folder into the product folder and rewrites the temp URLs inside `Description`.
- **Delete** soft-deletes the product and every `ProductLink` involving it in both directions (its own related/cross-sell links and other products' links pointing at it), in one `SaveChanges`. A live link to a deleted product would make `GetProductAsync` return a link with a null `LinkedProduct`. Media files are kept, so a restore is still possible.

## Media and storage

- `IMediaService` (Media entity plus file naming) sits on top of `IStorageService`. The `Storage:Provider` setting picks `LocalStorageService` or `AzureBlobStorageService`. Product files go under `Product/{productId}/` (`Guid.ToStoragePath`). Category thumbnails are stored flat in `Category/`, with no per-id subfolder.
- `Media.FileName` stores **only** the file name (`{guid}.ext`). Pass the same subfolder the file was saved under to `DeleteMediaAsync(fileName, subFolder)`, or the delete silently misses.
- **File-vs-DB ordering**: save the DB first, then delete the old files. If the save fails, delete the files you just uploaded. `UpdateProductHandler` and `UpdateCategoryHandler` show the pattern.
- The database stores relative file paths. **Controllers** convert them to absolute URLs through `BaseApiController.BuildImageUrl`, which points at `api/public/files/...`. `PublicFilesController` serves those files from disk, but only with the Local provider. Any new endpoint that returns media must call `BuildImageUrl` on every URL field, as `AdminProductController` does (`AdminCategoryController.ApplyImageUrl` covers GET, POST and PUT).
- `IMediaService.GetMediaUrl/GetThumbnailUrl(Media?)` return `string.Empty` for a null media. There is no placeholder image, so the client shows its own. Handlers fill URL fields themselves (`CategoryResponse.ThumbnailImageUrl` is `Ignore`d in `MappingProfile`) and leave them null when there is no media.
- **Upload validation**: validators call `MustBeValidImage` / `MustBeValidDocument` (`Catalog.Application/Extensions/FileValidationExtensions.cs`). They check only that the file is non-empty, within the size limit, and has an allowed extension. Content-Type and file contents are deliberately not checked. Size limits come from the `FileUpload` section of `appsettings.json` (`FileUploadSettings`, injected into validators as `IOptions<FileUploadSettings>`). The Admin UI has its own copy of these limits, so change both.
- **Category thumbnail**: on update, a new `ThumbnailImage` replaces the file in place on the existing `Media` row. `RemoveThumbnail = true` without a file soft-deletes the `Media` and deletes the file after the save. A failure while deleting the old file is logged, not thrown. Deleting a category keeps its `Media` and file, the same as for products.

## Data model notes

- Every entity except `ProductAttributeValue` derives from `BaseEntity` (Guid `Id`, dates, `IsDeleted`), so the global soft-delete filter applies to all of them. `ProductAttributeValue` cannot use `IBaseRepository<T>`; query it through `Product.AttributeValues`. Several handlers still add a redundant `!t.IsDeleted` predicate.
- Join and child entities (`ProductCategory`, `ProductMedia`, `ProductLink`, `ProductOptionValue`, `ProductAttributeValue`, `ProductTemplateProductAttribute`) are synced by adding and removing them on the aggregate's collections in `ProductService`. They are not managed through their own repositories.
- Fluent configurations in `Catalog.Infrastructure/Data/Configurations` are applied automatically via `ApplyConfigurationsFromAssembly`.
- `Slugify()` (`Catalog.Application/Extensions/StringExtension.cs`) generates slugs from `Name` for brands, categories and products.
