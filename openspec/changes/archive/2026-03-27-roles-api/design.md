## Context

The `Role` entity and its database configuration already exist, but there is no application layer or API layer in the project yet. The `TimeManagement.Api` project currently only has a placeholder weather forecast endpoint. This change introduces the first real feature and therefore establishes the foundational architectural patterns for all future features.

The project uses the **Minimal API Structure Pattern** (defined in `.claude/skills/api-minimal/SKILL.md`), which mandates thin handlers, service-layer business logic, auto-registered endpoint groups, and Swagger support.

## Goals / Non-Goals

**Goals:**
- Deliver a working CRUD REST API for roles
- Establish the Minimal API pattern (endpoint groups + thin handlers + service layer) as the baseline for all future features
- Keep code testable by default — services are the primary unit-test seam

**Non-Goals:**
- Authentication or authorization (out of scope for this change)
- Pagination, filtering, or sorting of role lists
- Soft-delete support
- Frontend implementation
- A separate `TimeManagement.Application` project

## Decisions

### 1. Minimal API endpoints, not controllers

**Decision:** Use ASP.NET Core Minimal API with one handler class per operation (`GetRoles`, `GetRoleById`, `CreateRole`, `UpdateRole`, `DeleteRole`), grouped under `RoleEndpoints : EndPoints`.

**Rationale:** The project's `api-minimal` skill mandates this pattern. It keeps route registration separate from handler logic and HTTP concerns separate from business logic.

### 2. Services live in `TimeManagement.Api/Services/Roles/`

**Decision:** No separate `TimeManagement.Application` project. `IRoleService` and `RoleService` live inside the API project under `Services/Roles/`.

**Rationale:** The skill places services inside the API project for simplicity. The service layer is still the clear unit-test seam and contains all business logic and EF Core access.

### 3. Direct `AppDbContext` injection into `RoleService`

**Decision:** Inject `AppDbContext` directly into the service. No repository abstraction.

**Rationale:** EF Core's `DbSet<T>` already acts as a repository. Adding a generic repository layer provides no real benefit and conflicts with the skill's guidelines.

### 4. Result types for service return values

**Decision:** Service methods return typed result objects (e.g. `RoleResult`, `RoleListResult`) rather than throwing exceptions or returning `IResult`.

**Rationale:** The skill requires services to never return `IResult` or depend on HTTP concerns. Typed result objects allow thin handlers to map outcomes to the correct HTTP response.

### 5. Standard HTTP status codes

- `GET /api/roles` → `200 OK` with list
- `GET /api/roles/{id}` → `200 OK` or `404 Not Found`
- `POST /api/roles` → `201 Created` with `Location` header
- `PUT /api/roles/{id}` → `200 OK` or `404 Not Found`
- `DELETE /api/roles/{id}` → `204 No Content` or `404 Not Found`
- Duplicate name on create/update → `409 Conflict`

### 6. Swagger enabled

**Decision:** Ensure `AddEndpointsApiExplorer` and `AddSwaggerGen` are configured in `Program.cs` with all endpoints producing typed responses for accurate Swagger documentation.

## Risks / Trade-offs

- **No abstraction over DbContext** → Unit testing the service requires an in-memory EF provider or integration test setup. Mitigation: use `UseInMemoryDatabase` in service tests.
- **Establishing the first pattern** → Naming, structure, and DI registration choices here will be copied by future features. Mitigation: follow the skill exactly and keep it clean.
- **FK constraint on delete** → Deleting a role assigned to employees will fail with a database constraint violation. Mitigation: check for existing `EmployeeRole` records before deleting and return `409 Conflict` with a clear message.
