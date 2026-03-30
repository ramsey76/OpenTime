## Why

Departments need to support a hierarchy so that organisational units can be nested (e.g. "Backend Engineering" under "Engineering"). Currently the `Department` entity has no parent concept, making it impossible to represent team structures. Adding an optional `ParentDepartmentId` enables this without breaking existing flat departments.

## What Changes

- New nullable `ParentDepartmentId` FK on the `Department` entity and a self-referencing relationship in EF Core.
- New EF Core migration to add the column.
- `DepartmentDto` updated to include `parentDepartmentId` (nullable).
- `CreateDepartmentRequest` and `UpdateDepartmentRequest` updated to accept an optional `parentDepartmentId`.
- `DepartmentService` enforces three integrity rules:
  1. **Single parent**: A department MAY have at most one parent (enforced by the nullable FK — a department cannot be assigned multiple parents).
  2. **Parent must exist**: If `parentDepartmentId` is provided, the referenced department must exist; otherwise `422 Unprocessable Entity` is returned.
  3. **No circular references**: A department cannot become a descendant of itself. When setting a parent, the API walks the ancestor chain of the proposed parent and rejects the request with `422 Unprocessable Entity` if a cycle would result.
- Delete is blocked with `409 Conflict` when the department has subdepartments.
- All three integrity rules are covered by unit tests in `DepartmentServiceTests`.

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `department-management`: The `Department` record storage requirement changes to include the `ParentDepartmentId` field and the single-parent and no-circular-reference invariants.
- `department-management-api`: Create, update, get, and list endpoints change to accept/return `parentDepartmentId`. Create and update enforce parent-exists and no-circular-reference rules. Delete is extended to return `409` when subdepartments are present.

## Impact

- **Domain:** `Department.cs` — add `ParentDepartmentId` (nullable `Guid?`) and `SubDepartments` / `ParentDepartment` navigation properties
- **Infrastructure:** `DepartmentConfiguration.cs` — configure self-referencing FK with `DeleteBehavior.Restrict`; new EF Core migration
- **API DTOs:** `DepartmentDto`, `CreateDepartmentRequest`, `UpdateDepartmentRequest` — add optional `ParentDepartmentId`
- **Service:** `DepartmentService` — parent existence check, ancestor-cycle detection, subdepartment deletion guard
- **Tests:** `DepartmentServiceTests` — new tests for all three integrity rules and subdepartment deletion guard
- **No changes** to roles, employees, projects, or time entries
