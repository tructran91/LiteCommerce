# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

@AGENTS.md

AGENTS.md (imported above) holds the commands, architecture, conventions, and gotchas. The notes below add to it or correct it.

## Additional notes

- **Catalog API port**: `launchSettings.json` runs Catalog.API at `http://localhost:53090`, with Scalar UI at `/scalar/v1` and the OpenAPI document at `/openapi/v1.json`. The Admin UI reads `ApiSettings:CatalogUrl` from `Web/LiteCommerce.Admin/wwwroot/appsettings.json`. That value switches between `53090` and `8081` (the 8081 port in AGENTS.md is the other option), so check it before you debug connection problems.
- **Storage provider is configurable**: `Storage:Provider` in Catalog.API `appsettings.json` chooses `LocalStorageService` (`"Local"`, writes to `Storage:Local:Path`) or `AzureBlobStorageService` (`"Azure"`). Both implement `IStorageService` in `Catalog.Application/Services/`. The `if` in `Catalog.API/Extensions/ServiceExtension.cs` registers one of them.
- **DI wiring**: all Catalog registrations live in `Catalog.API/Extensions/ServiceExtension.cs` (`RegisterApplicationLayers`, `ApplySeedAsync`). This covers repositories, MediatR, AutoMapper, `ValidationBehavior`, and storage. Register new repositories and services there. Each Refit client in the Admin UI needs its own `AddRefitClient<T>()` line in `Web/LiteCommerce.Admin/Program.cs`.
- **Errors**: `ExceptionHandlingMiddleware` (in `Catalog.API/Middlewares/`) turns unhandled exceptions into HTTP responses. Handlers don't need their own try/catch for that.
- **Controller routes**: the base route has the form `[Route("api/admin/{kebab-resource}")]`, for example `api/admin/product-attribute-group`. Public, non-admin endpoints go under `api/public/...` (see `PublicFilesController`).
- **`.github/copilot-instructions.md` is partly stale**: it calls the domain project `Catalog.Domain` (the real name is `Catalog.Core`). Its controller example also uses action-named routes such as `[HttpPut("UpdateProduct")]`. Follow the RESTful route convention in AGENTS.md instead. Its other guidance still applies: records for DTOs, `StateHasChanged()` after async UI updates, `[FromForm]` for multipart uploads, and confirming before delete in the Admin UI.
- `.github/workflows/` exists but is empty, so there is still no CI.
