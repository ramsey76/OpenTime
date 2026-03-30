## 1. API Project Setup

- [x] 1.1 Add Swagger packages (`Swashbuckle.AspNetCore`) to `TimeManagement.Api` if not already present
- [x] 1.2 Configure `AddEndpointsApiExplorer` and `AddSwaggerGen` in `Program.cs`
- [x] 1.3 Add `UseSwagger` and `UseSwaggerUI` middleware in `Program.cs`
- [x] 1.4 Create `Endpoints/EndPoints.cs` abstract base class
- [x] 1.5 Create `Extensions/EndpointExtensions.cs` with `MapEndpointsFromAssembly` auto-registration
- [x] 1.6 Wire up `app.MapEndpointsFromAssembly()` in `Program.cs`

## 2. DTOs

- [x] 2.1 Create `DTOs/RoleDto.cs` with `Id`, `Name`, `Description`
- [x] 2.2 Create `DTOs/CreateRoleRequest.cs` with `Name` (required, max 50), `Description` (optional)
- [x] 2.3 Create `DTOs/UpdateRoleRequest.cs` with `Name` (required, max 50), `Description` (optional)

## 3. Service Layer

- [x] 3.1 Create `Services/Roles/IRoleService.cs` interface with `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`
- [x] 3.2 Create `Services/Roles/RoleService.cs` implementing `IRoleService` with `AppDbContext` injected
- [x] 3.3 Implement `GetAllAsync` — return all roles as `List<RoleDto>` using `AsNoTracking`
- [x] 3.4 Implement `GetByIdAsync` — return `RoleDto` or `null` if not found
- [x] 3.5 Implement `CreateAsync` — check for duplicate name (case-insensitive), persist, return result
- [x] 3.6 Implement `UpdateAsync` — find by ID, check for duplicate name on other roles, update, return result
- [x] 3.7 Implement `DeleteAsync` — find by ID, check for existing `EmployeeRole` assignments, delete, return result
- [x] 3.8 Register `IRoleService` / `RoleService` as scoped in `Program.cs`

## 4. Endpoints

- [x] 4.1 Create `Endpoints/Roles/GetRoles.cs` handler — calls `GetAllAsync`, returns `200 OK`
- [x] 4.2 Create `Endpoints/Roles/GetRoleById.cs` handler — calls `GetByIdAsync`, returns `200 OK` or `404 Not Found`
- [x] 4.3 Create `Endpoints/Roles/CreateRole.cs` handler — calls `CreateAsync`, returns `201 Created` with `Location` header, `400`, or `409`
- [x] 4.4 Create `Endpoints/Roles/UpdateRole.cs` handler — calls `UpdateAsync`, returns `200 OK`, `400`, `404`, or `409`
- [x] 4.5 Create `Endpoints/Roles/DeleteRole.cs` handler — calls `DeleteAsync`, returns `204 No Content`, `404`, or `409`
- [x] 4.6 Create `Endpoints/Roles/RoleEndpoints.cs` — registers all 5 routes under `/api/roles` with tags and typed `Produces` declarations

## 5. Validation

- [x] 5.1 Add `name` required validation in `CreateRoleRequest` and `UpdateRoleRequest` (data annotations or minimal API validation)
- [x] 5.2 Add `name` max length (50) validation matching the database constraint in `RoleConfiguration`

## 6. Verify

- [x] 6.1 Run the application and confirm all 5 endpoints appear in Swagger UI
- [x] 6.2 Verify create returns `201` with correct `Location` header
- [x] 6.3 Verify duplicate name returns `409 Conflict`
- [x] 6.4 Verify get/update/delete on unknown ID returns `404 Not Found`
- [x] 6.5 Verify delete on a role with employee assignments returns `409 Conflict`
