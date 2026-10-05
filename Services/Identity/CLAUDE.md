# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

Scope: the Identity microservice (`Services/Identity`). Repo-wide conventions are in the root `AGENTS.md` and `CLAUDE.md`. Catalog-specific notes are in `Services/Catalog/CLAUDE.md`. This file covers how Identity differs from Catalog. It does **not** follow the Catalog CQRS/MediatR pattern.

## Commands (run from repo root)

```bash
dotnet build Services/Identity/Identity.API
dotnet run --project Services/Identity/Identity.API       # http://localhost:5131, Scalar UI at /scalar/v1, OpenAPI at /openapi/v1.json
```

EF commands (no local tool manifest here, so use the global `dotnet-ef`). Migrations live in `Identity.Infrastructure/Data/Migrations`:

```bash
dotnet ef migrations add <Name> --project Services/Identity/Identity.Infrastructure --startup-project Services/Identity/Identity.API --output-dir Data/Migrations
dotnet ef database update --project Services/Identity/Identity.Infrastructure --startup-project Services/Identity/Identity.API
```

There are no tests. Startup **does not migrate**. `IdentityDbInitializer.SeedAdminAsync` runs right after `app.Build()` and throws if the database or tables are missing, so run `database update` first on a fresh database.

## Layout

```
Identity.API            → Program.cs (JWT bearer validation, Scalar/OpenAPI), AuthController
Identity.Application    → DTOs (records), IAuthService / ITokenService, JwtOptions, FluentValidation validators
Identity.Core           → Entities (User, Role, RefreshToken, BaseEntity), IdentityRoles constants
Identity.Infrastructure → IdentityDbContext, EF configurations + migrations, AuthService, JwtTokenService, DI, admin seeder
```

Dependency flow: `API → Application → Core ← Infrastructure`. `API` also references `Infrastructure` directly to call `AddInfrastructureServices`.

- **No MediatR.** The controller calls `IAuthService` directly, and `AuthService` (in **Infrastructure**, not Application) holds all the business logic. It uses `IdentityDbContext` directly. There is no repository layer.
- **Validation is manual.** `AuthService.Validate(...)` runs the FluentValidation validators and builds the same `Dictionary<string, List<string>>` error shape as Catalog (camelCase keys). There is no `ValidationBehavior` and no exception middleware, so any unexpected exception is an unhandled 500.
- **Responses** are `BaseResponse<T>` from `LiteCommerce.Shared`. `BaseApiController.ToActionResult` sets the HTTP status from `BaseResponse.StatusCode`, so the service picks the status (`Created`, `Conflict`, `Unauthorized`, `NotFound`; the default `Failure` is 400).
- **All DI is in `Identity.Infrastructure/DependencyInjection.cs`** (`AddInfrastructureServices`): DbContext, `JwtOptions`, `IPasswordHasher<User>`, `ITokenService`, `IAuthService`, validators. `Program.cs` additionally registers JWT bearer auth.

## Endpoints (`api/auth`)

| Route | Auth | Notes |
|---|---|---|
| `POST register` | public | Always creates a `Customer`. Returns 201 with tokens. 409 if the email exists. |
| `POST login` | public | Generic "Invalid email or password." for unknown, inactive or wrong password. |
| `POST refresh` | public | Rotates the refresh token. Presenting an already-rotated token revokes **all** of that user's tokens. |
| `POST revoke` | public | Logout. Idempotent, and unknown tokens still return success. |
| `GET me` | `[Authorize]` | Reads `sub` (or `NameIdentifier`) from the JWT, then loads the user. |

## Auth design

- **RS256 with PEM key files.** `JwtTokenService` (signer) loads `Jwt:PrivateKeyPath`. `Program.cs` (verifier) loads only `Jwt:PublicKeyPath`. Other services should verify with the public key and the same `Issuer` / `Audience` / `KeyId`. Dev keys are in `Identity.API/Keys/` and are copied to the output directory. Prod must override the paths.
- Access token: 15 min (`AccessTokenMinutes`), `ClockSkew = 0`. Claims: `sub`, `email`, `role` (written via `ClaimTypes.Role`), `jti`. Refresh token: 64 random bytes in base64, 7 days (`RefreshTokenDays`), stored in `RefreshTokens.Token` (unique index).
- Rotation: the old token gets `Revoked` and `ReplacedByToken`. Reuse detection keys on `Revoked != null && ReplacedByToken != null`. A token revoked by logout has no `ReplacedByToken`, so it is rejected without nuking the other sessions.
- Roles are seeded through `HasData` in `RoleConfiguration` (fixed GUIDs for `Admin`, `Staff`, `Customer`), not by the initializer. The first admin comes from `SeedAdmin:*` config and is created only if that email does not exist.
- Passwords use `PasswordHasher<User>` from `Microsoft.Extensions.Identity.Core`. This is not the full ASP.NET Identity stack.
- **Login protection.** 5 consecutive wrong passwords lock the account for 15 min (`User.FailedLoginCount` / `LockoutEnd`, constants in `AuthService`). A locked account gets 429. An unknown email still runs a dummy hash verify so timing does not reveal it. `AuthController` is also behind a per-IP fixed-window rate limiter (policy `auth`, 20 req/min, configured in `Program.cs`). Behind a reverse proxy, enable `ForwardedHeaders`, otherwise every client shares the proxy IP.
- **Concurrency.** `RefreshToken.RowVersion` is a concurrency token, so only one of two parallel refreshes wins (the loser gets 401). `Register` turns a unique-index race into 409. `JwtTokenService` is a singleton (loads the PEM once).

## Conventions and gotchas

- `BaseEntity` is a copy of Catalog's (`Id`, `CreatedDate`, `LastModifiedDate`, `IsDeleted`) with the same soft-delete global query filter, built reflectively in `IdentityDbContext.OnModelCreating`. Unique indexes use `HasFilter("[IsDeleted] = 0")`.
- **There is no `AuditableEntityInterceptor` here.** Set `CreatedDate` and `LastModifiedDate` by hand in the service (existing code does). There is also no audit log or activity log.
- Emails are normalised with `Trim().ToLowerInvariant()` before storage and lookup.
- `ExecuteUpdateAsync` (used by `RevokeAllUserTokensAsync`) bypasses the change tracker, so it does not touch `LastModifiedDate`.
- Scalar and OpenAPI are mapped in **all** environments **on purpose** (the `IsDevelopment` guard in `Program.cs` is commented out deliberately). Do not "fix" this.
- The connection string, `SeedAdmin` password and dev keys are in `appsettings.json` / the repo. Treat them as dev-only and never reuse them elsewhere.
- Catalog.API does not validate these tokens yet, so nothing is protected by Identity so far.

## Known issues (from the review on 2026-10-05)

See the review summary in the conversation or PR. In priority order:

1. `Keys/dev-private.pem` is untracked but **not git-ignored**. `.gitignore` has no `*.pem` or `Keys/` rule, so it would be committed. SQL password and seed admin password are in `appsettings.json`.
2. Refresh tokens are stored in plaintext, so a DB leak yields live sessions. Store a SHA-256 hash instead.
3. No cleanup of expired or revoked `RefreshTokens`, so the table grows forever.
4. Seed admin has a weak default password committed in config and is created in every environment.
5. Revocation, deactivation or a role change does not affect issued access tokens for up to 15 min.
