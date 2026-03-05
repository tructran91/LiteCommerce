# GitHub Copilot Instructions for LiteCommerce

## Project Overview
LiteCommerce is an e-commerce platform built with:
- **Backend**: .NET 10 Microservices Architecture
- **Frontend**: Blazor WebAssembly
- **Patterns**: Clean Architecture, CQRS with MediatR

## Architecture

### Backend Structure
- `Services/Catalog/Catalog.API`: API Controllers (endpoints)
- `Services/Catalog/Catalog.Application`: Application logic (Commands, Queries, Handlers)
- `Services/Catalog/Catalog.Domain`: Domain entities and business rules
- `Services/Catalog/Catalog.Infrastructure`: Data access, external services

### Frontend Structure
- `Web/LiteCommerce.Admin`: Blazor WebAssembly admin application
- Follow code-behind pattern (.razor + .razor.cs)

## Coding Standards

### C# Conventions
- Use C# 14.0 features when appropriate
- Target .NET 10
- Use `async`/`await` for all I/O operations
- Prefer `record` types for DTOs and requests
- Use nullable reference types
- Follow PascalCase for public members, camelCase for private fields with `_` prefix

### API Controllers
- Use `[ApiController]` attribute
- Route pattern: `[Route("api/admin/{resource}")]`
- Return `IActionResult` or `ActionResult<T>`
- Use `[FromQuery]` for GET requests
- Use `[FromBody]` for JSON payloads (PUT/PATCH)
- Use `[FromForm]` for multipart/form-data (file uploads)

### CQRS Pattern
- **Commands**: Use for Create, Update, Delete operations
  - Place in `Catalog.Application/Products/Commands/`
  - Handler suffix: `{CommandName}Handler`
  - Return response objects wrapped in `BaseResponse<T>`

- **Queries**: Use for Read operations
  - Place in `Catalog.Application/Products/Queries/`
  - Handler suffix: `{QueryName}Handler`

### MediatR
- Inject `IMediator` in controllers
- Use `await _mediator.Send(command)` pattern
- Keep controllers thin - all logic in handlers

### Blazor Components
- Use code-behind pattern (.razor + .razor.cs)
- Inject services via `[Inject]` attribute
- Use `StateHasChanged()` after async operations that update UI
- Follow component naming: PascalCase for files and components

### Error Handling
- Return proper HTTP status codes:
  - 200 OK: Success
  - 201 Created: Resource created
  - 400 Bad Request: Validation errors
  - 404 Not Found: Resource not found
  - 500 Internal Server Error: Unhandled exceptions

### Validation
- Use FluentValidation for request validation
- Validate in command/query handlers
- Return validation errors in response

## Common Patterns

### Creating a New Endpoint
1. Create Request DTO in `Catalog.Application/Requests/`
2. Create Command in `Catalog.Application/Products/Commands/`
3. Create Handler in `Catalog.Application/Products/Handlers/`
4. Add endpoint in `Catalog.API/Controllers/`

### Example Structure
```csharp
// Request
public record UpdateProductRequest(Guid Id, string Name, ...);

// Command
public record UpdateProductCommand(UpdateProductRequest Request) : IRequest<BaseResponse<ProductResponse>>;

// Handler
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, BaseResponse<ProductResponse>>
{
    // Implementation
}

// Controller
[HttpPut("UpdateProduct")]
public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
{
    var command = new UpdateProductCommand(request);
    var result = await _mediator.Send(command);
    return Ok(result);
}
```

## Project-Specific Guidelines

### Products Module
- Product entity is in Catalog domain
- Support image uploads (multipart/form-data)
- Track created/updated timestamps
- Soft delete preferred over hard delete

### Admin UI
- Use Blazor EditForm for forms
- Show loading states during API calls
- Display validation errors from API
- Confirm before delete operations

## Dependencies
- MediatR: Command/Query pattern
- FluentValidation: Request validation
- Entity Framework Core: Data access

## Testing
- Write unit tests for handlers
- Mock IMediator in controller tests
- Test validation rules separately

## When Generating Code
- Follow existing patterns in the codebase
- Match naming conventions of similar files
- Include proper error handling
- Add XML comments for public APIs
- Consider async/await best practices
