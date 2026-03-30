## Why

The `Department` entity and its database configuration already exist, but there is no API to manage departments. Employees are assigned to departments, so administrators need a way to create, read, update, and delete departments via REST. This change also adds unit tests for the service layer, following the pattern established by `role-service-tests`.

## What Changes

- New REST API endpoints for department management: list all, get by ID, create, update, delete.
- New `DepartmentService` in the API project under `Services/Departments/`, following the `api-minimal` skill pattern.
- New `DepartmentEndpoints` group with thin handlers at `/api/departments`.
- New unit tests for `DepartmentService` in `TimeManagement.Api.Tests`, using EF Core in-memory.
- Swagger documentation for all endpoints.

## Capabilities

### New Capabilities

- `department-management-api`: CRUD REST API for departments — create, read (single + list), update, and delete via Minimal API endpoints.
- `department-service-testing`: Unit test coverage for `DepartmentService` — success paths, not-found, duplicate code/name conflicts, and delete-with-employees conflict.

### Modified Capabilities

_(none)_

## Impact

- **API:** New endpoints at `/api/departments` via `DepartmentEndpoints : EndPoints`
- **Services:** `IDepartmentService` + `DepartmentService` in `TimeManagement.Api/Services/Departments/`
- **DTOs:** `DepartmentDto`, `CreateDepartmentRequest`, `UpdateDepartmentRequest` in `TimeManagement.Api/DTOs/`
- **Tests:** New tests in `TimeManagement.Api.Tests/Services/DepartmentServiceTests.cs`
- **Database:** No schema changes — `Departments` table already exists in the `timemanagement` schema
- **Dependencies:** No new packages required
