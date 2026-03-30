## Context

The `Employee` entity, EF Core configuration, and `DbSet<Employee>` in `AppDbContext` already exist. The missing piece is the HTTP API layer. The `department-management-api` is the reference implementation — this change follows its pattern exactly.

## Goals / Non-Goals

**Goals:**
- Expose CRUD endpoints for employee records at `/api/employees`
- Keep all business logic (duplicate checks, FK validation, field validation) in `EmployeeService`
- Thin handlers that bind input, call one service method, and map the result to HTTP

**Non-Goals:**
- Changes to the `Employee` entity or database schema
- Authentication or authorization on the new endpoints
- Filtering, sorting, or pagination of the employee list
- Managing `EmployeeRole` or `ProjectAssignment` relationships

## Decisions

### Follow the DepartmentService result pattern
Use a sealed `EmployeeResult` record with `IsSuccess`, `Value`, `Error`, and a `Status` enum (`Ok`, `NoContent`, `NotFound`, `Conflict`, `UnprocessableEntity`). This keeps handler logic uniform and avoids throwing exceptions for predictable business failures.

_Alternative considered_: Throwing domain exceptions — rejected because it couples HTTP concerns to service logic and makes unit testing harder.

### One handler class per operation
Each of the five operations (`GetEmployees`, `GetEmployeeById`, `CreateEmployee`, `UpdateEmployee`, `DeleteEmployee`) is a separate static class under `Endpoints/Employees/`. `EmployeeEndpoints` inherits `EndPoints` and registers all routes into a `/api/employees` group.

_Alternative considered_: Single handler class — rejected because it grows unbounded and mixes unrelated concerns.

### ExternalId is immutable after creation
The `UpdateEmployeeRequest` does not include `ExternalId`. Entra ID object identifiers are stable identifiers assigned at creation; changing them would break the external identity link.

_Alternative considered_: Allowing ExternalId updates — rejected because it undermines the uniqueness guarantee and the link to the identity provider.

### Department FK validated in service, not via EF exception
`EmployeeService` checks `Departments.AnyAsync(d => d.Id == request.DepartmentId)` before saving. This produces a predictable `422` response rather than relying on a database constraint violation.

_Alternative considered_: Catching `DbUpdateException` — rejected because it leaks infrastructure details and is harder to test.

## File Locations

| Artifact | Path |
|---|---|
| Response DTO | `src/TimeManagement.Api/DTOs/EmployeeDto.cs` |
| Create request | `src/TimeManagement.Api/DTOs/CreateEmployeeRequest.cs` |
| Update request | `src/TimeManagement.Api/DTOs/UpdateEmployeeRequest.cs` |
| Service interface | `src/TimeManagement.Api/Services/Employees/IEmployeeService.cs` |
| Service implementation | `src/TimeManagement.Api/Services/Employees/EmployeeService.cs` |
| Result type | `src/TimeManagement.Api/Services/Employees/EmployeeResult.cs` |
| Endpoint group | `src/TimeManagement.Api/Endpoints/Employees/EmployeeEndpoints.cs` |
| Handlers | `src/TimeManagement.Api/Endpoints/Employees/Get*.cs`, `Create*.cs`, `Update*.cs`, `Delete*.cs` |
| DI registration | `src/TimeManagement.Api/Program.cs` |
| Service tests | `src/TimeManagement.Api.Tests/Services/Employees/EmployeeServiceTests.cs` |

## Risks / Trade-offs

- **No cascade delete** — `EmployeeConfiguration` uses `DeleteBehavior.Restrict` on the Department FK. Deleting a department with employees will fail at the DB level. The department-delete endpoint already guards against this (returns `409`); the employee API does not need extra logic here.
- **List returns all employees** — no pagination. Acceptable for now given the expected employee count, but may need revisiting as the dataset grows.

## Open Questions

_(none — design is fully determined by the department reference implementation)_
