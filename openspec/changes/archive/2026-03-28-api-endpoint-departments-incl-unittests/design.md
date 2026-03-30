## Context

The `Department` entity, its EF Core configuration, and `AppDbContext` already exist. The `roles-api` change established the Minimal API pattern for this project: endpoint groups, thin handlers, a service layer with typed results, and DTOs — all inside `TimeManagement.Api`. The `role-service-tests` change established the unit test pattern: xUnit + EF Core in-memory in `TimeManagement.Api.Tests`. This change follows both patterns exactly for Departments.

Department differs from Role in one key way: it has two identity fields (`Name` and `Code`), where `Code` is the unique business key. Delete is also constrained by the `Employees` FK.

## Goals / Non-Goals

**Goals:**
- CRUD REST API for departments at `/api/departments` following the `api-minimal` skill
- Unit tests for `DepartmentService` in the existing `TimeManagement.Api.Tests` project
- Consistent with the patterns already in place for Roles

**Non-Goals:**
- Listing employees within a department (separate concern)
- Pagination or filtering
- Authentication / authorization
- Frontend implementation

## Decisions

### 1. Follow the established Minimal API pattern exactly

**Decision:** `DepartmentEndpoints : EndPoints`, one handler class per operation, `IDepartmentService` + `DepartmentService` in `Services/Departments/`, DTOs in `DTOs/`.

**Rationale:** The pattern was established by `roles-api` and is mandated by the `api-minimal` skill. Consistency is more important than any marginal design improvement.

### 2. Two uniqueness checks: `Code` is the primary unique key, `Name` has no uniqueness constraint

**Decision:** Only `Code` is enforced as unique (matches the database index on `DepartmentConfiguration`). `Name` has no uniqueness constraint and is not checked for duplicates.

**Rationale:** The DB schema enforces uniqueness on `Code` only. Introducing a name uniqueness check would diverge from the domain model.

### 3. `DepartmentResult` typed result — same pattern as `RoleResult`

**Decision:** Use a `DepartmentResult` record with `IsSuccess`, `Value`, `Error`, and `DepartmentResultStatus` (Ok / NoContent / NotFound / Conflict).

**Rationale:** Mirrors `RoleResult` exactly. Thin handlers map `DepartmentResultStatus` to HTTP status codes without any business logic.

### 4. Delete blocked by employee assignments

**Decision:** Before deleting, check `context.Employees.AnyAsync(e => e.DepartmentId == id)` and return `Conflict` if true.

**Rationale:** The `DepartmentConfiguration` does not configure `OnDelete` behaviour for the Employees FK, which means the DB will reject the delete anyway. The service-level check gives a better error message and avoids a DB exception.

### 5. Tests go in the existing `TimeManagement.Api.Tests` project

**Decision:** Add `DepartmentServiceTests.cs` to the existing test project rather than creating a new one.

**Rationale:** The test project already exists and is wired into the solution. A separate project per service would be unnecessary fragmentation.

## Risks / Trade-offs

- **In-memory provider ignores unique index on `Code`** → Duplicate `Code` checks must be tested via service logic (same situation as Role name). Mitigation: `DepartmentService.CreateAsync` and `UpdateAsync` perform explicit `AnyAsync` checks before saving.
- **`Name` has no uniqueness constraint** → Two departments with the same name are valid. This may be surprising to API consumers but is correct per the domain model. Mitigation: document clearly in the API spec.
