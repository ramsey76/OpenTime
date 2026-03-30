---
name: api-minimal
description: Define and scaffold a .NET Minimal API using a thin-handler, service-layer pattern that is testable by default with Swagger support.
---

# Minimal API Structure Pattern

Use this pattern for .NET Minimal APIs that should stay simple, reusable, and testable.

## Goal

Generated code must be testable by default.

Use these boundaries:
- endpoint groups define routes
- handlers translate HTTP requests into service calls
- services contain business logic and data access orchestration
- services do not return `IResult` or depend on HTTP concerns
- api must implement swagger, so that we can test our endpoints

## Project Structure

```text
MyProject.Api/
├── Program.cs
├── Extensions/
│   └── EndpointExtensions.cs
├── Endpoints/
│   ├── EndPoints.cs
│   └── {Resource}/
│       ├── {Resource}Endpoints.cs
│       ├── Get{Resources}.cs
│       ├── Get{Resource}ById.cs
│       ├── Create{Resource}.cs
│       ├── Update{Resource}.cs
│       └── Delete{Resource}.cs
├── DTOs/
│   ├── {Resource}Dto.cs
│   ├── Create{Resource}Request.cs
│   └── Update{Resource}Request.cs
└── Services/
    └── {Resource}/
        ├── I{Resource}Service.cs
        └── {Resource}Service.cs
```

If tests live alongside source projects in the solution, keep the test projects under `src/`, not inside the API project folder.

## Core Rules

1. Keep endpoint groups focused on route registration only.
2. Keep handlers thin: bind input, call a service, map the result to HTTP.
3. Put business rules, validation, duplicate checks, and EF Core logic in services.
4. Use interfaces plus concrete service implementations.
5. Return DTOs or result objects from services, not `IResult`.
6. Register services explicitly in DI.
7. Unit test the service layer first.

## Base Pattern

### Endpoint Base Class

```csharp
public abstract class EndPoints
{
    public abstract void MapEndpoints(WebApplication app);
}
```

### Auto-Registration

```csharp
using System.Reflection;

public static class EndpointExtensions
{
    public static void MapEndpointsFromAssembly(this WebApplication app)
    {
        var endpointTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(EndPoints)) && !type.IsAbstract);

        foreach (var endpointType in endpointTypes)
        {
            if (Activator.CreateInstance(endpointType) is EndPoints endpoint)
            {
                endpoint.MapEndpoints(app);
            }
        }
    }
}
```

## Example Resource

### Endpoint Group

```csharp
public sealed class UserEndpoints : EndPoints
{
    public override void MapEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users");

        group.MapGet("/", GetUsers.Handle)
            .WithName("GetUsers")
            .Produces<List<UserDto>>(StatusCodes.Status200OK);

        group.MapPost("/", CreateUser.Handle)
            .WithName("CreateUser")
            .Produces<UserDto>(StatusCodes.Status201Created)
            .Produces<string>(StatusCodes.Status400BadRequest);
    }
}
```

### Thin Handler

```csharp
public static class CreateUser
{
    public static async Task<Results<Created<UserDto>, BadRequest<string>>> Handle(
        CreateUserRequest request,
        IUserService userService,
        CancellationToken cancellationToken)
    {
        var result = await userService.CreateAsync(request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"/api/users/{result.Value!.Id}", result.Value)
            : TypedResults.BadRequest(result.Error!);
    }
}
```

### Service Contract

```csharp
public interface IUserService
{
    Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<CreateUserResult> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken);
}
```

### Service Implementation

```csharp
using Microsoft.EntityFrameworkCore;

public sealed class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<CreateUserResult> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Users
            .AnyAsync(user => user.Email == request.Email, cancellationToken);

        if (exists)
        {
            return CreateUserResult.Failure("A user with this email already exists.");
        }

        var entity = new User
        {
            Name = request.Name,
            Email = request.Email
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return CreateUserResult.Success(new UserDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email
        });
    }
}
```

### Result Type

```csharp
public sealed record CreateUserResult(bool IsSuccess, UserDto? Value, string? Error)
{
    public static CreateUserResult Success(UserDto value) => new(true, value, null);
    public static CreateUserResult Failure(string error) => new(false, null, error);
}
```

## Program Setup

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapEndpointsFromAssembly();
```

## Testing Guidance

Test services first. That is the main unit-test seam.

Cover at least:
- success paths
- not found paths
- validation failures
- duplicate checks
- state changes

Keep handler tests lightweight and focused on HTTP mapping. If a handler becomes hard to test, business logic has likely leaked out of the service layer.

## Adding a New Resource

1. Add `Endpoints/{Resource}/{Resource}Endpoints.cs`.
2. Add one thin handler per operation.
3. Add `I{Resource}Service` and `{Resource}Service`.
4. Add DTOs and request models.
5. Register the service in DI.
6. Add service tests.

## Guardrails

- Do not inject `AppDbContext` directly into handlers unless the endpoint is truly trivial.
- Do not return `IResult` from services.
- Do not put business rules in route mapping or handlers.
- Prefer one handler class per operation.
- Keep generated code consistent with this separation even for simple CRUD endpoints.
